/* =================================================================================== */
/* FILE 2: MainWindow.xaml.cs (The NEW Code-Behind Logic)                             */
/* Includes logic for the new custom title bar and buttons.                           */
/* =================================================================================== */
using System;
using System.Windows;
using System.Windows.Input; // Required for MouseButtonEventArgs
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace CursorCloak.UI
{
    public partial class MainWindow : Window
    {
        private HwndSource _hwndSource;

        public MainWindow()
        {
            InitializeComponent();
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
            _hwndSource.RemoveHook(HwndHook);
            HotKeyManager.UnregisterHotKeys(_hwndSource.Handle);
            base.OnClosed(e);
        }

        // --- Hotkey Message Listener ---

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY)
            {
                int id = wParam.ToInt32();
                if (id == HotKeyManager.HIDE_HOTKEY_ID)
                {
                    CursorEngine.HideSystemCursor();
                }
                else if (id == HotKeyManager.SHOW_HOTKEY_ID)
                {
                    CursorEngine.ShowSystemCursor();
                }
                handled = true;
            }
            return IntPtr.Zero;
        }
    }

    // --- Engine and Hotkey Manager (No Changes) ---

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

        private const uint OCR_NORMAL = 32512;
        private const uint OCR_IBEAM = 32513;
        private const uint OCR_HAND = 32649;

        private static readonly IntPtr originalNormalCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_NORMAL));
        private static readonly IntPtr originalIBeamCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_IBEAM));
        private static readonly IntPtr originalHandCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_HAND));

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
            SetSystemCursor(CopyIcon(originalNormalCursor), OCR_NORMAL);
            SetSystemCursor(CopyIcon(originalIBeamCursor), OCR_IBEAM);
            SetSystemCursor(CopyIcon(originalHandCursor), OCR_HAND);
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
