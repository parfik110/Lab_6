using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Models
{
    public class Booking : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int GuestId { get; set; }

        public override string ToString()
        {
            return $"Booking ID: {Id}, Room: {RoomId}, From: {CheckInDate:dd.MM.yyyy}, To: {CheckOutDate:dd.MM.yyyy}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _roomId;
        public int RoomId
        {
            get => _roomId;
            set
            {
                if (_roomId != value)
                {
                    _roomId = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _checkInDate;
        public DateTime CheckInDate
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

        private DateTime _checkOutDate;
        public DateTime CheckOutDate
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
    }
}
