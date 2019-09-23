using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class BookOrder : PageModel
    {
        private readonly IUtilBookingDataService _utilBookingDataService;
        private readonly IScheduleDataService _scheduleDataService;

        [BindProperty]  
        public BookingViewModel BookingViewModel { get; set; }

        [TempData]
        public String ScheduleAvailableMessage { get; set; }

        public bool ShowMessage => !String.IsNullOrEmpty(ScheduleAvailableMessage);
        
        public BookOrder(IUtilBookingDataService utilBookingDataService , IScheduleDataService scheduleDataService)
        {
            _utilBookingDataService = utilBookingDataService;
            _scheduleDataService = scheduleDataService;
        }
        
        public void OnGet()
        {
            
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = GetUserId();
            
            var externalBookingId  = await _utilBookingDataService.GetBookingNumber();
            
            //TODO check if schedule exists

            BookingViewModel.PalletsRemaining = BookingViewModel.TotalPallets;
            BookingViewModel.ExternalId = externalBookingId.bookingid;
            
            HttpContext.Session.SetObject(id ,BookingViewModel);

            var schedule = await _scheduleDataService.GetScheduleBydate(BookingViewModel.BookingTime);

            if (schedule == null)
            {
                ScheduleAvailableMessage = "Det er ikke muligt at booke på denne dag, vælg en ny";
                return Page();
            }
            return new RedirectToPageResult("orderinformation");
        }
        
        public string GetUserId() {
            
            var id = "";
            
            try
            {
                return User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
    
   
}