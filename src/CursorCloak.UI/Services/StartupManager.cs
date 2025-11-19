using System;
using System.IO;
using Microsoft.Win32;

namespace CursorCloak.UI.Services
{
    /// <summary>
    /// Manages the application's startup configuration via the Windows Registry.
    /// </summary>
    public static class StartupManager
    {
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "CursorCloak";

        /// <summary>
        /// Enables or disables the application to start automatically with Windows.
        /// </summary>
        /// <param name="isEnabled">True to enable startup; false to disable.</param>
        public static void SetStartup(bool isEnabled)
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key != null)
                    {
                        if (isEnabled)
                        {
                            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
                            if (!string.IsNullOrEmpty(exePath) && File.Exists(exePath))
                            {
                                string quotedPath = $"\"{exePath}\"";
                                key.SetValue(AppName, quotedPath);
                                System.Diagnostics.Debug.WriteLine($"[StartupManager] Set registry: {quotedPath}");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"[StartupManager] Could not resolve exe path for startup.");
                            }
                        }
                        else
                        {
                            key.DeleteValue(AppName, false);
                            System.Diagnostics.Debug.WriteLine("[StartupManager] Startup registry entry removed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to modify startup registry entry: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if the application is configured to start with Windows.
        /// </summary>
        /// <returns>True if startup is enabled; otherwise, false.</returns>
        public static bool IsStartupEnabled()
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
                {
                    if (key != null)
                    {
                        object? value = key.GetValue(AppName);
                        if (value is string regPath)
                        {
                            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
                            
                            // Normalize paths for comparison (handle quotes and case)
                            string normalizedReg = regPath.Trim().Trim('"');
                            string normalizedExe = exePath.Trim().Trim('"');
                            
                            bool match = string.Equals(normalizedReg, normalizedExe, StringComparison.OrdinalIgnoreCase);
                            System.Diagnostics.Debug.WriteLine($"[StartupManager] Registry: {normalizedReg}, Current: {normalizedExe}, Match: {match}");
                            return match;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to read startup registry entry: {ex.Message}");
            }
            return false;
        }

        /// <summary>
        /// Removes the startup entry from the registry.
        /// </summary>
        public static void RemoveStartupEntry()
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key != null)
                    {
                        key.DeleteValue(AppName, false);
                    }
                }
            }
            catch (Exception)
            {
                // Silently handle potential registry access errors
                System.Diagnostics.Debug.WriteLine("Failed to remove startup registry entry");
            }
        }
    }
}
