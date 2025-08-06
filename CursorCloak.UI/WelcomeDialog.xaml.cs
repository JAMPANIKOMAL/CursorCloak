using System.Windows;

namespace CursorCloak.UI
{
    public partial class WelcomeDialog : Window
    {
        private UserConfig _config;

        public WelcomeDialog(UserConfig config)
        {
            InitializeComponent();
            _config = config;
            LoadUserData();
        }

        private void LoadUserData()
        {
            DisplayNameBox.Text = _config.DisplayName;
            OrganizationBox.Text = _config.Organization;
            UsernameBox.Text = _config.UserName;
            BackgroundModeCheck.IsChecked = _config.EnableBackgroundMode;
            StartupCheck.IsChecked = _config.StartWithWindows;
            NotificationsCheck.IsChecked = _config.ShowNotifications;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _config.DisplayName = DisplayNameBox.Text.Trim();
            _config.Organization = OrganizationBox.Text.Trim();
            _config.EnableBackgroundMode = BackgroundModeCheck.IsChecked == true;
            _config.StartWithWindows = StartupCheck.IsChecked == true;
            _config.ShowNotifications = NotificationsCheck.IsChecked == true;
            
            if (string.IsNullOrWhiteSpace(_config.DisplayName))
            {
                _config.DisplayName = _config.UserName;
            }
            
            if (string.IsNullOrWhiteSpace(_config.Organization))
            {
                _config.Organization = "Personal";
            }

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
