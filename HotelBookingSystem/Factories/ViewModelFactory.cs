using HotelBookingSystem.Services;
using HotelBookingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Factories
{
    public class ViewModelFactory
    {
        private readonly BookingService _bookingService;

        public ViewModelFactory(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public BookingViewModel CreateBookingViewModel()
        {
            return new BookingViewModel(_bookingService);
        }
    }
}
