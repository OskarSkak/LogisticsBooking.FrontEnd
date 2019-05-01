using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
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
            

            var booking = new DataServices.Models.Booking
            {
                totalPallets = model.TotalPallets,
                ExternalId = model.ExternalId,
                port = 0,
                email = model.email,
                transporterName = model.TransporterName

            };

            booking.Orders = new List<Order>();
            foreach (var order in model.OrderViewModels)
            {
                booking.Orders.Add(new Order
                {
                    orderNumber = order.orderNumber,
                    ExternalId = order.ExternalId,
                    TotalPallets = order.totalPallets,
                    BottomPallets = order.totalPallets,
                    customerNumber = order.customerNumber,
                    SupplierName = order.SupplierName
                });
            }
            
            
           var result = await _bookingDataService.CreateBooking(booking);
           
           

           if (!result.IsSuccesfull)
           {
               return new RedirectToPageResult("Error");
           }
            
           return  new RedirectToPageResult("confirm");


        }
    }
}