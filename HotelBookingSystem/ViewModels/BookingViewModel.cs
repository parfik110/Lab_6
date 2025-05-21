using HotelBookingSystem.Models;
using HotelBookingSystem.Services;
using HotelBookingSystem.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HotelBookingSystem.ViewModels
{
    public class BookingViewModel : BaseViewModel
    {
        private readonly BookingService _bookingService;
        private readonly ILogger _logger;

        public ObservableCollection<Booking> Bookings { get; set; }

        private Booking? _selectedBooking;
        public DateTime? FilterFrom { get; set; }
        public DateTime? FilterTo { get; set; }
        public int? FilterRoomId { get; set; }
        public ICommand EditBookingCommand { get; }

        public ObservableCollection<Room> Rooms { get; set; }
        public int SelectedRoomId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public ICommand BookRoomCommand { get; }


        private void EditBooking()
        {
            var window = new EditBookingWindow(SelectedBooking);
            if (window.ShowDialog() == true)
            {
                try
                {
                    _bookingService.UpdateBooking(window.EditedBooking);

                    var updated = Bookings.FirstOrDefault(b => b.Id == window.EditedBooking.Id);
                    if (updated != null)
                    {
                        updated.RoomId = window.EditedBooking.RoomId;
                        updated.CheckInDate = window.EditedBooking.CheckInDate;
                        updated.CheckOutDate = window.EditedBooking.CheckOutDate;
                    }

                    _logger.LogInfo($"Booking {window.EditedBooking.Id} updated.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Edit Error");
                }
            }
        }



        public ICommand ApplyFilterCommand => new RelayCommand(ApplyFilter);

        private void ApplyFilter()
        {
            var filtered = _bookingService.FilterBookings(FilterFrom, FilterTo, FilterRoomId);
            Bookings.Clear();
            foreach (var booking in filtered)
                Bookings.Add(booking);
        }

        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBooking));
                (CancelBookingCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (EditBookingCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }


        public ICommand CancelBookingCommand { get; }

        public BookingViewModel(BookingService bookingService, ILogger logger)
        {
            _bookingService = bookingService;
            _logger = logger;

            Bookings = new ObservableCollection<Booking>(_bookingService.GetAllBookings());
            CancelBookingCommand = new RelayCommand(CancelBooking, () => SelectedBooking != null);
            EditBookingCommand = new RelayCommand(EditBooking, () => SelectedBooking != null);
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
