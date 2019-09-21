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
                if (String.IsNullOrWhiteSpace(booking.TransporterName)) booking.TransporterName = "N/A";
                if (String.IsNullOrWhiteSpace(booking.Email)) booking.Email = "N/A";
                
                booking.ActualArrival = default(DateTime).Add(booking.ActualArrival.TimeOfDay);
                booking.EndLoading = default(DateTime).Add(booking.EndLoading.TimeOfDay);
                booking.StartLoading = default(DateTime).Add(booking.StartLoading.TimeOfDay);
                
                foreach (var order in booking.OrderViewModels)
                {
                    if (String.IsNullOrWhiteSpace(order.customerNumber)) order.customerNumber = "N/A";
                    if (String.IsNullOrWhiteSpace(order.orderNumber)) order.orderNumber = "N/A";
                    if (String.IsNullOrWhiteSpace(order.InOut)) order.InOut = "N/A";
                }
            }
        }
    }
}
