using HotelBookingSystem.ViewModels;
using HotelBookingSystem.Repositories;
using HotelBookingSystem.Services;
using System.Windows;
using HotelBookingSystem.DI;
using HotelBookingSystem.Factories;

namespace HotelBookingSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Налаштування DI
            var serviceProvider = new SimpleServiceProvider();

            serviceProvider.Register<IBookingRepository>(() => new JsonBookingRepository());
            serviceProvider.Register<BookingService>(() => new BookingService(serviceProvider.Resolve<IBookingRepository>()));

            var bookingService = serviceProvider.Resolve<BookingService>();
            var viewModelFactory = new ViewModelFactory(bookingService);
            var bookingViewModel = viewModelFactory.CreateBookingViewModel();

            // Прив’язуємо ViewModel до вікна
            DataContext = bookingViewModel;
        }
    }
}
