using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    [BindProperties]
    public class CreateNewSchedule : PageModel
    {
        
        public List<InternalInterval> Intervals { get; set; }
        
        private IScheduleDataService ScheduleDataService { get;}

        [BindProperty] public bool IsDay { get; set; } = true;


        public void OnGet()
        {
            Intervals = PopulateList(Intervals);
            foreach (var VARIABLE in Intervals)
            {
                
            }
            var la = "";
        }

        public void OnPost()
        {
            var la = "";
        }
        
        public CreateNewSchedule(IScheduleDataService scheduleDataService)
        {
            ScheduleDataService = scheduleDataService;
        }

        public async Task<IActionResult> OnPostSubmit(List<InternalInterval> intervals)
        {
            var schedule = CreateScheduleFromInternalIntervals(intervals);
            var result = await ScheduleDataService.CreateSchedule(schedule);

            HttpContext.Session.SetObject("scheduleId", schedule.ScheduleId);
            
            return new RedirectToPageResult("Calendar");
            return Page();
        }

        private DataServices.Models.Schedule CreateScheduleFromInternalIntervals(List<InternalInterval> intervals)
        {
            var Schedule = new DataServices.Models.Schedule();
            Schedule.Intervals = new List<Interval>();

            foreach (var internalInterval in intervals)
            {
                if (internalInterval.BottomPallets != 0 && internalInterval.StartTime != DateTime.MinValue
                    && internalInterval.EndTime != DateTime.MinValue)
                {
                    var interval = new Interval
                    {
                        BottomPallets = internalInterval.BottomPallets, 
                        StartTime = internalInterval.StartTime, 
                        EndTime = internalInterval.EndTime
                    };
                    
                    Schedule.Intervals.Add(interval);
                }
            }
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            Schedule.ScheduleId = Guid.NewGuid();
            
            Schedule.CreatedBy = Guid.Parse(id);

            return Schedule;
        }

        public List<InternalInterval> PopulateList(List<InternalInterval> Intervals)
        {
            Intervals = new List<InternalInterval>();
            DateTime Time = new DateTime();
            if (IsDay)
            {
                Time = Convert.ToDateTime("07:00");
            }
            else
            {
                Time = Convert.ToDateTime("22:00");
            }
            
            for (int i = 0; i < 11; i++)
            {
                var interval = new InternalInterval();
                interval.StartTime = Time.AddMinutes(i * 45);
                interval.EndTime = Time.AddMinutes((i * 45) + 45);
                interval.BottomPallets = 33;
                Intervals.Add(interval);
                interval.InternalId = i;
                interval.IsShown = true;
            }

            for (int i = 11; i < 27; i++)
            {
                var interval = new InternalInterval();
                interval.InternalId = i;
                Intervals.Add(interval);
            }

            return Intervals;
        }

        //DAY: 8 timer per default 7-15
        //Night: 8 timer 22-06
    }

    public class InternalInterval
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int InternalId { get; set; }
        public int BottomPallets { get; set; }
        public bool IsShown { get; set; }
    }
}