using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class bookings : PageModel
    {
        private readonly IBookingDataService _bookingDataService;
        
        public List<DataServices.Models.Booking> Bookings { get; set; }

        public bookings(IBookingDataService bookingDataService)
        {
            _bookingDataService = bookingDataService;
        }
        
        public async Task OnGetAsync()
        {
            Bookings = await _bookingDataService.GetBookings();
        }
    }
}