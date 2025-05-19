using HotelBookingSystem.Models;
using HotelBookingSystem.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace HotelBookingSystem.ViewModels
{
    public class BookingViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<string> BookingDescriptions { get; set; }

        private int _selectedRoomId;
        public int SelectedRoomId
        {
            get => _selectedRoomId;
            set
            {
                if (_selectedRoomId != value)
                {
                    _selectedRoomId = value;
                    OnPropertyChanged(nameof(SelectedRoomId));
                    ((RelayCommand)BookRoomCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private DateTime _checkInDate = DateTime.Today;
        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                if (_checkInDate != value)
                {
                    _checkInDate = value;
                    OnPropertyChanged(nameof(CheckInDate));
                    ((RelayCommand)BookRoomCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private DateTime _checkOutDate = DateTime.Today.AddDays(1);
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (_checkOutDate != value)
                {
                    _checkOutDate = value;
                    OnPropertyChanged(nameof(CheckOutDate));
                    ((RelayCommand)BookRoomCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public ICommand BookRoomCommand { get; }

        private readonly BookingService _bookingService;
        private readonly ILogger _logger;

        public BookingViewModel(BookingService bookingService, ILogger logger)
        {
            _bookingService = bookingService;
            _logger = logger;

            Rooms = new ObservableCollection<Room>
            {
                new Room { Id = 1, Number = "101", Type = "Standard" },
                new Room { Id = 2, Number = "102", Type = "Deluxe" }
            };

            BookingDescriptions = new ObservableCollection<string>(
                _bookingService.GetAllBookings()
                    .Select(b => $"Booking ID: {b.Id}, Room: {Rooms.FirstOrDefault(r => r.Id == b.RoomId)?.Number}, " +
                                 $"From: {b.CheckInDate:d}, To: {b.CheckOutDate:d}")
            );

            BookRoomCommand = new RelayCommand(BookRoom, CanBook);
        }

        private void BookRoom()
        {
            try
            {
                var booking = _bookingService.CreateBooking(SelectedRoomId, 1, CheckInDate, CheckOutDate);
                BookingDescriptions.Add($"Booking ID: {booking.Id}, Room: {Rooms.FirstOrDefault(r => r.Id == booking.RoomId)?.Number}, " +
                                        $"From: {booking.CheckInDate:d}, To: {booking.CheckOutDate:d}");
                OnPropertyChanged(nameof(BookingDescriptions));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error booking room: {ex.Message}");
            }
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
