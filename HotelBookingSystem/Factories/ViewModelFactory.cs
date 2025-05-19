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
        private readonly ILogger _logger;

        public ViewModelFactory(BookingService bookingService, ILogger logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        public BookingViewModel CreateBookingViewModel()
        {
            return new BookingViewModel(_bookingService, _logger);
        }
    }
}
