namespace CursorCloak.UI.Models
{
    /// <summary>
    /// Represents the application settings.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Gets or sets a value indicating whether cursor hiding is enabled.
        /// </summary>
        public bool IsHidingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application should start with Windows.
        /// </summary>
        public bool StartWithWindows { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor should automatically hide after inactivity.
        /// </summary>
        public bool AutoHideCursor { get; set; }

        /// <summary>
        /// Gets or sets the inactivity timeout in seconds before auto-hiding the cursor.
        /// </summary>
        public int AutoHideTimeoutSeconds { get; set; } = 5;
    }
}
