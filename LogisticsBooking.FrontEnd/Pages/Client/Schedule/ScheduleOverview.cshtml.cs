using System;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    
    public class ScheduleOverviewIndexModel : PageModel
    {
        private readonly IScheduleDataService _scheduleDataService;

        [TempData]
        public String Message { get; set; }
        
        [TempData]
        public string CompleteMessage { get; set; }
        
        
        
        
        [BindProperty]
        public CalenderViewModel CalenderViewModel { get; set; }
        
        public SchedulesListViewModel SchedulesListViewModel { get; set; }

        public ScheduleOverviewIndexModel(IScheduleDataService scheduleDataService)
        {
            _scheduleDataService = scheduleDataService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            SchedulesListViewModel = await _scheduleDataService.GetSchedules();

            if (SchedulesListViewModel == null)
            {
                SchedulesListViewModel = new SchedulesListViewModel();
            }
            
            var calender =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calender == null)
            {
                calender = new CalenderViewModel();
            }
            
            CalenderViewModel = calender;

            foreach (var schedule in SchedulesListViewModel.Schedules)
            {
                schedule.Intervals =  schedule.Intervals.OrderBy(x => x.StartTime).ToList();
            }
            

            return Page();
        }

        
    }
}
