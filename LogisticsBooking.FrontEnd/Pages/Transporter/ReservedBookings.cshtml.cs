using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter
{
    public class ReservedBookingsIndexModel : PageModel
    {
        private IBookingDataService BookingDataService { get; set; }
        private ITransporterDataService TransporterDataService { get; set; }
        [BindProperty] public List<BookingViewModel> Bookings { get; set; }

        public ReservedBookingsIndexModel(IBookingDataService _bookingDataService, ITransporterDataService _transporterDataService)
        {
            BookingDataService = _bookingDataService;
            TransporterDataService = _transporterDataService;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            Bookings = new List<BookingViewModel>();
            
            var stringId = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            var guid = Guid.Parse(stringId);

            var loggedIn = await TransporterDataService.GetTransporterById(guid);
            
            var allBookings = await BookingDataService.GetBookings();
            
            foreach (var booking in allBookings.Bookings)
            {
                if (booking.TransporterName == loggedIn.Name) Bookings.Add(booking);
            }

            return Page();
        }
    }
}