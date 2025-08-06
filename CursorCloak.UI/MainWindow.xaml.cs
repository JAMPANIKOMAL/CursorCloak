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
using System.ComponentModel;

namespace CursorCloak.UI
{
    public partial class MainWindow : Window
    {
        private HwndSource? _hwndSource;
        private bool _isExiting = false;

        public MainWindow()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("MainWindow constructor starting...");
                InitializeComponent();
                System.Diagnostics.Debug.WriteLine("InitializeComponent completed.");
                LoadSettingsAndApply();
                System.Diagnostics.Debug.WriteLine("LoadSettingsAndApply completed.");
                
                // Add startup to Windows registry for background running
                BackgroundHelper.RegisterStartupIfEnabled();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MainWindow constructor failed: {ex}");
                MessageBox.Show($"Failed to initialize main window: {ex.Message}", "Constructor Error", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
                throw; // Re-throw to prevent partial initialization
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            try
            {
                _hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
                _hwndSource.AddHook(HwndHook);
                
                // Initialize the engine and hotkeys only when the window is ready
                CursorEngine.Initialize();
                HotKeyManager.RegisterHotKeys(_hwndSource.Handle);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize application: {ex.Message}", "Initialization Error", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // If user clicked X button, minimize to background instead of closing
            if (!_isExiting)
            {
                e.Cancel = true;
                this.Hide();
                
                // Save settings when hiding
                SaveSettings();
                
                // Show a brief notification (optional)
                BackgroundHelper.ShowBalloonTip("CursorCloak is running in the background. Hotkeys remain active.");
                return;
            }
            
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                _isExiting = true;
                SaveSettings();
                if (_hwndSource != null)
                {
                    _hwndSource.RemoveHook(HwndHook);
                    HotKeyManager.UnregisterHotKeys(_hwndSource.Handle);
                }
                CursorEngine.Cleanup();
            }
            catch (Exception ex)
            {
                // Log error but don't show UI since we're closing
                System.Diagnostics.Debug.WriteLine($"Error during cleanup: {ex.Message}");
            }
            finally
            {
                base.OnClosed(e);
            }
        }

