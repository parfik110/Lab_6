using HotelBookingSystem.Models;
using System.Text.Json;
using System.IO;

namespace HotelBookingSystem.Repositories
{
    public class JsonGuestRepository : IGuestRepository
    {
        private readonly string _filePath;
        private List<Guest> _guests;

        public JsonGuestRepository(string filePath = "guests.json")
        {
            _filePath = filePath;
            LoadGuests();
        }

        private void LoadGuests()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _guests = JsonSerializer.Deserialize<List<Guest>>(json) ?? new List<Guest>();
            }
            else
            {
                _guests = new List<Guest>();
            }
        }

        public void AddGuest(Guest guest)
        {
            _guests.Add(guest);
        }

        public Guest? GetGuestByUsername(string username)
        {
            return _guests.FirstOrDefault(g => g.Username == username);
        }

        public IEnumerable<Guest> GetAllGuests()
        {
            return _guests;
        }

        public void SaveChanges()
        {
            var json = JsonSerializer.Serialize(_guests, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
