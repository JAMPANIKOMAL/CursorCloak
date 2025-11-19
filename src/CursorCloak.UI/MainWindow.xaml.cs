using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Win32; // Required for Registry access
using System.Windows.Forms; // Required for NotifyIcon
using System.Drawing; // Required for SystemIcons
using CursorCloak.UI.Services;
using CursorCloak.UI.Models;

namespace CursorCloak.UI
{
    public partial class MainWindow : Window
    {
        public static MainWindow? CurrentInstance { get; private set; }
        private System.Windows.Threading.DispatcherTimer? _autoHideTimer;
        private DateTime _lastMouseMove = DateTime.Now;
        private bool _isCursorHidden = false;
        private HwndSource? _hwndSource;
        private bool _allowClose = false;
        private NotifyIcon? _notifyIcon;

        // Win32 API for hiding from Alt+Tab
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public MainWindow()
        {
            CurrentInstance = this;
            InitializeComponent();
            SetWindowIcon();
            LoadSettingsAndApply();
            InitializeSystemTray();

            // Mouse move event for auto hide
            this.MouseMove += MainWindow_MouseMove;
            // Install global mouse hook via static helper
            //GlobalMouseHook.Install(OnGlobalMouseMove);
            // Handle background mode - minimize to system tray when closed
            this.Closing += MainWindow_Closing;
            this.StateChanged += MainWindow_StateChanged;
        }

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
            // No-op: logic now handled by global mouse hook
        }

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

        private void StopAutoHideTimer()
        {
            _autoHideTimer?.Stop();
        }

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

        private void SetWindowIcon()
        {
            try
            {
                // Use pack URI for embedded resource
                var iconUri = new Uri("pack://application:,,,/app-icon.ico");
                this.Icon = new System.Windows.Media.Imaging.BitmapImage(iconUri);
            }
            catch (Exception ex)
            {
                // If icon loading fails, just continue without it
                System.Diagnostics.Debug.WriteLine($"Failed to load window icon: {ex.Message}");
            }
    }

        private void InitializeSystemTray()
        {
            _notifyIcon = new NotifyIcon();
            
            // Try to load the icon from embedded resources first, then file system as fallback
            try
            {
                // First try to load from embedded resources
                var resourceStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/app-icon.ico"));
                if (resourceStream != null)
                {
                    _notifyIcon.Icon = new System.Drawing.Icon(resourceStream.Stream);
                }
                else
                {
                    // Fallback to file system path
                    var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "icons", "app-icon.ico");
                    if (File.Exists(iconPath))
                    {
                        _notifyIcon.Icon = new System.Drawing.Icon(iconPath);
                    }
                    else
                    {
                        // Use a default system icon if our icon file is not found
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
            _notifyIcon.Visible = true; // Always show tray icon
            
            // Create context menu for tray icon
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

        private void ShowWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.ShowInTaskbar = true;
            
            // Remove WS_EX_TOOLWINDOW to make it appear in Alt+Tab again
            if (_hwndSource != null)
            {
                IntPtr hwnd = _hwndSource.Handle;
                int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, exStyle & ~WS_EX_TOOLWINDOW);
            }
            
            this.Activate();
            _notifyIcon!.Visible = true; // Keep tray icon visible
        }

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
                
                // Set WS_EX_TOOLWINDOW to hide from Alt+Tab task switcher
                if (_hwndSource != null)
                {
                    IntPtr hwnd = _hwndSource.Handle;
                    int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                    SetWindowLong(hwnd, GWL_EXSTYLE, exStyle | WS_EX_TOOLWINDOW);
                }
                
                this.Hide(); // Completely hide the window from Task View
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
                // Don't actually close, just minimize to system tray
                e.Cancel = true;
                this.WindowState = WindowState.Minimized;
                // The StateChanged event handler will take care of hiding from taskbar and showing tray icon
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            _hwndSource.AddHook(HwndHook);
            
            // *** FIX: Initialize the engine and hotkeys only when the window is ready ***
            CursorEngine.Initialize();
            HotKeyManager.RegisterHotKeys(_hwndSource.Handle);
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveSettings();
            _hwndSource?.RemoveHook(HwndHook);
            HotKeyManager.UnregisterHotKeys(_hwndSource?.Handle ?? IntPtr.Zero);
            _notifyIcon?.Dispose();
            //GlobalMouseHook.Uninstall();
            base.OnClosed(e);
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
            
            // Save the settings to persist the change
            SaveSettings();
            
            // Verify the startup setting was applied correctly
            bool actuallyEnabled = StartupManager.IsStartupEnabled();
            if (actuallyEnabled != isEnabled)
            {
                // If the setting didn't apply, revert the checkbox and notify user
                StartupCheck.IsChecked = actuallyEnabled;
                System.Windows.MessageBox.Show(
                    "Unable to modify startup settings. Please run as administrator to change startup behavior.",
                    "Startup Settings", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
            }
        }

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
                // We delay the actual hiding until the window is initialized
                this.SourceInitialized += (s, a) => CursorEngine.HideSystemCursor();
            }

            if (settings.AutoHideCursor)
            {
                StartAutoHideTimer();
            }
        }

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

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY)
            {
                int id = wParam.ToInt32();
                if (id == HotKeyManager.HIDE_HOTKEY_ID)
                {
                    CursorEngine.HideSystemCursor();
                    MainToggle.IsChecked = true; // Sync UI
                }
                else if (id == HotKeyManager.SHOW_HOTKEY_ID)
                {
                    CursorEngine.ShowSystemCursor();
                    MainToggle.IsChecked = false; // Sync UI
                }
                handled = true;
            }
            return IntPtr.Zero;
        }
    }
}
