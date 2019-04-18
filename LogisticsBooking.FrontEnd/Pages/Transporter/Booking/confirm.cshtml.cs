using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class confirm : PageModel
    {
        public void OnGet(BookingViewModel bookingOrderViewModel)
        {
            Console.WriteLine(bookingOrderViewModel);

           // ViewData["BookingDate"] = HttpContext.Session.GetString("BookingDate");

           var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
           
            var test = HttpContext.Session.GetObject<Object>(id);
          

            var model = JsonConvert.DeserializeObject<BookingViewModel>(test.ToString());

            Console.WriteLine(model);
           // Console.WriteLine(test1);
          //  Console.WriteLine(test);
          
          
            
            
        }
    }
}