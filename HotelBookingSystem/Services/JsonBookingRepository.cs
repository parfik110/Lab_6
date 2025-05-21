using HotelBookingSystem.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HotelBookingSystem.Repositories
{
    public class JsonBookingRepository : IBookingRepository
    {
        private const string FilePath = "bookings.json";
        private List<Booking> _bookings;

        public JsonBookingRepository()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                _bookings = JsonSerializer.Deserialize<List<Booking>>(json) ?? new List<Booking>();
            }
            else
            {
                _bookings = new List<Booking>();
            }
        }

        public IEnumerable<Booking> GetAll() => _bookings;

        public void Add(Booking booking)
        {
            _bookings.Add(booking);
            Save();
        }

        public void Delete(int id)
        {
            var booking = _bookings.Find(b => b.Id == id);
            if (booking != null)
                _bookings.Remove(booking);
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(_bookings);
            File.WriteAllText(FilePath, json);
        }

        public void Update(Booking booking)
        {
            var existing = _bookings.FirstOrDefault(b => b.Id == booking.Id);
            if (existing != null)
            {
                existing.RoomId = booking.RoomId;
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
            return new List<Room>
        {
            new Room { Id = 1, Number = "101" },
            new Room { Id = 2, Number = "102" },
            new Room { Id = 3, Number = "103" }
        };
        }
    }
}
