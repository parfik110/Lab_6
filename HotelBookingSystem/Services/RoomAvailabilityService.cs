using HotelBookingSystem.Models;
using HotelBookingSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelBookingSystem.Services
{
    public class RoomAvailabilityService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public RoomAvailabilityService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public IEnumerable<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            var allRooms = _roomRepository.GetAll();
            var bookedRoomIds = _bookingRepository.GetAll()
              .Where(b =>
                b.CheckInDate < checkOut &&
                checkIn < b.CheckOutDate)
              .Select(b => b.RoomId)
              .Distinct();

            return allRooms.Where(r => !bookedRoomIds.Contains(r.Id));
        }
    }
}
