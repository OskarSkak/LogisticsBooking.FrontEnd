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
            foreach (var VARIABLE in HttpContext.User.Claims)
            {
                Console.WriteLine(VARIABLE.Type);
                Console.WriteLine(VARIABLE.Value);
            }
            
            if (User.HasClaim("role", "client"))
            {
                return RedirectToPage("/Client/Bookings/BookingOverview");
            }  
            
            if (User.HasClaim("role" , "transporter"))
            {
                return RedirectToPage("/Transporter/Booking/BookOrder");
            } 
            if (User.HasClaim("role" , "admin"))
            {
                return RedirectToPage("/Client/Bookings/BookingOverview");
            } 
            return new RedirectToPageResult("Error");
        }

        public async Task OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}