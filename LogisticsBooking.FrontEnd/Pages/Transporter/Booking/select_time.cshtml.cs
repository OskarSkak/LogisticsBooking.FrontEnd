using System;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class select_time : PageModel
    {
        private readonly IBookingDataService _bookingDataService;

        public select_time(IBookingDataService bookingDataService)
        {
            _bookingDataService = bookingDataService;
        }
        
        public void OnGet()
        {
            Console.WriteLine("");
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = "";

            try
            {
                id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex);
            }

            var test = HttpContext.Session.GetObject<Object>(id);


            var model = JsonConvert.DeserializeObject<BookingViewModel>(test.ToString());

           var result = await _bookingDataService.CreateBooking(model);

           if (!result.IsSuccesfull)
           {
               return new RedirectToPageResult("Error");
           }
            
           return  new RedirectToPageResult("confirm");


        }
    }
}