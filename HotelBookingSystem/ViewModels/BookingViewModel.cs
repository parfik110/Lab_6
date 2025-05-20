using HotelBookingSystem.Models;
using HotelBookingSystem.Services;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace HotelBookingSystem.ViewModels
{
    public class BookingViewModel : BaseViewModel
    {
        private readonly BookingService _bookingService;
        private readonly ILogger _logger;

        public ObservableCollection<Booking> Bookings { get; set; }

        private Booking? _selectedBooking;
        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBooking));
                (CancelBookingCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand CancelBookingCommand { get; }

        public BookingViewModel(BookingService bookingService, ILogger logger)
        {
            _bookingService = bookingService;
            _logger = logger;

            Bookings = new ObservableCollection<Booking>(_bookingService.GetAllBookings());
            CancelBookingCommand = new RelayCommand(CancelBooking, () => SelectedBooking != null);
        }

        private void CancelBooking()
        {
            var booking = SelectedBooking;
            if (booking != null)
            {
                _bookingService.CancelBooking(booking.Id);

                var toRemove = Bookings.FirstOrDefault(b => b.Id == booking.Id);
                if (toRemove != null)
                    Bookings.Remove(toRemove);

                _logger.LogInfo($"Booking {booking.Id} cancelled.");
                SelectedBooking = null;
            }
        }

    }
}
