using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class BookOrder : PageModel
    {
        
        [BindProperty]  
        public BookingViewModel BookingOrderViewModel { get; set; }
        
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
            }

            bookingOrderViewModel.PalletsRemaining = BookingOrderViewModel.TotalPallets;
            HttpContext.Session.SetObject(id ,bookingOrderViewModel);

            
            return new RedirectToPageResult("orderinformation");
        }
    }
}