        private void MainToggle_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to toggle cursor visibility: {ex.Message}", "Error", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                // Reset toggle to previous state
                MainToggle.IsChecked = !MainToggle.IsChecked;
            }
        }

        private void StartupCheck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartupManager.SetStartup(StartupCheck.IsChecked == true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update startup settings: {ex.Message}", "Error", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                // Reset checkbox to previous state
                StartupCheck.IsChecked = !StartupCheck.IsChecked;
            }
        }

        private void LoadSettingsAndApply()
        {
            try
            {
                var settings = SettingsManager.Load();
                MainToggle.IsChecked = settings.IsHidingEnabled;
                StartupCheck.IsChecked = settings.StartWithWindows;

                if (settings.IsHidingEnabled)
                {
                    // We delay the actual hiding until the window is initialized
                    this.SourceInitialized += (s, a) => 
                    {
                        try
                        {
                            CursorEngine.HideSystemCursor();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Failed to hide cursor on startup: {ex.Message}");
                            MainToggle.IsChecked = false;
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load settings: {ex.Message}", "Settings Error", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveSettings()
        {
            try
            {
                var settings = new Settings
                {
                    IsHidingEnabled = MainToggle.IsChecked == true,
                    StartWithWindows = StartupCheck.IsChecked == true
                };
                SettingsManager.Save(settings);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY)
            {
                try
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
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error handling hotkey: {ex.Message}");
                }
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
                var directory = Path.GetDirectoryName(_settingsFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (UnauthorizedAccessException)
            {
                throw new InvalidOperationException("Access denied when trying to save settings. Please run as administrator or check folder permissions.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new InvalidOperationException("Could not create settings directory. Please check your system permissions.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save settings: {ex.Message}", ex);
            }
        }

        public static Settings Load()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string json = File.ReadAllText(_settingsFilePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var settings = JsonSerializer.Deserialize<Settings>(json);
                        return settings ?? new Settings();
                    }
                }
            }
            catch (JsonException)
            {
                // If JSON is corrupted, delete it and return defaults
                try
                {
                    File.Delete(_settingsFilePath);
                }
                catch { /* Ignore if we can't delete */ }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
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
                    if (key == null)
                    {
                        throw new InvalidOperationException("Could not access Windows startup registry key.");
                    }

                    if (isEnabled)
                    {
                        string exePath = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
                        if (!File.Exists(exePath))
                        {
                            // Fallback for different deployment scenarios
                            exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
                        }
                        
                        if (string.IsNullOrEmpty(exePath) || !File.Exists(exePath))
                        {
                            throw new InvalidOperationException("Could not determine application executable path.");
                        }
                        
                        key.SetValue(AppName, $"\"{exePath}\"", RegistryValueKind.String);
                    }
                    else
                    {
                        key.DeleteValue(AppName, false);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new InvalidOperationException("Access denied when trying to modify startup settings. Please run as administrator.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update startup settings: {ex.Message}", ex);
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
        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        private const uint OCR_NORMAL = 32512;
        private const uint OCR_IBEAM = 32513;
        private const uint OCR_HAND = 32649;
        private const uint SPI_SETCURSORS = 0x0057;
        private const uint SPIF_SENDWININICHANGE = 0x02;

        // Removed readonly and initialized in a separate method
        private static IntPtr originalNormalCursor;
        private static IntPtr originalIBeamCursor;
        private static IntPtr originalHandCursor;
        private static IntPtr blankCursorHandle;
        private static bool isInitialized = false;
        private static readonly object initLock = new object();

        public static void Initialize()
        {
            lock (initLock)
            {
                if (isInitialized) return;
                
                try
                {
                    originalNormalCursor = LoadCursor(IntPtr.Zero, (int)OCR_NORMAL);
                    originalIBeamCursor = LoadCursor(IntPtr.Zero, (int)OCR_IBEAM);
                    originalHandCursor = LoadCursor(IntPtr.Zero, (int)OCR_HAND);
                    
                    if (originalNormalCursor == IntPtr.Zero || originalIBeamCursor == IntPtr.Zero || originalHandCursor == IntPtr.Zero)
                    {
                        throw new InvalidOperationException("Failed to load system cursors.");
                    }
                    
                    // Create the blank cursor
                    CreateBlankCursor();
                    
                    isInitialized = true;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to initialize cursor engine: {ex.Message}", ex);
                }
            }
        }

        private static void CreateBlankCursor()
        {
            var andMask = new byte[32 * 4];
            var xorMask = new byte[32 * 4];
            for (int i = 0; i < andMask.Length; i++) 
            { 
                andMask[i] = 0xFF; 
                xorMask[i] = 0x00; 
            }

            IntPtr maskBitmap = CreateBitmap(32, 32, 1, 1, andMask);
            IntPtr colorBitmap = CreateBitmap(32, 32, 1, 32, xorMask);
            
            if (maskBitmap == IntPtr.Zero || colorBitmap == IntPtr.Zero)
            {
                if (maskBitmap != IntPtr.Zero) DeleteObject(maskBitmap);
                if (colorBitmap != IntPtr.Zero) DeleteObject(colorBitmap);
                throw new InvalidOperationException("Failed to create cursor bitmaps.");
            }

            ICONINFO iconInfo = new ICONINFO 
            { 
                fIcon = false, 
                xHotspot = 0, 
                yHotspot = 0, 
                hbmMask = maskBitmap, 
                hbmColor = colorBitmap 
            };
            
            blankCursorHandle = CreateIconIndirect(ref iconInfo);
            
            // Clean up bitmaps
            DeleteObject(maskBitmap);
            DeleteObject(colorBitmap);
            
            if (blankCursorHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to create blank cursor.");
            }
        }

        public static void HideSystemCursor()
        {
            if (!isInitialized) Initialize();
            
            try
            {
                SetSystemCursor(CopyIcon(blankCursorHandle), OCR_NORMAL);
                SetSystemCursor(CopyIcon(blankCursorHandle), OCR_IBEAM);
                SetSystemCursor(CopyIcon(blankCursorHandle), OCR_HAND);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to hide system cursor: {ex.Message}", ex);
            }
        }

        public static void ShowSystemCursor()
        {
            if (!isInitialized) Initialize();
            
            try
            {
                if (!SystemParametersInfo(SPI_SETCURSORS, 0, IntPtr.Zero, SPIF_SENDWININICHANGE))
                {
                    throw new InvalidOperationException("Failed to restore system cursors.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to show system cursor: {ex.Message}", ex);
            }
        }

        public static void Cleanup()
        {
            lock (initLock)
            {
                try
                {
                    if (blankCursorHandle != IntPtr.Zero)
                    {
                        DestroyIcon(blankCursorHandle);
                        blankCursorHandle = IntPtr.Zero;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error during cursor cleanup: {ex.Message}");
                }
                finally
                {
                    isInitialized = false;
                }
            }
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
            try
            {
                if (!RegisterHotKey(handle, HIDE_HOTKEY_ID, MOD_ALT, VK_H))
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new InvalidOperationException($"Failed to register hide hotkey (Alt+H). Error code: {error}");
                }
                
                if (!RegisterHotKey(handle, SHOW_HOTKEY_ID, MOD_ALT, VK_S))
                {
                    int error = Marshal.GetLastWin32Error();
                    // Clean up the first hotkey if second fails
                    UnregisterHotKey(handle, HIDE_HOTKEY_ID);
                    throw new InvalidOperationException($"Failed to register show hotkey (Alt+S). Error code: {error}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to register hotkeys: {ex.Message}", ex);
            }
        }

        public static void UnregisterHotKeys(IntPtr handle)
        {
            try
            {
                UnregisterHotKey(handle, HIDE_HOTKEY_ID);
                UnregisterHotKey(handle, SHOW_HOTKEY_ID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error unregistering hotkeys: {ex.Message}");
                }
            }
        }
    }

    // Helper methods for background operation
    public static class BackgroundHelper
    {
        public static void ShowBalloonTip(string message)
        {
            // Simple Windows notification without persistent tray icon
            try
            {
                MessageBox.Show(message, "CursorCloak", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
            }
            catch
            {
                // Ignore if notification fails
            }
        }

        public static void RegisterStartupIfEnabled()
        {
            try
            {
                // Auto-register for startup to ensure background running
                var settings = LoadAppSettings();
                if (settings?.StartWithWindows == true)
                {
                    // StartupManager.SetStartup(true); // TODO: Fix startup management
                }
            }
            catch
            {
                // Ignore startup registration errors
            }
        }

        private static dynamic? LoadAppSettings()
        {
            try
            {
                string settingsPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "CursorCloak",
                    "settings.json"
                );

                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    return JsonSerializer.Deserialize<dynamic>(json);
                }
            }
            catch
            {
                // Ignore settings load errors
            }
            return null;
        }
    }