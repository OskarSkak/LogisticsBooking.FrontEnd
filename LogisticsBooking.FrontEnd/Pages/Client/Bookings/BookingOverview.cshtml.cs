using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.RequestModels;
using LogisticsBooking.FrontEnd.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class BookingOverviewModel : PageModel
    {
        private IBookingDataService bookingDataService;
        [BindProperty] public List<Booking> Bookings { get; set; }
        
        public BookingOverviewModel(IBookingDataService _bookingDataService)
        {
            bookingDataService = _bookingDataService;
            Bookings = new List<Booking>(); 
            Bookings = bookingDataService.GetBookings().Result;
            foreach (var booking in Bookings)
            {
                if (String.IsNullOrWhiteSpace(booking.transporterName)) booking.transporterName = "N/A";
                if (String.IsNullOrWhiteSpace(booking.email)) booking.email = "N/A";
                
                booking.actualArrival = default(DateTime).Add(booking.actualArrival.TimeOfDay);
                booking.endLoading = default(DateTime).Add(booking.endLoading.TimeOfDay);
                booking.startLoading = default(DateTime).Add(booking.startLoading.TimeOfDay);
                
                foreach (var order in booking.Orders)
                {
                    if (String.IsNullOrWhiteSpace(order.customerNumber)) order.customerNumber = "N/A";
                    if (String.IsNullOrWhiteSpace(order.orderNumber)) order.orderNumber = "N/A";
                    if (String.IsNullOrWhiteSpace(order.InOut)) order.InOut = "N/A";
                }
            }
        }
        
        public void OnGet(){}

        public async Task<IActionResult> OnPostUpdate(DateTime actualArrival, DateTime startLoading, DateTime endLoading, string id)
        {
            var idConverted = Guid.Parse(id);

            var bookingToUpdate =  await bookingDataService.GetBookingById(idConverted);
            bookingToUpdate.actualArrival = actualArrival;
            bookingToUpdate.startLoading = startLoading;
            bookingToUpdate.endLoading = endLoading;

            var response = await bookingDataService.UpdateBooking(bookingToUpdate);

            if (!response.IsSuccesfull) return new RedirectToPageResult("~Error");
            
            return new RedirectToPageResult("./BookingOverview");
        }
        

       
        
    }
}
