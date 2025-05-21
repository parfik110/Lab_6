using HotelBookingSystem.Models;
using System.Collections.Generic;

namespace HotelBookingSystem.Repositories
{
    public class InMemoryBookingRepository : IBookingRepository
    {
        private readonly List<Booking> _bookings = new();

        

        public IEnumerable<Booking> GetAll() => _bookings;

        public void Add(Booking booking) => _bookings.Add(booking);

        public void Delete(int id)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
                _bookings.Remove(booking);
        }


        public void Save()
        {
        }

        public void Update(Booking booking)
        {
            var existing = _bookings.FirstOrDefault(b => b.Id == booking.Id);
            if (existing != null)
            {
                existing.RoomId = booking.RoomId;
                existing.GuestId = booking.GuestId;
                existing.CheckInDate = booking.CheckInDate;
                existing.CheckOutDate = booking.CheckOutDate;
            }
        }

        public Booking? GetById(int id)
        {
            return _bookings.FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Room> GetRooms()
        {
            throw new NotImplementedException();
        }
    }
}
