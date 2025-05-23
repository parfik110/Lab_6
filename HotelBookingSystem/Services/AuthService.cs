using HotelBookingSystem.Models;
using HotelBookingSystem.Repositories;

namespace HotelBookingSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IGuestRepository guestRepository, IPasswordHasher passwordHasher)
        {
            _guestRepository = guestRepository;
            _passwordHasher = passwordHasher;
        }

        public Guest? Authenticate(string username, string password)
        {
            var guest = _guestRepository.GetGuestByUsername(username);
            if (guest == null) return null;

            return _passwordHasher.Verify(password, guest.PasswordHash) ? guest : null;
        }

        public void Register(string fullName, string email, string phone, string username, string password)
        {
            if (_guestRepository.GetGuestByUsername(username) != null)
                throw new Exception("Користувач з таким логіном вже існує.");

            var newGuest = new Guest
            {
                Id = GetNextId(),
                FullName = fullName,
                Email = email,
                PhoneNumber = phone,
                Username = username,
                PasswordHash = _passwordHasher.Hash(password),
                Role = "Guest"
            };

            _guestRepository.AddGuest(newGuest);
            _guestRepository.SaveChanges();
        }

        private int GetNextId() =>
            _guestRepository.GetAllGuests().Any() ? _guestRepository.GetAllGuests().Max(g => g.Id) + 1 : 1;
    }
}
