using System;
using System.IO;
using System.Text.Json;
using CursorCloak.UI.Models;

namespace CursorCloak.UI.Services
{
    public static class SettingsManager
    {
        private static readonly string _settingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CursorCloak", "settings.json");

        public static void Save(Settings settings)
        {
            try
            {
                var directoryPath = Path.GetDirectoryName(_settingsFilePath);
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string json = JsonSerializer.Serialize(settings);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception)
            {
                // Silently handle potential saving errors
                System.Diagnostics.Debug.WriteLine("Failed to save settings");
            }
        }

        public static Settings Load()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string json = File.ReadAllText(_settingsFilePath);
                    return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
                }
            }
            catch (Exception)
            {
                // Silently handle potential loading errors
                System.Diagnostics.Debug.WriteLine("Failed to load settings");
            }
            return new Settings(); // Return default settings if file doesn't exist or is corrupt
        }
    }
}
