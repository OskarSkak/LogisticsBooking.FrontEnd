using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class select_time : PageModel
    {
        [BindProperty]
        public BookingOrderViewModel BookingOrderViewModel { get; set; }
        
        public void OnGet(BookingOrderViewModel bookingOrderViewModel)
        {
            BookingOrderViewModel = bookingOrderViewModel;
            
        }

        public void OnTestPostAsync(BookingOrderViewModel bookingOrderViewModel)
        {
            Console.WriteLine(bookingOrderViewModel);
        }
    }
}