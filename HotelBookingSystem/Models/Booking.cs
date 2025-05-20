using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public override string ToString()
        {
            return $"Booking ID: {Id}, Room: {RoomId}, From: {CheckInDate:dd.MM.yyyy}, To: {CheckOutDate:dd.MM.yyyy}";
        }

    }
}
