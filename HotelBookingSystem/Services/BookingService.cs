using HotelBookingSystem.Models;
using HotelBookingSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelBookingSystem.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _repository;
        private readonly ILogger _logger;
        private readonly IAvailabilityStrategy _availabilityStrategy;

        public BookingService(
            IBookingRepository repository,
            ILogger logger,
            IAvailabilityStrategy availabilityStrategy)
        {
            _repository = repository;
            _logger = logger;
            _availabilityStrategy = availabilityStrategy;
        }

        public Booking CreateBooking(int roomId, int guestId, DateTime checkIn, DateTime checkOut)
        {
            var existingBookings = _repository.GetAll();
            if (!_availabilityStrategy.IsRoomAvailable(roomId, checkIn, checkOut, existingBookings))
            {
                _logger.LogError($"Failed booking attempt: Room {roomId} not available from {checkIn:d} to {checkOut:d}");
                throw new InvalidOperationException("Room is not available");
            }

            var nextId = GetNextBookingId();

            var booking = new Booking
            {
                Id = nextId,
                RoomId = roomId,
                GuestId = guestId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut
            };

            _repository.Add(booking);
            _repository.Save();

            _logger.LogInfo($"Created booking: ID {booking.Id} for Room {booking.RoomId} from {booking.CheckInDate:d} to {booking.CheckOutDate:d}");

            return booking;
        }

        public bool CancelBooking(int bookingId)
        {
            var existing = _repository.GetAll().FirstOrDefault(b => b.Id == bookingId);
            if (existing == null)
            {
                _logger.LogError($"Attempt to cancel non-existing booking with ID {bookingId}");
                return false;
            }

            _repository.Delete(bookingId);
            _repository.Save();
            _logger.LogInfo($"Cancelled booking ID {bookingId}");
            return true;
        }

        public List<Booking> GetBookingsForRoom(int roomId) =>
            _repository.GetAll().Where(b => b.RoomId == roomId).ToList();

        public List<Booking> GetAllBookings() =>
            _repository.GetAll().ToList();

        public void EditBooking(int id, int roomId, DateTime checkIn, DateTime checkOut)
        {
            var booking = _repository.GetById(id);
            if (booking != null)
            {
                booking.RoomId = roomId;
                booking.CheckInDate = checkIn;
                booking.CheckOutDate = checkOut;
                _repository.Update(booking);
                _repository.Save();
                _logger.LogInfo($"Booking {id} updated.");
            }
        }

        private int GetNextBookingId()
        {
            var all = _repository.GetAll();
            return all.Any() ? all.Max(b => b.Id) + 1 : 1;
        }

        public IEnumerable<Booking> FilterBookings(DateTime? from = null, DateTime? to = null, int? roomId = null)
        {
            return _repository.GetAll().Where(b =>
                (!from.HasValue || b.CheckInDate >= from.Value) &&
                (!to.HasValue || b.CheckOutDate <= to.Value) &&
                (!roomId.HasValue || b.RoomId == roomId.Value));
        }

        public void UpdateBooking(Booking updatedBooking)
        {
            var allBookings = _repository.GetAll();
            bool conflict = !_availabilityStrategy.IsRoomAvailable(
                updatedBooking.RoomId,
                updatedBooking.CheckInDate,
                updatedBooking.CheckOutDate,
                allBookings.Where(b => b.Id != updatedBooking.Id));

            if (conflict)
                throw new InvalidOperationException("Booking conflicts with an existing reservation.");

            _repository.Update(updatedBooking);
            _repository.Save();
        }

        public IEnumerable<Room> GetAvailableRooms()
        {
            return _repository.GetRooms();
        }
    }
}