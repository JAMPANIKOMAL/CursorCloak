using System;
using System.Runtime.InteropServices;

namespace CursorCloak.UI.Services
{
    /// <summary>
    /// Manages the registration and unregistration of global system hotkeys.
    /// </summary>
    public static class HotKeyManager
    {
        #region Native Methods

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        #endregion

        #region Constants

        /// <summary>
        /// ID for the Hide Cursor hotkey.
        /// </summary>
        public const int HIDE_HOTKEY_ID = 9000;

        /// <summary>
        /// ID for the Show Cursor hotkey.
        /// </summary>
        public const int SHOW_HOTKEY_ID = 9001;

        private const uint MOD_ALT = 0x0001;
        private const uint VK_H = 0x48;
        private const uint VK_S = 0x53;

        #endregion

        /// <summary>
        /// Registers the global hotkeys for the application.
        /// </summary>
        /// <param name="handle">The window handle to associate the hotkeys with.</param>
        public static void RegisterHotKeys(IntPtr handle)
        {
            // Alt+H to Hide
            RegisterHotKey(handle, HIDE_HOTKEY_ID, MOD_ALT, VK_H);
            
            // Alt+S to Show
            RegisterHotKey(handle, SHOW_HOTKEY_ID, MOD_ALT, VK_S);
        }

        /// <summary>
        /// Unregisters the global hotkeys.
        /// </summary>
        /// <param name="handle">The window handle associated with the hotkeys.</param>
        public static void UnregisterHotKeys(IntPtr handle)
        {
            UnregisterHotKey(handle, HIDE_HOTKEY_ID);
            UnregisterHotKey(handle, SHOW_HOTKEY_ID);
        }
    }
}
