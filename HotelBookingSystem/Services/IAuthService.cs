using HotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Services
{
    public interface IAuthService
    {
        Guest? Authenticate(string username, string password);
        void Register(string fullName, string email, string phone, string username, string password);
    }
}
