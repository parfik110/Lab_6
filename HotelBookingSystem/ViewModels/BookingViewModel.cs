using HotelBookingSystem.Models;
using HotelBookingSystem.Services;
using HotelBookingSystem.Views;
using MvvmHelpers;
using System;
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
        public ICommand CancelBookingCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand BookRoomCommand { get; }

        public ObservableCollection<Room> Rooms { get; set; }
        private int _selectedRoomId;
        public int SelectedRoomId
        {
            get => _selectedRoomId;
            set
            {
                if (_selectedRoomId != value)
                {
                    _selectedRoomId = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime? _checkInDate;
        public DateTime? CheckInDate
        {
            get => _checkInDate;
            set
            {
                if (_checkInDate != value)
                {
                    _checkInDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime? _checkOutDate;
        public DateTime? CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (_checkOutDate != value)
                {
                    _checkOutDate = value;
                    OnPropertyChanged();
                }
            }
        }


        private Guest? _currentGuest;
        public Guest? CurrentGuest
        {
            get => _currentGuest;
            set
            {
                _currentGuest = value;
                OnPropertyChanged(nameof(CurrentGuest));
                (BookRoomCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
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

        public BookingViewModel(BookingService bookingService, ILogger logger)
        {
            _bookingService = bookingService;
            _logger = logger;

            Bookings = new ObservableCollection<Booking>(_bookingService.GetAllBookings());
            Rooms = new ObservableCollection<Room>(_bookingService.GetAvailableRooms());

            CancelBookingCommand = new RelayCommand(CancelBooking, () => SelectedBooking != null);
            EditBookingCommand = new RelayCommand(EditBooking, () => SelectedBooking != null);
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
            BookRoomCommand = new RelayCommand(BookRoom, CanBookRoom);

            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedRoomId) ||
                    e.PropertyName == nameof(CheckInDate) ||
                    e.PropertyName == nameof(CheckOutDate))
                {
                    (BookRoomCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            };
        }

        private void EditBooking()
        {
            if (SelectedBooking == null) return;

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

        private void ApplyFilter()
        {
            var filtered = _bookingService.FilterBookings(FilterFrom, FilterTo, FilterRoomId);
            Bookings.Clear();
            foreach (var booking in filtered)
                Bookings.Add(booking);
        }

        private void BookRoom()
        {
            try
            {
                if (!CheckInDate.HasValue || !CheckOutDate.HasValue)
                {
                    MessageBox.Show("Будь ласка, виберіть дати заїзду і виїзду.", "Помилка");
                    return;
                }

                if (SelectedRoomId == 0)
                {
                    MessageBox.Show("Будь ласка, виберіть кімнату.", "Помилка");
                    return;
                }

                if (CurrentGuest == null)
                {
                    MessageBox.Show("Гість не авторизований.", "Помилка");
                    return;
                }

                var guestId = CurrentGuest.Id;


                var booking = _bookingService.CreateBooking(SelectedRoomId, guestId, CheckInDate.Value, CheckOutDate.Value);
                Bookings.Add(booking);

                _logger.LogInfo($"Room {SelectedRoomId} booked from {CheckInDate.Value:d} to {CheckOutDate.Value:d}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка бронювання");
            }
        }

        private bool CanBookRoom()
        {
            var result = SelectedRoomId > 0 && CheckInDate.HasValue && CheckOutDate.HasValue && CheckInDate < CheckOutDate && CurrentGuest != null;
            return result;
        }


    }
}
