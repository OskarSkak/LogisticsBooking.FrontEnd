using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class BookOrder : PageModel
    {
        private readonly IUtilBookingDataService _utilBookingDataService;

        [BindProperty]  
        public BookingViewModel BookingOrderViewModel { get; set; }


        public BookOrder(IUtilBookingDataService utilBookingDataService)
        {
            _utilBookingDataService = utilBookingDataService;
        }
        
        public void OnGet()
        {
            
            Console.WriteLine();
        }

        public async Task<IActionResult> OnPostAsync(BookingViewModel bookingOrderViewModel)
        {
            // Get the logged in transporter       
            var id = "";
            
            try
            {
                id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex);
                return Page();
            }

            var bookingid = await _utilBookingDataService.GetBookingNumber();

            bookingOrderViewModel.PalletsRemaining = BookingOrderViewModel.TotalPallets;
            
            HttpContext.Session.SetObject(id ,bookingOrderViewModel);
            HttpContext.Session.SetObject(bookingid.bookingid.ToString() , 1);
            
            return new RedirectToPageResult("orderinformation");
        }
    }
}