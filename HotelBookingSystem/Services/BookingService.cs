using HotelBookingSystem.Models;
using HotelBookingSystem.Repositories;
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
        private List<Booking> _allBookings;

        public BookingService(IBookingRepository repository)
        {
            _repository = repository;
            _allBookings = _repository.GetAll().ToList();
        }
        


        public Booking CreateBooking(int roomId, int guestId, DateTime checkIn, DateTime checkOut)
        {
            if (!IsRoomAvailable(roomId, checkIn, checkOut))
                throw new InvalidOperationException("Room is not available");

            var nextId = GetNextBookingId();

            var booking = new Booking
            {
                Id = nextId,
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

            _repository.Delete(bookingId);
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


        public List<Booking> GetAllBookings()
        {
            return _repository.GetAll().ToList();
        }


        private int GetNextBookingId()
        {
            var all = _repository.GetAll();
            return all.Any() ? all.Max(b => b.Id) + 1 : 1;
        }
    }

}
