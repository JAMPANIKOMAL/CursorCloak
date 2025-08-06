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

namespace CursorCloak.UI
{
    public partial class MainWindow : Window
    {
        private HwndSource _hwndSource;

        public MainWindow()
        {
            InitializeComponent();
            LoadSettingsAndApply();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            _hwndSource.AddHook(HwndHook);
            HotKeyManager.RegisterHotKeys(_hwndSource.Handle);
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveSettings();
            _hwndSource.RemoveHook(HwndHook);
            HotKeyManager.UnregisterHotKeys(_hwndSource.Handle);
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
                CursorEngine.HideSystemCursor();
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
                Directory.CreateDirectory(Path.GetDirectoryName(_settingsFilePath));
                string json = JsonSerializer.Serialize(settings);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception ex) { /* Handle potential saving errors */ }
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
            catch (Exception ex) { /* Handle potential loading errors */ }
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
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true);
                if (isEnabled)
                {
                    // Get the path to the currently running .exe file
                    string exePath = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
                    key.SetValue(AppName, $"\"{exePath}\"");
                }
                else
                {
                    key.DeleteValue(AppName, false);
                }
                key.Close();
            }
            catch (Exception ex) { /* Handle potential registry access errors */ }
        }
    }

    // --- Engine and Hotkey Manager ---
    public static class CursorEngine
    {
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

        public static void HideSystemCursor()
        {
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
            // *** FIX: This is the new, more reliable method to restore the cursor ***
            // It tells Windows to reload the current user's cursor scheme from the registry.
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
