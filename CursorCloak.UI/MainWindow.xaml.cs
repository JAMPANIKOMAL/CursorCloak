/* =================================================================================== */
/* FILE 2: MainWindow.xaml.cs (Milestone 3 - Persistence Logic)                       */
/* This file now contains the logic for saving/loading settings and UI interaction.   */
/* =================================================================================== */
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

namespace CursorCloak.UI
{
    public partial class MainWindow : Window
    {
        private HwndSource? _hwndSource;
        private bool _allowClose = false;
        private NotifyIcon? _notifyIcon;
        
        // Windows API imports for hiding from Alt+Tab
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public MainWindow()
        {
            InitializeComponent();
            SetWindowIcon();
            LoadSettingsAndApply();
            InitializeSystemTray();
            
            // Handle background mode - minimize to system tray when closed
            this.Closing += MainWindow_Closing;
            this.StateChanged += MainWindow_StateChanged;
        }

        private void SetWindowIcon()
        {
            try
            {
                // Use pack URI for embedded resource
                var iconUri = new Uri("pack://application:,,,/Resources/app-icon.ico");
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
                var resourceStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resources/app-icon.ico"));
                if (resourceStream != null)
                {
                    _notifyIcon.Icon = new System.Drawing.Icon(resourceStream.Stream);
                }
                else
                {
                    // Fallback to file system path
                    var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "app-icon.ico");
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
            StartupManager.SetStartup(StartupCheck.IsChecked == true);
        }

        private void LoadSettingsAndApply()
        {
            var settings = SettingsManager.Load();
            MainToggle.IsChecked = settings.IsHidingEnabled;
            StartupCheck.IsChecked = settings.StartWithWindows;

            if (settings.IsHidingEnabled)
            {
                // We delay the actual hiding until the window is initialized
                this.SourceInitialized += (s, a) => CursorEngine.HideSystemCursor();
            }
        }

        private void SaveSettings()
        {
            var settings = new Settings
            {
                IsHidingEnabled = MainToggle.IsChecked == true,
                StartWithWindows = StartupCheck.IsChecked == true
            };
            SettingsManager.Save(settings);
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

    // --- Settings Management ---
    public class Settings
    {
        public bool IsHidingEnabled { get; set; }
        public bool StartWithWindows { get; set; }
    }

    public static class SettingsManager
    {
        private static readonly string _settingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CursorCloak", "settings.json");

        public static void Save(Settings settings)
        {
            try
            {
                var directoryPath = Path.GetDirectoryName(_settingsFilePath);
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string json = JsonSerializer.Serialize(settings);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception)
            {
                // Silently handle potential saving errors
                System.Diagnostics.Debug.WriteLine("Failed to save settings");
            }
        }

        public static Settings Load()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string json = File.ReadAllText(_settingsFilePath);
                    return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
                }
            }
            catch (Exception)
            {
                // Silently handle potential loading errors
                System.Diagnostics.Debug.WriteLine("Failed to load settings");
            }
            return new Settings(); // Return default settings if file doesn't exist or is corrupt
        }
    }

    // --- Startup Management ---
    public static class StartupManager
    {
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "CursorCloak";

        public static void SetStartup(bool isEnabled)
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key != null)
                    {
                        if (isEnabled)
                        {
                            string exePath = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
                            key.SetValue(AppName, $"\"{exePath}\"");
                        }
                        else
                        {
                            key.DeleteValue(AppName, false);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Silently handle potential registry access errors
                System.Diagnostics.Debug.WriteLine("Failed to modify startup registry entry");
            }
        }
    }

    // --- Engine and Hotkey Manager ---
    public static class CursorEngine
    {
        // --- API Imports ---
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetSystemCursor(IntPtr hcur, uint id);
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref ICONINFO piconinfo);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, byte[] lpvBits);
        [DllImport("user32.dll")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        private const uint OCR_NORMAL = 32512;
        private const uint OCR_IBEAM = 32513;
        private const uint OCR_HAND = 32649;
        private const uint SPI_SETCURSORS = 0x0057;
        private const uint SPIF_SENDWININICHANGE = 0x02;

        // *** FIX: Removed readonly and initialized in a separate method ***
        private static IntPtr originalNormalCursor;
        private static IntPtr originalIBeamCursor;
        private static IntPtr originalHandCursor;
        private static bool isInitialized = false;

        public static void Initialize()
        {
            if (isInitialized) return;
            originalNormalCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_NORMAL));
            originalIBeamCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_IBEAM));
            originalHandCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_HAND));
            isInitialized = true;
        }

        public static void HideSystemCursor()
        {
            if (!isInitialized) Initialize();
            var andMask = new byte[32 * 4];
            var xorMask = new byte[32 * 4];
            for (int i = 0; i < andMask.Length; i++) { andMask[i] = 0xFF; xorMask[i] = 0x00; }

            ICONINFO iconInfo = new ICONINFO { fIcon = false, xHotspot = 0, yHotspot = 0, hbmMask = CreateBitmap(32, 32, 1, 1, andMask), hbmColor = CreateBitmap(32, 32, 1, 32, xorMask) };
            IntPtr blankCursorHandle = CreateIconIndirect(ref iconInfo);

            SetSystemCursor(CopyIcon(blankCursorHandle), OCR_NORMAL);
            SetSystemCursor(CopyIcon(blankCursorHandle), OCR_IBEAM);
            SetSystemCursor(CopyIcon(blankCursorHandle), OCR_HAND);
        }

        public static void ShowSystemCursor()
        {
            if (!isInitialized) Initialize();
            SystemParametersInfo(SPI_SETCURSORS, 0, IntPtr.Zero, SPIF_SENDWININICHANGE);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO { public bool fIcon; public int xHotspot; public int yHotspot; public IntPtr hbmMask; public IntPtr hbmColor; }
    }

    public static class HotKeyManager
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int HIDE_HOTKEY_ID = 9000;
        public const int SHOW_HOTKEY_ID = 9001;
        private const uint MOD_ALT = 0x0001;
        private const uint VK_H = 0x48;
        private const uint VK_S = 0x53;

        public static void RegisterHotKeys(IntPtr handle)
        {
            RegisterHotKey(handle, HIDE_HOTKEY_ID, MOD_ALT, VK_H);
            RegisterHotKey(handle, SHOW_HOTKEY_ID, MOD_ALT, VK_S);
        }

        public static void UnregisterHotKeys(IntPtr handle)
        {
            UnregisterHotKey(handle, HIDE_HOTKEY_ID);
            UnregisterHotKey(handle, SHOW_HOTKEY_ID);
        }
    }
}
