using System;
using System.Runtime.InteropServices;

namespace CursorCloak.UI.Services
{
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
}
