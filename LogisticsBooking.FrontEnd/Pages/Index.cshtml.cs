using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.DataServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public async Task OnPostSubmit()
        {
            var test = new BookingDataService();
            var a = await test.CreateBooking(new DataServices.Models.Booking("Test")); 
        }
    }
}
