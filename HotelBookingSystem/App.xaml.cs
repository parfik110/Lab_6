using HotelBookingSystem.DI;
using HotelBookingSystem.Factories;
using HotelBookingSystem.Models;
using HotelBookingSystem.Repositories;
using HotelBookingSystem.Services;
using HotelBookingSystem.ViewModels;
using HotelBookingSystem.Views;
using System.Windows;

namespace HotelBookingSystem
{
    public partial class App : Application
    {
        public static Guest? CurrentGuest { get; private set; }

        public static void SetCurrentGuest(Guest guest)
        {
            CurrentGuest = guest;
        }

    }
}
