using System;
using System.Runtime.InteropServices;

namespace CursorCloak.UI.Services
{
    /// <summary>
    /// Provides low-level cursor manipulation using Windows API.
    /// Handles hiding and showing the system cursor by replacing it with a transparent icon.
    /// </summary>
    public static class CursorEngine
    {
        #region Native Methods

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

        #endregion

        #region Constants & Fields

        private const uint OCR_NORMAL = 32512;
        private const uint OCR_IBEAM = 32513;
        private const uint OCR_HAND = 32649;
        private const uint SPI_SETCURSORS = 0x0057;
        private const uint SPIF_SENDWININICHANGE = 0x02;

        private static IntPtr _originalNormalCursor;
        private static IntPtr _originalIBeamCursor;
        private static IntPtr _originalHandCursor;
        private static bool _isInitialized = false;

        #endregion

        /// <summary>
        /// Initializes the cursor engine by backing up the original system cursors.
        /// This method should be called once at application startup.
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;

            // Backup original cursors
            _originalNormalCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_NORMAL));
            _originalIBeamCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_IBEAM));
            _originalHandCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_HAND));
            
            _isInitialized = true;
        }

        /// <summary>
        /// Hides the system cursor by replacing standard cursors with a transparent icon.
        /// </summary>
        public static void HideSystemCursor()
        {
            if (!_isInitialized) Initialize();

            // Create a transparent cursor
            var andMask = new byte[32 * 4];
            var xorMask = new byte[32 * 4];
            for (int i = 0; i < andMask.Length; i++) 
            { 
                andMask[i] = 0xFF; 
                xorMask[i] = 0x00; 
            }

            ICONINFO iconInfo = new ICONINFO 
            { 
                fIcon = false, 
                xHotspot = 0, 
                yHotspot = 0, 
                hbmMask = CreateBitmap(32, 32, 1, 1, andMask), 
                hbmColor = CreateBitmap(32, 32, 1, 32, xorMask) 
            };

            IntPtr blankCursorHandle = CreateIconIndirect(ref iconInfo);

            // Replace system cursors
            SetSystemCursor(CopyIcon(blankCursorHandle), OCR_NORMAL);
            SetSystemCursor(CopyIcon(blankCursorHandle), OCR_IBEAM);
            SetSystemCursor(CopyIcon(blankCursorHandle), OCR_HAND);
        }

        /// <summary>
        /// Restores the system cursor to its default state.
        /// </summary>
        public static void ShowSystemCursor()
        {
            if (!_isInitialized) Initialize();
            
            // Reset system cursors to default
            SystemParametersInfo(SPI_SETCURSORS, 0, IntPtr.Zero, SPIF_SENDWININICHANGE);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO 
        { 
            public bool fIcon; 
            public int xHotspot; 
            public int yHotspot; 
            public IntPtr hbmMask; 
            public IntPtr hbmColor; 
        }
    }
}
