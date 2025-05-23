using HotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Repositories
{
    public interface IGuestRepository
    {
        void AddGuest(Guest guest);
        Guest? GetGuestByUsername(string username);
        IEnumerable<Guest> GetAllGuests();
        void SaveChanges();
    }
}
