using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class select_time : PageModel
    {
        private readonly IBookingDataService _bookingDataService;
        private readonly IScheduleDataService _scheduleDataService;
        private readonly IMapper _mapper;

        [BindProperty]
        public ScheduleViewModel ScheduleViewModel { get; set; }
        
        [BindProperty]
        public IntervalViewModel IntervalViewModel { get; set; }
        
        [TempData]
        public string ErrorMessage { get; set; }

        public bool ShowErrorMessage => !String.IsNullOrEmpty(ErrorMessage);

        public select_time(IBookingDataService bookingDataService , IScheduleDataService scheduleDataService , IMapper mapper)
        {
            _bookingDataService = bookingDataService;
            _scheduleDataService = scheduleDataService;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> OnGet()
        {
            var currentLoggedInUserId = GetLoggedInUserId();

            // Get the current booking View Model from the session created at previous page
            var currentBooking = HttpContext.Session.GetObject<BookingViewModel>(currentLoggedInUserId);

            // Get the schedule that match the booking. (Chech has already been made at the first page)
            var result = await _scheduleDataService.GetScheduleBydate(currentBooking.BookingTime);
            
            // remove the intervals that does not overlap with the suppliers time range. 
            // It is only possible to book a time with that match the selected suppliers on the orders. 
            RemoveIntervalsNotOverlap(currentBooking , result);
            
            
            // Set the Schedule to be shown in the view and sort it. 
            ScheduleViewModel = result;
            ScheduleViewModel.Intervals = result.Intervals.OrderBy(e => e.StartTime.Value).ToList();
            return Page();
            
        }

        

        public async Task<IActionResult> OnPostSelectedTime(string interval , ScheduleViewModel schedule)
        {
            var currentLoggedInUserId = GetLoggedInUserId();

            // Get the current booking View Model from the session created at previous page
            var currentBooking = HttpContext.Session.GetObject<BookingViewModel>(currentLoggedInUserId);



            var createBookingcommand = _mapper.Map<CreateBookingCommand>(currentBooking);
            createBookingcommand.IntervalId = Guid.Parse(interval);

            var result = await _bookingDataService.CreateBooking(createBookingcommand);

            if (result.IsSuccesfull)
            {
                return RedirectToPage("confirm"); 
            }

            ErrorMessage = "Der skete en fejl, prøv igen";
            return new RedirectToPageResult("");
        }
        
        /**
         * gets the current logged in user
         */
        private string GetLoggedInUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            
        }
        
        private bool Overlap(IntervalViewModel intervalViewModel,
            OrderViewModel orderViewModel)
        {
            
            
            TimeSpan start = orderViewModel.SupplierViewModel.DeliveryStart.TimeOfDay; // 10 PM
            TimeSpan end = orderViewModel.SupplierViewModel.DeliveryEnd.TimeOfDay;   // 2 AM
            TimeSpan start1 = intervalViewModel.StartTime.Value.TimeOfDay;
            TimeSpan end1 = intervalViewModel.EndTime.Value.TimeOfDay;

            if (start <= end)
            {
                // start and stop times are in the same day
                if (end1 > start && start1 < end)
                {
                   
                    return false;
                }
            }
            else
            {
                // start and stop times are in different days
                if (end1 > start || start1 < end)
                {
                    return false;
                }
            }

            return true;
            
            
            
        }

        private void RemoveIntervalsNotOverlap(BookingViewModel bookingViewModel, ScheduleViewModel scheduleViewModel)
        {
            foreach (var order in bookingViewModel.OrdersListViewModel.Orders)
            {
                scheduleViewModel.Intervals.RemoveAll(item => scheduleViewModel.Intervals.Any( iss => Overlap(item ,order )));
            }
        }

       

    }
}