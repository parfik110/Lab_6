using HotelBookingSystem.Models;
using System.IO;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace HotelBookingSystem.Services
{
    public class GuestService : IGuestService
    {
        private const string FilePath = "guests.json";
        private readonly List<Guest> _guests;

        public GuestService()
        {
            _guests = LoadGuests();
        }

        public Guest? Authenticate(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            return _guests.FirstOrDefault(g => g.Username == username && g.PasswordHash == hashedPassword);
        }

        public void Register(string fullName, string email, string phone, string username, string password)
        {
            if (_guests.Any(g => g.Username == username))
                throw new Exception("User with this username already exists.");

            var newGuest = new Guest
            {
                Id = _guests.Count > 0 ? _guests.Max(g => g.Id) + 1 : 1,
                FullName = fullName,
                Email = email,
                PhoneNumber = phone,
                Username = username,
                PasswordHash = HashPassword(password),
                Role = "Guest"
            };

            _guests.Add(newGuest);
            SaveData();
        }

        public Guest? GetByUsername(string username) => 
            _guests.FirstOrDefault(g => g.Username == username);

        private List<Guest> LoadGuests()
        {
            return File.Exists(FilePath) 
                ? JsonSerializer.Deserialize<List<Guest>>(File.ReadAllText(FilePath)) ?? new List<Guest>()
                : new List<Guest>();
        }

        private void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(FilePath, JsonSerializer.Serialize(_guests, options));
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}