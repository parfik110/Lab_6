using HotelBookingSystem.Models;
using HotelBookingSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repository;
        private readonly ILogger _logger;

        public BookingService(IBookingRepository repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        private bool HasConflictingBookings(int roomId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null)
        {
            return _repository.GetAll().Any(b =>
              b.RoomId == roomId &&
              (!excludeBookingId.HasValue || b.Id != excludeBookingId.Value) &&
              b.CheckInDate < checkOut &&
              checkIn < b.CheckOutDate
            );
        }

        public Booking CreateBooking(int roomId, int guestId, DateTime checkIn, DateTime checkOut)
        {
            ValidateBookingDates(checkIn, checkOut);
            
            if (!IsRoomAvailable(roomId, checkIn, checkOut))
            {
                _logger.LogError($"Failed booking attempt: Room {roomId} not available from {checkIn:d} to {checkOut:d}");
                throw new InvalidOperationException("Room is not available");
            }

            var booking = new Booking
            {
                Id = GetNextBookingId(),
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
            var existing = _repository.GetById(bookingId);
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

        public bool IsRoomAvailable(int roomId, DateTime from, DateTime to)
        {
            return !HasConflictingBookings(roomId, from, to);
        }

        public List<Booking> GetBookingsForRoom(int roomId) => 
            _repository.GetAll().Where(b => b.RoomId == roomId).ToList();

        public List<Booking> GetAllBookings() => 
            _repository.GetAll().ToList();

        public void UpdateBooking(Booking updatedBooking)
        {
            ValidateBookingDates(updatedBooking.CheckInDate, updatedBooking.CheckOutDate);
            CheckForBookingConflicts(updatedBooking);

            _repository.Update(updatedBooking);
            _repository.Save();
        }

        public IEnumerable<Booking> FilterBookings(DateTime? from = null, DateTime? to = null, int? roomId = null)
        {
            return _repository.GetAll().Where(b =>
                (!from.HasValue || b.CheckInDate >= from.Value) &&
                (!to.HasValue || b.CheckOutDate <= to.Value) &&
                (!roomId.HasValue || b.RoomId == roomId.Value));
        }

        public IEnumerable<Room> GetAvailableRooms() => 
            _repository.GetRooms();

        private int GetNextBookingId()
        {
            var all = _repository.GetAll();
            return all.Any() ? all.Max(b => b.Id) + 1 : 1;
        
            if (updatedBooking.CheckInDate < DateTime.Today || updatedBooking.CheckOutDate <= updatedBooking.CheckInDate)
                throw new ArgumentException("Invalid booking dates.");

            if (HasConflictingBookings(updatedBooking.RoomId, updatedBooking.CheckInDate, updatedBooking.CheckOutDate, updatedBooking.Id))
                throw new InvalidOperationException("Booking conflicts with an existing reservation.");
         }

        private void ValidateBookingDates(DateTime checkIn, DateTime checkOut)
        {
            if (checkIn < DateTime.Today)
                throw new ArgumentException("Check-in date cannot be in the past");
                
            if (checkOut <= checkIn)
                throw new ArgumentException("Check-out date must be after check-in date");
        }

        private void CheckForBookingConflicts(Booking booking)
        {
            var conflictingBookings = _repository.GetAll()
                .Where(b => b.Id != booking.Id &&
                           b.RoomId == booking.RoomId &&
                           b.CheckInDate < booking.CheckOutDate &&
                           booking.CheckInDate < b.CheckOutDate);

            if (conflictingBookings.Any())
                throw new InvalidOperationException("Booking conflicts with an existing reservation.");
        }
    }
}