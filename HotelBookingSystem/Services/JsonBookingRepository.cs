using HotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HotelBookingSystem.Services
{
    public class JsonBookingRepository : IBookingRepository
    {
        private readonly string _filePath = "bookings.json";
        private List<Booking> _bookings;

        public JsonBookingRepository()
        {
            _bookings = LoadFromFile();
        }

        public List<Booking> GetAll() => _bookings;

        public void Add(Booking booking)
        {
            _bookings.Add(booking);
            Save();
        }

        public void Remove(int bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking != null)
            {
                _bookings.Remove(booking);
                Save();
            }
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(_bookings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        private List<Booking> LoadFromFile()
        {
            if (!File.Exists(_filePath)) return new List<Booking>();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Booking>>(json) ?? new List<Booking>();
        }
    }
}
