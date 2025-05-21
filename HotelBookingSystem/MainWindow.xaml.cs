using HotelBookingSystem.DI;
using HotelBookingSystem.Factories;
using HotelBookingSystem.Models;
using HotelBookingSystem.Repositories;
using HotelBookingSystem.Services;
using HotelBookingSystem.ViewModels;
using HotelBookingSystem.Views;
using System.Windows;

namespace HotelBookingSystem
{
    public partial class MainWindow : Window
    {
        private Guest? _currentGuest = null;

        public MainWindow()
        {
            InitializeComponent();

            InitializeServicesAndViewModel();

            UpdateUIForLoggedOutState();
        }

        private void InitializeServicesAndViewModel()
        {
            var serviceProvider = new SimpleServiceProvider();

            serviceProvider.Register<ILogger>(() => new FileLogger());
            serviceProvider.Register<IBookingRepository>(() => new JsonBookingRepository());
            serviceProvider.Register<BookingService>(() =>
                new BookingService(
                    serviceProvider.Resolve<IBookingRepository>(),
                    serviceProvider.Resolve<ILogger>()
                )
            );

            var logger = serviceProvider.Resolve<ILogger>();
            var bookingService = serviceProvider.Resolve<BookingService>();
            var viewModelFactory = new ViewModelFactory(bookingService, logger);
            var bookingViewModel = viewModelFactory.CreateBookingViewModel();

            DataContext = bookingViewModel;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            var result = loginWindow.ShowDialog();

            if (result == true && App.CurrentGuest != null)
            {
                _currentGuest = App.CurrentGuest;

                if (DataContext is BookingViewModel vm)
                    vm.CurrentGuest = _currentGuest;

                UpdateUIForLoggedInState();
            }
            else
            {
                MessageBox.Show("Авторизація не вдалася або була скасована.");
            }
        }


        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        private void UpdateUIForLoggedInState()
        {
            MainContentGrid.IsEnabled = true;

            LoginButton.IsEnabled = false;
            RegisterButton.IsEnabled = false;

            WelcomeText.Text = $"Вітаємо, {_currentGuest.Username}!";
        }

        private void UpdateUIForLoggedOutState()
        {
            MainContentGrid.IsEnabled = false;

            LoginButton.IsEnabled = true;
            RegisterButton.IsEnabled = true;

            WelcomeText.Text = "Будь ласка, увійдіть або зареєструйтесь.";
        }
    }
}
