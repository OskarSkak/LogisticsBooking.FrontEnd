using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class bookings : PageModel
    {
        private readonly IBookingDataService _bookingDataService;
        
        public BookingsListViewModel BookingsListViewModel { get; set; }

        public bookings(IBookingDataService bookingDataService)
        {
            _bookingDataService = bookingDataService;
        }
        
        public async Task OnGetAsync()
        {
            BookingsListViewModel = await _bookingDataService.GetBookings();
        }
    }
}