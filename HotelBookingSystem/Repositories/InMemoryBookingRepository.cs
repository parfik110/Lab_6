using HotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Repositories
{
    public class InMemoryBookingRepository : IBookingRepository
    {
        private readonly List<Booking> _bookings = new();
        private int _nextId = 1;

        public IEnumerable<Booking> GetAll() => _bookings;

        public void Add(Booking booking)
        {
            booking.Id = _nextId++;
            _bookings.Add(booking);
        }

        public void Delete(int id)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == id);
            if (booking != null)
                _bookings.Remove(booking);
        }
    }
}
