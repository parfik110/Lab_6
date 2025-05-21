using HotelBookingSystem.Models;
using System;
using System.Windows;

namespace HotelBookingSystem.Views
{
    public partial class EditBookingWindow : Window
    {
        public Booking EditedBooking { get; private set; }

        public EditBookingWindow(Booking bookingToEdit)
        {
            InitializeComponent();
            EditedBooking = new Booking
            {
                Id = bookingToEdit.Id,
                RoomId = bookingToEdit.RoomId,
                GuestId = bookingToEdit.GuestId,
                CheckInDate = bookingToEdit.CheckInDate,
                CheckOutDate = bookingToEdit.CheckOutDate
            };

            RoomIdTextBox.Text = EditedBooking.RoomId.ToString();
            CheckInDatePicker.SelectedDate = EditedBooking.CheckInDate;
            CheckOutDatePicker.SelectedDate = EditedBooking.CheckOutDate;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RoomIdTextBox.Text, out int roomId) &&
                CheckInDatePicker.SelectedDate is DateTime checkIn &&
                CheckOutDatePicker.SelectedDate is DateTime checkOut)
            {
                EditedBooking.RoomId = roomId;
                EditedBooking.CheckInDate = checkIn;
                EditedBooking.CheckOutDate = checkOut;

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter valid data.");
            }
        }
    }
}
