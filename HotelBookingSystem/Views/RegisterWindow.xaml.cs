using HotelBookingSystem.Services;
using System;
using System.Windows;

namespace HotelBookingSystem.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly IGuestService _guestService;

        public RegisterWindow()
        {
            InitializeComponent();
            _guestService = new GuestService();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _guestService.Register(
                    FullNameTextBox.Text,
                    EmailTextBox.Text,
                    PhoneTextBox.Text,
                    UsernameTextBox.Text,
                    PasswordBox.Password
                );

                MessageBox.Show("Реєстрація успішна!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
