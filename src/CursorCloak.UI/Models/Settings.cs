namespace CursorCloak.UI.Models
{
    public class Settings
    {
        public bool IsHidingEnabled { get; set; }
        public bool StartWithWindows { get; set; }
        public bool AutoHideCursor { get; set; }
        public int AutoHideTimeoutSeconds { get; set; } = 5;
    }
}
