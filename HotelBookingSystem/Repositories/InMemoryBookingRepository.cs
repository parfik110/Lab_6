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
            var booking = _bookings.Find(b => b.Id == id);
            if (booking != null)
                _bookings.Remove(booking);
        }

        public void Save()
        {
        }
    }
}
