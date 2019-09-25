using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class select_time : PageModel
    {
        private readonly IBookingDataService _bookingDataService;
        private readonly IScheduleDataService _scheduleDataService;
        
        [BindProperty]
        public ScheduleViewModel ScheduleViewModel { get; set; }
        
        [BindProperty]
        public IntervalViewModel IntervalViewModel { get; set; }

        public select_time(IBookingDataService bookingDataService , IScheduleDataService scheduleDataService)
        {
            _bookingDataService = bookingDataService;
            _scheduleDataService = scheduleDataService;
        }
        
        public async Task<IActionResult> OnGet()
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

            var CurrentBooking = HttpContext.Session.GetObject<BookingBuildModel>(id);

            var result = await _scheduleDataService.GetScheduleBydate(CurrentBooking.BookingTime);

            ScheduleViewModel = result;
           
            return Page();
            
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

            var CurrentBooking = HttpContext.Session.GetObject<BookingBuildModel>(id);

            var result = await _scheduleDataService.GetScheduleBydate(CurrentBooking.BookingTime);

            if (result != null)
            {
                ScheduleViewModel = result;
            }
           

            
            
            var sortedList = ScheduleViewModel.Intervals.OrderBy(x => x.StartTime).ToList();
            if (sortedList != null)
            {
                ScheduleViewModel.Intervals = sortedList;
            }
           
           
            return Page();
        }

        public async Task<IActionResult> OnPostSelectedTime(string interval , ScheduleViewModel schedule)
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

            var CurrentBooking = HttpContext.Session.GetObject<BookingViewModel>(id);

            
            
 
            
            List<CreateOrderCommand> orders = new List<CreateOrderCommand>();

            foreach (var order in CurrentBooking.OrdersListViewModel.Orders)
            {
                orders.Add(new CreateOrderCommand
                {
                    
                    BottomPallets = order.BottomPallets,
                    Comments = order.Comment,
                     ExternalId = order.ExternalId,
                     // TODO InOut = order.InOut,
                    OrderNumber = order.OrderNumber,
                    SupplierId = order.SupplierViewModel.SupplierId,
                    TotalPallets = order.TotalPallets,
                    
                });
            }

            

            
            // Create a booking on the chosen interval
            await _bookingDataService.CreateBooking(new CreateBookingCommand
            {
                DeliveryDate = CurrentBooking.BookingTime,
                ExternalId = CurrentBooking.ExternalId,
                IntervalId = Guid.Parse(interval),
                TotalPallets = CurrentBooking.TotalPallets,
                TransporterId = CurrentBooking.TransporterId,
                CreateOrderCommand = orders
                
                
            });
            
            
            
            return RedirectToPage("confirm");
        }
    }
}