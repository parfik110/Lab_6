using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.Repositories
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetAll();
        Room? GetById(int id);
    }

}
