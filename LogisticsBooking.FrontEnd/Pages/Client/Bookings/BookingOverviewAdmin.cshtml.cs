using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.Documents;
using LogisticsBooking.FrontEnd.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class BookingOvervieAdminwModel : PageModel
    {
        private IBookingDataService bookingDataService;
        [BindProperty] public BookingsListViewModel BookingsListViewModel { get; set; } = new BookingsListViewModel();
        public BookingOvervieAdminwModel(IBookingDataService _bookingDataService)
        {
            bookingDataService = _bookingDataService;
            BookingsListViewModel.Bookings = new List<BookingViewModel>(); 
            BookingsListViewModel = bookingDataService.GetBookings().Result;
            foreach (var booking in BookingsListViewModel.Bookings)
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
    }
}
