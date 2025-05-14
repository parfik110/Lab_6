using HotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Services
{
    public class BookingService
    {
        private readonly List<Booking> _bookings = new();
        private int _nextBookingId = 1;

        public Booking CreateBooking(int roomId, int guestId, DateTime checkIn, DateTime checkOut)
        {
            if (!IsRoomAvailable(roomId, checkIn, checkOut))
                throw new InvalidOperationException("Room is not available for the selected dates.");

            var booking = new Booking
            {
                Id = _nextBookingId++,
                RoomId = roomId,
                GuestId = guestId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut
            };

            _bookings.Add(booking);
            return booking;
        }

        public bool CancelBooking(int bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null) return false;

            _bookings.Remove(booking);
            return true;
        }

        public bool IsRoomAvailable(int roomId, DateTime from, DateTime to)
        {
            return !_bookings.Any(b =>
                b.RoomId == roomId &&
                b.CheckInDate < to &&
                from < b.CheckOutDate
            );
        }

        public List<Booking> GetBookingsForRoom(int roomId)
        {
            return _bookings.Where(b => b.RoomId == roomId).ToList();
        }

        public List<Booking> GetAllBookings()
        {
            return _bookings;
        }
    }
}
