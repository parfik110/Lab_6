using HotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Repositories
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAll();
        void Add(Booking booking);
        void Delete(int id);
        void Save();
    }
}
