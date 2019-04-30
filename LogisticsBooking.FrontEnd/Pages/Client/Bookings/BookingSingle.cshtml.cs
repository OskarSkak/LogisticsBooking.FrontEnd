using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.RequestModels;
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using LogisticsBooking.FrontEnd.Documents;
using LogisticsBooking.FrontEnd.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class BookingSingleModel : PageModel
    {
        private IBookingDataService bookingDataService;
        [BindProperty] public Booking Booking { get; set; }

        public BookingSingleModel(IBookingDataService _bookingDataService)
        {
            bookingDataService = _bookingDataService;
        }

        public async Task OnGetAsync(string id)
        {
            Booking = new Booking();
            Booking = await bookingDataService.GetBookingById(Guid.Parse(id));
            Booking = BookingUtil.RemoveDates(Booking);
            var a = "";
        }

        public async Task<IActionResult> OnPostUpdate(DateTime ViewBookTime,
            int ViewPallets, int ViewPort, DateTime ViewActual, 
            DateTime ViewStart, DateTime ViewEnd, Guid ViewBookingId)
        {
            
           /* var booking = new Booking(
            {
                bookingTime = ViewBookTime, 
                totalPallets = ViewPallets, 
                port = ViewPort, 
                actualArrival = ViewActual, 
                startLoading = ViewStart, 
                endLoading = ViewEnd, 
                internalId = ViewBookingId
            });*/
            
            return new RedirectToPageResult("BookingOverViewAdmin");
        }
    }
}

