using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Booking
{
    public class BookingsListViewModel
    {
        public List<BookingViewModel> Bookings { get; set; } = new List<BookingViewModel>();
    }
}