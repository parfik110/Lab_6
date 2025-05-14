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
        private readonly IBookingRepository _repository;
        private int _nextBookingId;

        public BookingService(IBookingRepository repository)
        {
            _repository = repository;
            _nextBookingId = _repository.GetAll().Any() ? _repository.GetAll().Max(b => b.Id) + 1 : 1;
        }

        public Booking CreateBooking(int roomId, int guestId, DateTime checkIn, DateTime checkOut)
        {
            if (!IsRoomAvailable(roomId, checkIn, checkOut))
                throw new InvalidOperationException("Room is not available");

            var booking = new Booking
            {
                Id = _nextBookingId++,
                RoomId = roomId,
                GuestId = guestId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut
            };

            _repository.Add(booking);
            return booking;
        }

        public bool CancelBooking(int bookingId)
        {
            var existing = _repository.GetAll().FirstOrDefault(b => b.Id == bookingId);
            if (existing == null) return false;

            _repository.Remove(bookingId);
            return true;
        }

        public bool IsRoomAvailable(int roomId, DateTime from, DateTime to)
        {
            return !_repository.GetAll().Any(b =>
                b.RoomId == roomId &&
                b.CheckInDate < to &&
                from < b.CheckOutDate
            );
        }

        public List<Booking> GetBookingsForRoom(int roomId) =>
            _repository.GetAll().Where(b => b.RoomId == roomId).ToList();

        public List<Booking> GetAllBookings() => _repository.GetAll();
    }

}
