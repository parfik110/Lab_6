using HotelBookingSystem.Models;
using HotelBookingSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelBookingSystem.ViewModels
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<Booking> Bookings { get; set; }

        public int SelectedRoomId { get; set; }
        public DateTime CheckInDate { get; set; } = DateTime.Today;
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

        public ICommand BookRoomCommand { get; }

        private readonly BookingService _bookingService;

        public BookingViewModel()
        {
            Rooms = new ObservableCollection<Room>
        {
            new Room { Id = 1, Number = "101", Type = "Standard" },
            new Room { Id = 2, Number = "102", Type = "Deluxe" }
        };

            var repo = new JsonBookingRepository();
            _bookingService = new BookingService(repo);

            Bookings = new ObservableCollection<Booking>(_bookingService.GetAllBookings());

            BookRoomCommand = new RelayCommand(BookRoom, CanBook);
        }

        private void BookRoom()
        {
            var booking = _bookingService.CreateBooking(SelectedRoomId, 1, CheckInDate, CheckOutDate); // GuestId=1 for demo
            Bookings.Add(booking);
            OnPropertyChanged(nameof(Bookings));
        }

        private bool CanBook()
        {
            return SelectedRoomId > 0 && CheckOutDate > CheckInDate;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
