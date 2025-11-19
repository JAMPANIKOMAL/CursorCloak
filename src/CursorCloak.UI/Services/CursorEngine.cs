using System;
using System.Runtime.InteropServices;

namespace CursorCloak.UI.Services
{
    /// <summary>
    /// Provides low-level cursor manipulation using Windows API.
    /// Handles hiding and showing the system cursor by replacing it with a transparent icon.
    /// Implements caching to prevent GDI object leaks.
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

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        #endregion

        #region Constants & Fields

        private const uint OCR_NORMAL = 32512;
        private const uint OCR_IBEAM = 32513;
        private const uint OCR_HAND = 32649;
        private const uint SPI_SETCURSORS = 0x0057;
        private const uint SPIF_SENDWININICHANGE = 0x02;

        private static bool _isInitialized = false;
        private static IntPtr _cachedBlankCursor = IntPtr.Zero;

        #endregion

        /// <summary>
        /// Initializes the cursor engine.
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;
        }

        /// <summary>
        /// Hides the system cursor by replacing standard cursors with a cached transparent icon.
        /// </summary>
        public static void HideSystemCursor()
        {
            if (!_isInitialized) Initialize();

            // Use cached cursor if available, otherwise create it once
            if (_cachedBlankCursor == IntPtr.Zero)
            {
                _cachedBlankCursor = CreateTransparentCursor();
            }

            if (_cachedBlankCursor != IntPtr.Zero)
            {
                // SetSystemCursor destroys the cursor passed to it, so we must pass a COPY
                SetSystemCursor(CopyIcon(_cachedBlankCursor), OCR_NORMAL);
                SetSystemCursor(CopyIcon(_cachedBlankCursor), OCR_IBEAM);
                SetSystemCursor(CopyIcon(_cachedBlankCursor), OCR_HAND);
            }
        }

        /// <summary>
        /// Restores the system cursor to its default state.
        /// </summary>
        public static void ShowSystemCursor()
        {
            // Reset system cursors to default
            SystemParametersInfo(SPI_SETCURSORS, 0, IntPtr.Zero, SPIF_SENDWININICHANGE);
        }

        /// <summary>
        /// Creates a 32x32 transparent cursor.
        /// Caller is responsible for cleaning up GDI objects used during creation.
        /// </summary>
        private static IntPtr CreateTransparentCursor()
        {
            IntPtr hBitmapMask = IntPtr.Zero;
            IntPtr hBitmapColor = IntPtr.Zero;
            IntPtr hIcon = IntPtr.Zero;

            try
            {
                var andMask = new byte[32 * 4];
                var xorMask = new byte[32 * 4];
                for (int i = 0; i < andMask.Length; i++)
                {
                    andMask[i] = 0xFF;
                    xorMask[i] = 0x00;
                }

                hBitmapMask = CreateBitmap(32, 32, 1, 1, andMask);
                hBitmapColor = CreateBitmap(32, 32, 1, 32, xorMask);

                ICONINFO iconInfo = new ICONINFO
                {
                    fIcon = false,
                    xHotspot = 0,
                    yHotspot = 0,
                    hbmMask = hBitmapMask,
                    hbmColor = hBitmapColor
                };

                hIcon = CreateIconIndirect(ref iconInfo);
                return hIcon;
            }
            finally
            {
                // Clean up the bitmaps immediately after creating the icon
                if (hBitmapMask != IntPtr.Zero) DeleteObject(hBitmapMask);
                if (hBitmapColor != IntPtr.Zero) DeleteObject(hBitmapColor);
            }
        }

        /// <summary>
        /// Cleans up the cached blank cursor when the application exits.
        /// </summary>
        public static void Cleanup()
        {
            if (_cachedBlankCursor != IntPtr.Zero)
            {
                DestroyIcon(_cachedBlankCursor);
                _cachedBlankCursor = IntPtr.Zero;
            }
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
