using HotelBookingSystem.Services;
using HotelBookingSystem.Models;
using System.Windows;

namespace HotelBookingSystem.Views
{
    public partial class LoginWindow : Window
    {
        private readonly IGuestService _guestService;

        public LoginWindow()
        {
            InitializeComponent();
            _guestService = new GuestService();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            var guest = _guestService.Authenticate(username, password);

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
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }
    }
}
