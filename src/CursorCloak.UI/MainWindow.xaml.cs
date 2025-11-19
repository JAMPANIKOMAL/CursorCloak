using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Win32;
using System.Windows.Forms; // Required for NotifyIcon
using System.Drawing; // Required for SystemIcons
using CursorCloak.UI.Services;
using CursorCloak.UI.Models;

namespace CursorCloak.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// Handles the main UI window, system tray integration, and user settings.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields & Properties

        /// <summary>
        /// Gets the current instance of the MainWindow.
        /// </summary>
        public static MainWindow? CurrentInstance { get; private set; }

        private System.Windows.Threading.DispatcherTimer? _autoHideTimer;
        private DateTime _lastMouseMove = DateTime.Now;
        private bool _isCursorHidden = false;
        private HwndSource? _hwndSource;
        private bool _allowClose = false;
        private NotifyIcon? _notifyIcon;

        // Win32 API constants and imports for window styling
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion

        #region Constructor & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            CurrentInstance = this;
            InitializeComponent();
            SetWindowIcon();
            LoadSettingsAndApply();
            InitializeSystemTray();

            // Event subscriptions
            this.MouseMove += MainWindow_MouseMove;
            this.Closing += MainWindow_Closing;
            this.StateChanged += MainWindow_StateChanged;
        }

        /// <summary>
        /// Handles the SourceInitialized event to attach window hooks.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            _hwndSource.AddHook(HwndHook);
            
            // Initialize engine and register hotkeys
            CursorEngine.Initialize();
            HotKeyManager.RegisterHotKeys(_hwndSource.Handle);
        }

        #endregion

        #region Core Logic

        /// <summary>
        /// Callback for global mouse movement events.
        /// Handles auto-hide logic reset on movement.
        /// </summary>
        public void OnGlobalMouseMove()
        {
            if (AutoHideToggle.IsChecked == true)
            {
                _lastMouseMove = DateTime.Now;
                if (_isCursorHidden)
                {
                    CursorEngine.ShowSystemCursor();
                    _isCursorHidden = false;
                }
            }
        }

        /// <summary>
        /// Starts the auto-hide timer.
        /// </summary>
        private void StartAutoHideTimer()
        {
            if (_autoHideTimer == null)
            {
                _autoHideTimer = new System.Windows.Threading.DispatcherTimer();
                _autoHideTimer.Interval = TimeSpan.FromSeconds(1);
                _autoHideTimer.Tick += AutoHideTimer_Tick;
            }
            _lastMouseMove = DateTime.Now;
            _autoHideTimer.Start();
        }

        /// <summary>
        /// Stops the auto-hide timer.
        /// </summary>
        private void StopAutoHideTimer()
        {
            _autoHideTimer?.Stop();
        }

        /// <summary>
        /// Handles the tick event of the auto-hide timer.
        /// Checks inactivity duration and hides cursor if threshold is met.
        /// </summary>
        private void AutoHideTimer_Tick(object? sender, EventArgs e)
        {
            if (AutoHideToggle.IsChecked == true && int.TryParse(AutoHideTimeoutBox.Text, out int timeout))
            {
                if ((DateTime.Now - _lastMouseMove).TotalSeconds >= Math.Max(1, timeout))
                {
                    if (!_isCursorHidden)
                    {
                        CursorEngine.HideSystemCursor();
                        _isCursorHidden = true;
                    }
                }
            }
        }

        #endregion

        #region UI Event Handlers

        private void AutoHideToggle_Click(object sender, RoutedEventArgs e)
        {
            bool enabled = AutoHideToggle.IsChecked == true;
            AutoHideTimeoutBox.IsEnabled = enabled;
            SaveSettings();
            if (enabled)
            {
                StartAutoHideTimer();
            }
            else
            {
                StopAutoHideTimer();
                CursorEngine.ShowSystemCursor();
            }
        }

        private void MainWindow_MouseMove(object? sender, System.Windows.Input.MouseEventArgs e)
        {
            // No-op: logic handled by global mouse hook, but kept for local window events if needed
        }

        private void MainToggle_Click(object sender, RoutedEventArgs e)
        {
            if (MainToggle.IsChecked == true)
            {
                CursorEngine.HideSystemCursor();
            }
            else
            {
                CursorEngine.ShowSystemCursor();
            }
        }

        private void StartupCheck_Click(object sender, RoutedEventArgs e)
        {
            bool isEnabled = StartupCheck.IsChecked == true;
            StartupManager.SetStartup(isEnabled);
            
            SaveSettings();
            
            // Verify setting application
            bool actuallyEnabled = StartupManager.IsStartupEnabled();
            if (actuallyEnabled != isEnabled)
            {
                StartupCheck.IsChecked = actuallyEnabled;
                System.Windows.MessageBox.Show(
                    "Unable to modify startup settings. Please run as administrator to change startup behavior.",
                    "Startup Settings", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
            }
        }

        private void TitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Window & Tray Management

        /// <summary>
        /// Sets the window icon from resources.
        /// </summary>
        private void SetWindowIcon()
        {
            try
            {
                var iconUri = new Uri("pack://application:,,,/app-icon.ico");
                this.Icon = new System.Windows.Media.Imaging.BitmapImage(iconUri);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load window icon: {ex.Message}");
            }
        }

        /// <summary>
        /// Initializes the system tray icon and context menu.
        /// </summary>
        private void InitializeSystemTray()
        {
            _notifyIcon = new NotifyIcon();
            
            try
            {
                var resourceStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/app-icon.ico"));
                if (resourceStream != null)
                {
                    _notifyIcon.Icon = new System.Drawing.Icon(resourceStream.Stream);
                }
                else
                {
                    var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "icons", "app-icon.ico");
                    if (File.Exists(iconPath))
                    {
                        _notifyIcon.Icon = new System.Drawing.Icon(iconPath);
                    }
                    else
                    {
                        _notifyIcon.Icon = SystemIcons.Application;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load tray icon: {ex.Message}");
                _notifyIcon.Icon = SystemIcons.Application;
            }
            
            _notifyIcon.Text = "CursorCloak - Click to show";
            _notifyIcon.Visible = true;
            
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Show CursorCloak", null, (s, e) => ShowWindow());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Hide Cursor (Alt+H)", null, (s, e) => {
                CursorEngine.HideSystemCursor();
                MainToggle.IsChecked = true;
            });
            contextMenu.Items.Add("Show Cursor (Alt+S)", null, (s, e) => {
                CursorEngine.ShowSystemCursor();
                MainToggle.IsChecked = false;
            });
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Exit", null, (s, e) => ExitApplication());
            
            _notifyIcon.ContextMenuStrip = contextMenu;
            _notifyIcon.DoubleClick += (s, e) => ShowWindow();
        }

        /// <summary>
        /// Shows the main window and restores it from the system tray.
        /// </summary>
        private void ShowWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.ShowInTaskbar = true;
            
            if (_hwndSource != null)
            {
                IntPtr hwnd = _hwndSource.Handle;
                int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, exStyle & ~WS_EX_TOOLWINDOW);
            }
            
            this.Activate();
            _notifyIcon!.Visible = true;
        }

        /// <summary>
        /// Exits the application completely.
        /// </summary>
        private void ExitApplication()
        {
            _allowClose = true;
            _notifyIcon?.Dispose();
            System.Windows.Application.Current.Shutdown();
        }
        
        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                
                if (_hwndSource != null)
                {
                    IntPtr hwnd = _hwndSource.Handle;
                    int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                    SetWindowLong(hwnd, GWL_EXSTYLE, exStyle | WS_EX_TOOLWINDOW);
                }
                
                this.Hide();
                _notifyIcon!.Visible = true;
                
                if (UserConfig.Load().ShowNotifications)
                {
                    _notifyIcon.ShowBalloonTip(2000, "CursorCloak", "Application minimized to system tray", System.Windows.Forms.ToolTipIcon.Info);
                }
            }
        }
        
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_allowClose)
            {
                e.Cancel = true;
                this.WindowState = WindowState.Minimized;
            }
        }

        /// <summary>
        /// Cleans up resources when the window is closed.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            SaveSettings();
            _hwndSource?.RemoveHook(HwndHook);
            HotKeyManager.UnregisterHotKeys(_hwndSource?.Handle ?? IntPtr.Zero);
            _notifyIcon?.Dispose();
            base.OnClosed(e);
        }

        /// <summary>
        /// Window procedure hook for handling low-level messages (Hotkeys).
        /// </summary>
        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY)
            {
                int id = wParam.ToInt32();
                if (id == HotKeyManager.HIDE_HOTKEY_ID)
                {
                    CursorEngine.HideSystemCursor();
                    MainToggle.IsChecked = true;
                }
                else if (id == HotKeyManager.SHOW_HOTKEY_ID)
                {
                    CursorEngine.ShowSystemCursor();
                    MainToggle.IsChecked = false;
                }
                handled = true;
            }
            return IntPtr.Zero;
        }

        #endregion

        #region Settings Management

        /// <summary>
        /// Loads settings from disk and applies them to the UI.
        /// </summary>
        private void LoadSettingsAndApply()
        {
            var settings = SettingsManager.Load();
            MainToggle.IsChecked = settings.IsHidingEnabled;
            StartupCheck.IsChecked = settings.StartWithWindows;
            AutoHideToggle.IsChecked = settings.AutoHideCursor;
            AutoHideTimeoutBox.Text = settings.AutoHideTimeoutSeconds.ToString();
            AutoHideTimeoutBox.IsEnabled = settings.AutoHideCursor;

            if (settings.IsHidingEnabled)
            {
                this.SourceInitialized += (s, a) => CursorEngine.HideSystemCursor();
            }

            if (settings.AutoHideCursor)
            {
                StartAutoHideTimer();
            }
        }

        /// <summary>
        /// Saves current UI state to settings.
        /// </summary>
        private void SaveSettings()
        {
            var settings = new Settings
            {
                IsHidingEnabled = MainToggle.IsChecked == true,
                StartWithWindows = StartupCheck.IsChecked == true,
                AutoHideCursor = AutoHideToggle.IsChecked == true,
                AutoHideTimeoutSeconds = int.TryParse(AutoHideTimeoutBox.Text, out int t) ? Math.Max(1, t) : 5
            };
            SettingsManager.Save(settings);
        }

        #endregion
    }
}
