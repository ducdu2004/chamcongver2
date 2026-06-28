using System.Windows;
using chamcong.WpfAdmin.Services;

namespace chamcong.WpfAdmin.Views
{
    public partial class LoginWindow : Window
    {
        private readonly ApiService _apiService;

        public LoginWindow()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = "Đang đăng nhập...";
            btnLogin.IsEnabled = false;

            var username = txtUsername.Text;
            var password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Vui lòng nhập đầy đủ thông tin.";
                btnLogin.IsEnabled = true;
                return;
            }

            bool isSuccess = await _apiService.LoginAsync(username, password);

            if (isSuccess)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                txtError.Text = "Đăng nhập thất bại. Sai tài khoản hoặc mật khẩu.";
                btnLogin.IsEnabled = true;
            }
        }
    }
}
