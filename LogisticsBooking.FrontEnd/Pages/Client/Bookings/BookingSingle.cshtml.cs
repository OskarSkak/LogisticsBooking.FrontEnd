﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models;
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



            var test = await bookingDataService.GetBookingsInbetweenDates(DateTime.Now.AddYears(-100),
                DateTime.Now.AddYears(+100));
            var la = "";
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            var guid = Guid.Parse(id);

            var result = await bookingDataService.DeleteBooking(guid);
            
            return new RedirectToPageResult("BookingOverViewAdmin");
        }

        public async Task<IActionResult> OnPostUpdate(DateTime ViewBookTime,
            int ViewPallets, int ViewPort, DateTime ViewActual, 
            DateTime ViewStart, DateTime ViewEnd, Guid ViewBookingId)
        {
            
            var booking = new Booking
            {
                bookingTime = ViewBookTime, 
                totalPallets = ViewPallets, 
                port = ViewPort, 
                actualArrival = ViewActual, 
                startLoading = ViewStart, 
                endLoading = ViewEnd, 
                internalId = ViewBookingId
            };

            var result = await bookingDataService.UpdateBooking(booking);
            
            if(result.IsSuccesfull) return new RedirectToPageResult("BookingOverViewAdmin");

            return new RedirectToPageResult("Error");
        }
    }
}

