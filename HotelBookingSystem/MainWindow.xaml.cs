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

        private SimpleServiceProvider _serviceProvider;

        private void InitializeServicesAndViewModel()
        {
            _serviceProvider = new SimpleServiceProvider();

            // Реєстрація логера та репозиторіїв
            _serviceProvider.Register<ILogger>(() => new FileLogger());
            _serviceProvider.Register<IBookingRepository>(() => new JsonBookingRepository());

            // DI для AuthService
            _serviceProvider.Register<IPasswordHasher>(() => new BCryptPasswordHasher());
            _serviceProvider.Register<IGuestRepository>(() => new JsonGuestRepository());
            _serviceProvider.Register<IAuthService>(() =>
                new AuthService(
                    _serviceProvider.Resolve<IGuestRepository>(),
                    _serviceProvider.Resolve<IPasswordHasher>()
                )
            );

            _serviceProvider.Register<BookingService>(() =>
                new BookingService(
                    _serviceProvider.Resolve<IBookingRepository>(),
                    _serviceProvider.Resolve<ILogger>()
                )
            );

            var logger = _serviceProvider.Resolve<ILogger>();
            var bookingService = _serviceProvider.Resolve<BookingService>();
            var viewModelFactory = new ViewModelFactory(bookingService, logger);
            var bookingViewModel = viewModelFactory.CreateBookingViewModel();

            DataContext = bookingViewModel;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var authService = _serviceProvider.Resolve<IAuthService>();
            var loginWindow = new LoginWindow(authService);
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
            var authService = _serviceProvider.Resolve<IAuthService>();
            var registerWindow = new RegisterWindow(authService);
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
