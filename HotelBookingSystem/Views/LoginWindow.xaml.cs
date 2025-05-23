using HotelBookingSystem.Services;
using HotelBookingSystem.Models;
using System.Windows;

namespace HotelBookingSystem.Views
{
    public partial class LoginWindow : Window
    {
        private readonly IAuthService _authService;

        public LoginWindow(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            var guest = _authService.Authenticate(username, password);

            if (guest != null)
            {
                App.SetCurrentGuest(guest);

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Невірний логін або пароль.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(_authService);
            registerWindow.ShowDialog();
        }
    }
}
