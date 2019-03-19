using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class orderinformation : PageModel
    {
        [BindProperty]
        public BookingOrderViewModel BookingOrderViewModel { get; set; }
        
        public void OnGet(BookingOrderViewModel bookingOrderViewModel)
        {
            BookingOrderViewModel = bookingOrderViewModel;
            
        }
    }
}