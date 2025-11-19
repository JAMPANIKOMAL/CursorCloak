using System;
using System.Runtime.InteropServices;

namespace CursorCloak.UI.Services
{
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
