using HotelBookingSystem.Models;
using System;
using System.Collections.Generic;

namespace HotelBookingSystem.Services
{
    public interface IAvailabilityStrategy
    {
        bool IsRoomAvailable(int roomId, DateTime from, DateTime to, IEnumerable<Booking> existingBookings);
    }
}