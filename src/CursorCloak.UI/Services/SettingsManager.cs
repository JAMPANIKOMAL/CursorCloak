using System;
using System.IO;
using System.Text.Json;
using CursorCloak.UI.Models;

namespace CursorCloak.UI.Services
{
    /// <summary>
    /// Manages the loading and saving of application settings.
    /// Persists settings to a JSON file in the user's AppData directory.
    /// </summary>
    public static class SettingsManager
    {
        private static readonly string _settingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CursorCloak", "settings.json");

        /// <summary>
        /// Saves the provided settings to disk.
        /// </summary>
        /// <param name="settings">The settings object to save.</param>
        public static void Save(Settings settings)
        {
            try
            {
                var directoryPath = Path.GetDirectoryName(_settingsFilePath);
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads settings from disk. Returns default settings if file doesn't exist or error occurs.
        /// </summary>
        /// <returns>The loaded settings or a new Settings instance.</returns>
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
            catch (Exception ex)
            {
                // Log error but don't crash
                System.Diagnostics.Debug.WriteLine($"Failed to load settings: {ex.Message}");
            }
            return new Settings();
        }
    }
}
