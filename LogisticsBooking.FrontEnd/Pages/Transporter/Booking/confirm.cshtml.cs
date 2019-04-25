using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class confirm : PageModel
    {
        private readonly IBookingDataService _bookingDataService;


        public confirm(IBookingDataService bookingDataService)
        {
            _bookingDataService = bookingDataService;
        }
        public async Task<IActionResult> OnGet(BookingViewModel bookingOrderViewModel)
        {
            Console.WriteLine(bookingOrderViewModel);

           // ViewData["BookingDate"] = HttpContext.Session.GetString("BookingDate");

           var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
           
            var test = HttpContext.Session.GetObject<Object>(id);
          

            var model = JsonConvert.DeserializeObject<BookingViewModel>(test.ToString());

             var result = await _bookingDataService.CreateBooking(model);

             return new RedirectToPageResult("confirm");
        
        }
    }
}