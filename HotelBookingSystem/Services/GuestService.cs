using HotelBookingSystem.Models;
using HotelBookingSystem.Services;
using System.IO;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

public class GuestService : IGuestService
{
    private const string FilePath = "guests.json";
    private List<Guest> _guests;

    public GuestService()
    {
        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            _guests = JsonSerializer.Deserialize<List<Guest>>(json) ?? new List<Guest>();
        }
        else
        {
            _guests = new List<Guest>();
        }
    }

    public Guest? Authenticate(string username, string password)
    {
        var hashed = HashPassword(password);
        return _guests.FirstOrDefault(g => g.Username == username && g.PasswordHash == hashed);
    }

    public void Register(string fullName, string email, string phone, string username, string password)
    {
        if (_guests.Any(g => g.Username == username))
            throw new Exception("Користувач з таким логіном вже існує.");

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
        Save();
    }

    public Guest? GetByUsername(string username)
    {
        return _guests.FirstOrDefault(g => g.Username == username);
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_guests, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
