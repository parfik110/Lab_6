using HotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelBookingSystem.Services
{
    public class StrictAvailabilityStrategy : IAvailabilityStrategy
    {
        private readonly ILogger _logger;

        public StrictAvailabilityStrategy(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsRoomAvailable(int roomId, DateTime from, DateTime to, IEnumerable<Booking> existingBookings)
        {
            if (from < DateTime.Today)
            {
                _logger.LogError($"Attempt to book room {roomId} with past check-in date: {from:d}");
                return false;
            }

            if (to <= from)
            {
                _logger.LogError($"Attempt to book room {roomId} with invalid date range: {from:d} to {to:d}");
                return false;
            }

            return !existingBookings.Any(b =>
                b.RoomId == roomId &&
                b.CheckInDate < to &&
                from < b.CheckOutDate
            );
        }
    }
}