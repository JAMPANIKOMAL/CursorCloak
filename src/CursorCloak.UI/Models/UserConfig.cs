using System;
using System.IO;
using System.Text.Json;

namespace CursorCloak.UI.Models
{
    /// <summary>
    /// Represents user-specific configuration and metadata.
    /// </summary>
    public class UserConfig
    {
        public string UserName { get; set; } = Environment.UserName;
        public string DisplayName { get; set; } = "CursorCloak User";
        public string Organization { get; set; } = "Personal";
        public bool EnableBackgroundMode { get; set; } = true;
        public bool StartWithWindows { get; set; } = true;
        public bool ShowNotifications { get; set; } = true;
        public string Theme { get; set; } = "Dark";
        public DateTime FirstRun { get; set; } = DateTime.Now;
        public DateTime LastUsed { get; set; } = DateTime.Now;

        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CursorCloak",
            "user-config.json"
        );

        /// <summary>
        /// Loads the user configuration from disk.
        /// </summary>
        /// <returns>The loaded configuration or a new instance if not found.</returns>
        public static UserConfig Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    var config = JsonSerializer.Deserialize<UserConfig>(json) ?? new UserConfig();
                    config.LastUsed = DateTime.Now;
                    return config;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load user config: {ex.Message}");
            }

            return new UserConfig();
        }

        /// <summary>
        /// Saves the current configuration to disk.
        /// </summary>
        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
                LastUsed = DateTime.Now;
                
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save user config: {ex.Message}");
            }
        }
    }
}
