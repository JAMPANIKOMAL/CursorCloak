using System;
using System.IO;
using Microsoft.Win32;

namespace CursorCloak.UI.Services
{
    public static class StartupManager
    {
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "CursorCloak";

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
                            string quotedExePath = $"\"{exePath}\"";
                            bool match = string.Equals(regPath.Trim(), quotedExePath, StringComparison.OrdinalIgnoreCase);
                            System.Diagnostics.Debug.WriteLine($"[StartupManager] Registry value: {regPath}, Current exe: {quotedExePath}, Match: {match}");
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
