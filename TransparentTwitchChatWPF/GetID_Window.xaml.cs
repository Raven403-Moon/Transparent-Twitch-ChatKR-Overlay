using System.Windows;
using System.Windows.Navigation;
using TwitchLib.Api;
using TransparentTwitchChatWPF.Utils;

namespace TransparentTwitchChatWPF
{
    /// <summary>
    /// Interaction logic for GetID_Window.xaml
    /// </summary>
    public partial class GetID_Window : Window
    {
        private TwitchAPI _api;
        private string _username = "";

        public GetID_Window(string Username)
        {
            this._username = Username;
            InitializeComponent();
            this.Title = $"'{Username}' 채널 ID 가져오기";
            this.lblChannelID.Content = $"'{Username}'의 채널 ID:";
            
            if (string.IsNullOrEmpty(Username))
            {
                tbChannelID.Text = "(사용자 이름이 비어 있습니다)";
                btnFetchID.IsEnabled = false;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ShellHelper.OpenUrl(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void btnFetchID_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btnFetchID.Content == "확인" || (string)btnFetchID.Content == "OK")
            {
                DialogResult = true;
            }
            else
            {
                tbChannelID.Text = "(여기에 ID가 표시됩니다)";
                btnFetchID.IsEnabled = false;
                btnFetchID.Content = "대기 중...";

                
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
