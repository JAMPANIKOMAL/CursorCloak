// Using statements to import necessary .NET libraries
using System;
using System.Runtime.InteropServices; // Required for calling Windows API functions (P/Invoke)
using System.Threading; // Required for creating a message loop

/// <summary>
/// This is the main class for our CursorCloak application's core logic.
/// It's a console application that demonstrates the two key features:
/// 1. Hiding and showing the system mouse cursor.
/// 2. Listening for a global hotkey to trigger the action.
/// </summary>
public class CursorCloakEngine
{
    // --- Windows API Imports ---
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("user32.dll")]
    private static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetSystemCursor(IntPtr hcur, uint id);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

    // --- Cursor and Hotkey Constants ---
    private const int HIDE_HOTKEY_ID = 9000;
    private const int SHOW_HOTKEY_ID = 9001;
    private const uint MOD_ALT = 0x0001;
    private const uint VK_H = 0x48; // 'H' key
    private const uint VK_S = 0x53; // 'S' key
    
    private const uint OCR_NORMAL = 32512;
    private const uint OCR_IBEAM = 32513;
    private const uint OCR_HAND = 32649;

    private const uint SPI_SETCURSORS = 0x0057;
    private const uint SPIF_SENDWININICHANGE = 0x02;

    // --- State Management ---
    private static IntPtr originalNormalCursor;
    private static IntPtr originalIBeamCursor;
    private static IntPtr originalHandCursor;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public static void Main()
    {
        Console.WriteLine("CursorCloak Engine Started (v4 - Separate Hotkeys)...");
        Console.WriteLine("Press Alt + H to HIDE cursor.");
        Console.WriteLine("Press Alt + S to SHOW cursor.");
        Console.WriteLine("Press Ctrl + C in this window to exit.");
        
        originalNormalCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_NORMAL));
        originalIBeamCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_IBEAM));
        originalHandCursor = CopyIcon(LoadCursor(IntPtr.Zero, (int)OCR_HAND));

        // Register HIDE hotkey
        if (!RegisterHotKey(IntPtr.Zero, HIDE_HOTKEY_ID, MOD_ALT, VK_H))
        {
            Console.WriteLine("Error: Could not register Alt + H hotkey.");
            return;
        }
        // Register SHOW hotkey
        if (!RegisterHotKey(IntPtr.Zero, SHOW_HOTKEY_ID, MOD_ALT, VK_S))
        {
            Console.WriteLine("Error: Could not register Alt + S hotkey.");
            UnregisterHotKey(IntPtr.Zero, HIDE_HOTKEY_ID); // Clean up the first one
            return;
        }

        MSG msg;
        while (GetMessage(out msg, IntPtr.Zero, 0, 0))
        {
            if (msg.message == 0x0312) // WM_HOTKEY message
            {
                int id = msg.wParam.ToInt32();
                if (id == HIDE_HOTKEY_ID)
                {
                    HideSystemCursor();
                }
                else if (id == SHOW_HOTKEY_ID)
                {
                    ShowSystemCursor();
                }
            }
        }

        // IMPORTANT: Always restore the cursor before exiting
        ShowSystemCursor();
        UnregisterHotKey(IntPtr.Zero, HIDE_HOTKEY_ID);
        UnregisterHotKey(IntPtr.Zero, SHOW_HOTKEY_ID);
    }

    /// <summary>
    /// Forces the system cursor to be hidden by replacing it with a truly blank cursor.
    /// </summary>
    private static void HideSystemCursor()
    {
        // *** FIX for Black Box: Create a proper transparent cursor ***
        // An AND mask of all 1s (0xFF) preserves the screen color.
        // An XOR mask of all 0s (0x00) writes no color. Result = transparent.
        var andMask = new byte[32 * 4];
        var xorMask = new byte[32 * 4];
        for (int i = 0; i < andMask.Length; i++)
        {
            andMask[i] = 0xFF;
            xorMask[i] = 0x00;
        }

        ICONINFO iconInfo = new ICONINFO
        {
            fIcon = false, // This defines it as a cursor, not an icon
            xHotspot = 0,
            yHotspot = 0,
            hbmMask = CreateBitmap(32, 32, 1, 1, andMask),
            hbmColor = CreateBitmap(32, 32, 1, 32, xorMask)
        };
        IntPtr blankCursorHandle = CreateIconIndirect(ref iconInfo);

        SetSystemCursor(CopyIcon(blankCursorHandle), OCR_NORMAL);
        SetSystemCursor(CopyIcon(blankCursorHandle), OCR_IBEAM);
        SetSystemCursor(CopyIcon(blankCursorHandle), OCR_HAND);

        Console.WriteLine("Cursor is now Hidden");
    }

    /// <summary>
    /// Restores the original system cursors using the handles saved at startup.
    /// </summary>
    private static void ShowSystemCursor()
    {
        SetSystemCursor(CopyIcon(originalNormalCursor), OCR_NORMAL);
        SetSystemCursor(CopyIcon(originalIBeamCursor), OCR_IBEAM);
        SetSystemCursor(CopyIcon(originalHandCursor), OCR_HAND);
        
        SystemParametersInfo(SPI_SETCURSORS, 0, IntPtr.Zero, SPIF_SENDWININICHANGE);
        
        Console.WriteLine("Cursor is now Visible");
    }

    // --- More advanced API imports needed for creating a blank cursor ---

    [DllImport("user32.dll")]
    public static extern IntPtr CreateIconIndirect(ref ICONINFO piconinfo);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, byte[] lpvBits);

    [DllImport("user32.dll")]
    public static extern IntPtr CopyIcon(IntPtr hIcon);
    
    [StructLayout(LayoutKind.Sequential)]
    public struct ICONINFO
    {
        public bool fIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    /// <summary>
    /// A structure that represents a Windows message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hWnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point pt;
    }
}
