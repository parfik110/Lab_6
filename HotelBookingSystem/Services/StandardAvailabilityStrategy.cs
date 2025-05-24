using HotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelBookingSystem.Services
{
    public class StandardAvailabilityStrategy : IAvailabilityStrategy
    {
        public bool IsRoomAvailable(int roomId, DateTime from, DateTime to, IEnumerable<Booking> existingBookings)
        {
            return !existingBookings.Any(b =>
                b.RoomId == roomId &&
                b.CheckInDate < to &&
                from < b.CheckOutDate
            );
        }
    }
}