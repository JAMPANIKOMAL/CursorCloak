using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CursorCloak.UI.Services
{
    /// <summary>
    /// Provides functionality to install and manage a global low-level mouse hook.
    /// Used to detect mouse movement system-wide even when the application is in the background.
    /// </summary>
    public static class GlobalMouseHook
    {
        private static IntPtr _hookId = IntPtr.Zero;
        private static LowLevelMouseProc? _proc;
        private static Action? _onMouseMove;

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        #region Native Methods

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion

        private const int WH_MOUSE_LL = 14;
        private const int WM_MOUSEMOVE = 0x0200;

        /// <summary>
        /// Installs the global mouse hook.
        /// </summary>
        /// <param name="onMouseMove">Action to execute when mouse movement is detected.</param>
        public static void Install(Action onMouseMove)
        {
            _onMouseMove = onMouseMove;
            _proc = HookCallback;
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule!)
            {
                _hookId = SetWindowsHookEx(WH_MOUSE_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        /// <summary>
        /// Uninstalls the global mouse hook and releases resources.
        /// </summary>
        public static void Uninstall()
        {
            if (_hookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookId);
                _hookId = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Callback method for the low-level mouse hook.
        /// </summary>
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_MOUSEMOVE)
            {
                _onMouseMove?.Invoke();
            }
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
    }
}
