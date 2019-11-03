using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.User
{
    [Authorize]
    public class Home : PageModel
    {
        [Authorize]
        public async Task<IActionResult> OnGet()
        {
            if (User.IsInRole("client"))
            {
                return RedirectToPage("/Client/Bookings/BookingOverview");
            }  
            
            if (User.IsInRole("transporter"))
            {
                return RedirectToPage("/Transporter/Booking/BookOrder");
            } 
            if (User.IsInRole("admin"))
            {
                return RedirectToPage("/Client/Bookings/BookingOverview");
            } 
            
             return RedirectToPage("/Client/Bookings/BookingOverview");
        }

        public async Task OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}