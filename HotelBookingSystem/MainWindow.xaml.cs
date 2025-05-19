using HotelBookingSystem.ViewModels;
using HotelBookingSystem.Repositories;
using HotelBookingSystem.Services;
using HotelBookingSystem.DI;
using HotelBookingSystem.Factories;
using System.Windows;

namespace HotelBookingSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Налаштування DI
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

            // Прив’язуємо ViewModel до вікна
            DataContext = bookingViewModel;
        }
    }
}
