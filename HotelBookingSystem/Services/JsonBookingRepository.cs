using HotelBookingSystem.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace HotelBookingSystem.Repositories
{
    public class JsonBookingRepository : IBookingRepository
    {
        private const string FilePath = "bookings.json";
        private readonly List<Booking> _bookings;

        public JsonBookingRepository()
        {
            _bookings = LoadBookings();
        }

        public IEnumerable<Booking> GetAll() => _bookings;

        public Booking? GetById(int id) => 
            _bookings.FirstOrDefault(b => b.Id == id);

        public void Add(Booking booking)
        {
            _bookings.Add(booking);
            SaveData();
        }

        public void Delete(int id)
        {
            var booking = GetById(id);
            if (booking != null)
            {
                _bookings.Remove(booking);
                SaveData();
            }
        }

        public void Update(Booking booking)
        {
            var existing = GetById(booking.Id);
            if (existing != null)
            {
                existing.RoomId = booking.RoomId;
                existing.CheckInDate = booking.CheckInDate;
                existing.CheckOutDate = booking.CheckOutDate;
                SaveData();
            }
        }

        public void Save() => SaveData();

        public IEnumerable<Room> GetRooms() => new List<Room>
        {
            new Room { Id = 1, Number = "101" },
            new Room { Id = 2, Number = "102" },
            new Room { Id = 3, Number = "103" }
        };

        private List<Booking> LoadBookings()
        {
            return File.Exists(FilePath)
                ? JsonSerializer.Deserialize<List<Booking>>(File.ReadAllText(FilePath)) ?? new List<Booking>()
                : new List<Booking>();
        }

        private void SaveData()
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(_bookings));
        }
    }
}