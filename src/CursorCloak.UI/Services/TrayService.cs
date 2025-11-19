using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CursorCloak.UI.Services
{
    /// <summary>
    /// Manages the system tray icon and its context menu.
    /// </summary>
    public class TrayService : IDisposable
    {
        private NotifyIcon? _notifyIcon;
        private readonly Action _onShowRequest;
        private readonly Action _onExitRequest;
        private readonly Action _onHideCursorRequest;
        private readonly Action _onShowCursorRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrayService"/> class.
        /// </summary>
        /// <param name="onShowRequest">Action to execute when "Show" is clicked.</param>
        /// <param name="onExitRequest">Action to execute when "Exit" is clicked.</param>
        /// <param name="onHideCursorRequest">Action to execute when "Hide Cursor" is clicked.</param>
        /// <param name="onShowCursorRequest">Action to execute when "Show Cursor" is clicked.</param>
        public TrayService(Action onShowRequest, Action onExitRequest, Action onHideCursorRequest, Action onShowCursorRequest)
        {
            _onShowRequest = onShowRequest;
            _onExitRequest = onExitRequest;
            _onHideCursorRequest = onHideCursorRequest;
            _onShowCursorRequest = onShowCursorRequest;
            
            InitializeTrayIcon();
        }

        private void InitializeTrayIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "CursorCloak - Click to show";
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += (s, e) => _onShowRequest();

            // Load Icon
            try
            {
                var resourceStream = System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/app-icon.ico"));
                if (resourceStream != null)
                {
                    _notifyIcon.Icon = new Icon(resourceStream.Stream);
                }
                else
                {
                    _notifyIcon.Icon = SystemIcons.Application;
                }
            }
            catch
            {
                _notifyIcon.Icon = SystemIcons.Application;
            }

            // Context Menu
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Show CursorCloak", null, (s, e) => _onShowRequest());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Hide Cursor (Alt+H)", null, (s, e) => _onHideCursorRequest());
            contextMenu.Items.Add("Show Cursor (Alt+S)", null, (s, e) => _onShowCursorRequest());
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Exit", null, (s, e) => _onExitRequest());

            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// Shows a balloon tip notification.
        /// </summary>
        /// <param name="title">Title of the notification.</param>
        /// <param name="message">Message body.</param>
        public void ShowNotification(string title, string message)
        {
            _notifyIcon?.ShowBalloonTip(2000, title, message, ToolTipIcon.Info);
        }

        public void Dispose()
        {
            _notifyIcon?.Dispose();
            _notifyIcon = null;
        }
    }
}
