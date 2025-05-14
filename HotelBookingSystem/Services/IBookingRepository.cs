using HotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Services
{
    public interface IBookingRepository
    {
        List<Booking> GetAll();
        void Add(Booking booking);
        void Remove(int bookingId);
        void Save();
    }
}
