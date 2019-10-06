using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Itenso.TimePeriod;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;
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
        private readonly IMasterScheduleDataService _masterScheduleDataService;

        [BindProperty]
        public ScheduleViewModel ScheduleViewModel { get; set; }
        
        [BindProperty]
        public IntervalViewModel IntervalViewModel { get; set; }
        
        [TempData]
        public string ErrorMessage { get; set; }

        public bool ShowErrorMessage => !String.IsNullOrEmpty(ErrorMessage);

        public select_time(IBookingDataService bookingDataService , IScheduleDataService scheduleDataService , IMapper mapper , IMasterScheduleDataService masterScheduleDataService)
        {
            _bookingDataService = bookingDataService;
            _scheduleDataService = scheduleDataService;
            _mapper = mapper;
            _masterScheduleDataService = masterScheduleDataService;
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

            if (result == null)
            {
                await CreateScheduleFromActiveMaster(currentBooking.BookingTime);
            }

            result = await _scheduleDataService.GetScheduleBydate(currentBooking.BookingTime);
            
            RemoveIntervalsNotOverlap(currentBooking , result);
            
            
            // Set the Schedule to be shown in the view and sort it. 
            ScheduleViewModel = result;
            ScheduleViewModel.Intervals = result.Intervals.OrderBy(e => e.StartTime.Value).ToList();
            return Page();
            
        }

        private async Task CreateScheduleFromActiveMaster(DateTime bookingTime)
        {
            var result = await _masterScheduleDataService.GetActiveMasterSchedule();
            
            var scheduleViewModel = new ScheduleViewModel
            {
                Name = result.Name,
                Shifts = result.Shifts,
                CreatedBy = result.CreatedBy,
                MischellaneousPallets = result.MischellaneousPallets,
                ScheduleDay = bookingTime,
                Intervals = Fx(result.MasterIntervalStandardViewModels , bookingTime)
            };

            var scheduleCreatedResult = await _scheduleDataService.CreateSchedule(new CreateScheduleCommand
            {
                Name = scheduleViewModel.Name,
                Shifts = scheduleViewModel.Shifts,
                CreatedBy = scheduleViewModel.CreatedBy,
                MischellaneousPallets = scheduleViewModel.MischellaneousPallets,
                ScheduleDay = scheduleViewModel.ScheduleDay,
                IntervalViewModels = scheduleViewModel.Intervals
            });

        }


        private List<IntervalViewModel> Fx(List<MasterIntervalStandardViewModel> masterIntervalStandardViewModels , DateTime bookingTime)
        {
           
            var list = new List<MasterIntervalStandardViewModel>();
            foreach (var interval in masterIntervalStandardViewModels)
            {
                var masterInterval = new MasterIntervalStandardViewModel
                {
                    BottomPallets = interval.BottomPallets,
                    MasterIntervalStandardId = interval.MasterIntervalStandardId,
                    MasterScheduleStandardId = interval.MasterScheduleStandardId,
                    MasterScheduleStandardViewModel = interval.MasterScheduleStandardViewModel
                };
                
                if (interval.StartTime.Value.Hour >22 && interval.EndTime.Value.Hour < 10 )
                { 
                    
                    masterInterval.EndTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day+1 , interval.EndTime.Value.Hour , interval.EndTime.Value.Minute , interval.EndTime.Value.Second);
                    masterInterval.StartTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day , interval.StartTime.Value.Hour , interval.StartTime.Value.Minute , interval.StartTime.Value.Second);
                }
                else if (interval.StartTime.Value.Hour < 10 )
                {
                    masterInterval.EndTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day+1 , interval.EndTime.Value.Hour , interval.EndTime.Value.Minute , interval.EndTime.Value.Second);
                    masterInterval.StartTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day+1 , interval.StartTime.Value.Hour , interval.StartTime.Value.Minute , interval.StartTime.Value.Second);
                }
                else
                {
                    masterInterval.EndTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day , interval.EndTime.Value.Hour , interval.EndTime.Value.Minute , interval.EndTime.Value.Second);
                    masterInterval.StartTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day , interval.StartTime.Value.Hour , interval.StartTime.Value.Minute , interval.StartTime.Value.Second);
                }
                
                list.Add(masterInterval);
            }

            return _mapper.Map<List<IntervalViewModel>>(list);
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
            return TimeUtility.IsWithin(start, end, start1, end1);
            
        }


        private void RemoveIntervalsNotOverlap(BookingViewModel bookingViewModel, ScheduleViewModel scheduleViewModel)
        {
            foreach (var order in bookingViewModel.OrdersListViewModel.Orders)
            {
                scheduleViewModel.Intervals.RemoveAll(item => scheduleViewModel.Intervals.Any( iss => !Overlap(item ,order )));
            }


        }

       

    }
}
