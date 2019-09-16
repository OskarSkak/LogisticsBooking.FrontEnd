using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{

    public class person
    {
        public string Name { get; set; }
    }

    public class Calendar : PageModel
    {
        private readonly IScheduleDataService _scheduleDataService;

        [BindProperty] public DateTime date { get; set; }

        public SchedulesListViewModel SchedulesListViewModel { get; set; }
        public int currentMonth { get; set; }
        public int CurrentYear { get; set; }


       
        public CalenderViewModel CalenderViewModel { get; set; }


        public Calendar(IScheduleDataService scheduleDataService)
        {
           
        }
      
        public async Task<IActionResult> OnGet()
        {
            var Subjectid = "";
            
            
            Subjectid = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            
            var calender =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calender == null)
            {
                calender = new CalenderViewModel();
            }

            CalenderViewModel = calender;

            return Page();
            /*

            SchedulesListViewModel = await _scheduleDataService.GetSchedules();
            var test = HttpContext.Session.GetObject<DataServices.Models.Schedule>("v");
            var Schedule = HttpContext.Session.GetObject<DataServices.Models.Schedule>("scheduleId");
            
            
            var Subjectid = "";
            
           
            HttpContext.Session.SetObject("v" , Schedule);
            
            Subjectid = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            var result = HttpContext.Session.GetObject<DateTime>(Subjectid);
            if (result == DateTime.MinValue)
            {
                result = date;
                result =  new DateTime(date.Year, date.Month, 1);
            }
            
            date = result;
            currentMonth = date.Month;
            CurrentYear = date.Year;
            Console.WriteLine(date);

            return Page();
            
            
            */

            
        }


        [ValidateAntiForgeryToken]
        [EnableCors("MyPolicy")]
        public async Task<IActionResult> OnPost([FromBody]string[] value)
        {
            
            /*
            List<DateTime> list = new List<DateTime>(); 
            var result = HttpContext.Session.GetObject<ScheduleViewModel>("v");
            
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != null)
                {
                    list.Add(DateTime.Parse(value[i]));
                    
                }
            }

            

            SchedulesListViewModel schedules = new SchedulesListViewModel();
            foreach (var date in list)
            {
                List<IntervalViewModel> intervals = new List<IntervalViewModel>();
                foreach (var interval in result.Intervals)
                {
                    DateTime starttime, endtime;
                    
                    if (CorrectDay(date, interval))
                    {
                        starttime = date.AddDays(1).Add(interval.StartTime.TimeOfDay);
                        endtime = date.AddDays(1).Add(interval.EndTime.TimeOfDay);
                    }
                    else
                    {
                        starttime = date.AddDays(0).Add(interval.StartTime.TimeOfDay);
                        endtime = date.AddDays(0).Add(interval.EndTime.TimeOfDay); 
                    }
                    
                    intervals.Add(new Interval
                    {
                        BookingId = interval.BookingId,
                        BottomPallets = interval.BottomPallets,
                        EndTime = endtime,
                        IntervalId = Guid.NewGuid(),
                        IsBooked = interval.IsBooked,
                        RemainingPallets = interval.RemainingPallets,
                        StartTime = starttime,
                        TransporterId = interval.TransporterId,
                        SecondaryBookingId = interval.SecondaryBookingId
                    });
                }
                
                var schedule = new DataServices.Models.Schedule
                {
                    CreatedBy = result.CreatedBy,
                    Intervals = intervals,
                    MischellaneousPallets = result.MischellaneousPallets,
                    Name = result.Name,
                    ScheduleDay = date,
                    ScheduleId = Guid.NewGuid(),
                    shift = result.shift
                };
                
                    schedules.Add(schedule);
                

            }

            var d = await _scheduleDataService.CreateManySchedule(new SchedulesListViewModel {Schedules = schedules});



            return new ObjectResult(HttpStatusCode.OK);
            
            */
            
            return new ObjectResult(HttpStatusCode.OK);

        }

        private bool CorrectDay(DateTime dateTime, IntervalViewModel interval)
        {
            
            if (1 <= interval.EndTime.Value.Hour && interval.EndTime.Value.Hour < 22)
            {
                return true;
            } 
            
            

            return false;
        }
        
        

        public IActionResult OnPostForward()
        { 
            var calendar =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calendar == null)
            {
                calendar = new CalenderViewModel();
            }

           calendar.AdvanceMonth();

           HttpContext.Session.SetObject("key" , calendar);
            
           return new RedirectToPageResult("Calendar");
            /*
            var id = "";
            
           
                id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
                var result = HttpContext.Session.GetObject<DateTime>(id);
                if (result == DateTime.MinValue)
                {
                    result =  new DateTime(date.Year, date.Month, 1);
                    
                }
                else
                {
                    result = new DateTime(result.Year, result.Month, 1);
                    
                }

                currentMonth = (result.Month + 1) % 13;

                if (currentMonth == 0)
                {
                    currentMonth++;
                    CurrentYear = result.Year;
                    CurrentYear++;
                    result = new DateTime(CurrentYear, currentMonth, 1);
                }
                else
                {
                    result = new DateTime(result.Year, currentMonth, 1);
                }
                
                
                
                HttpContext.Session.SetObject(id , result);
                */
                return new RedirectToPageResult("Calendar");
        }
    

        public void getDate()
        {
            
        }

        public IActionResult OnPostBack()
        {
            
            var calendar =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calendar == null)
            {
                calendar = new CalenderViewModel();
            }

            calendar.DecreaseMonth();

            HttpContext.Session.SetObject("key" , calendar);
            
            
            return new RedirectToPageResult("Calendar");
            /*
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var result = HttpContext.Session.GetObject<DateTime>(id);

            currentMonth = result.Month;
            CurrentYear = result.Year;
            
            if (result == DateTime.MinValue)
            {
                result =  new DateTime(date.Year, date.Month, 1);
                    
            }
            else
            {
                result = new DateTime(result.Year, result.Month, 1);
                    
            }
            if (currentMonth == 1)
            {
                currentMonth=12;
                CurrentYear-- ;
            }
            else
            {
                currentMonth--;
            }
            
            
            result = new DateTime(CurrentYear, currentMonth, 1);
            HttpContext.Session.SetObject( id , result);
            
            */
            
            DateTime myDT = new DateTime( 2002, 4, 3, new GregorianCalendar() );

           

            return new RedirectToPageResult("Calendar");

           
            
        }

        public ScheduleViewModel DateAlreadyHasSchedule(DateTime dateTime, SchedulesListViewModel schedules)
        {
            foreach (var schedule in SchedulesListViewModel.Schedules)
            {
                if (dateTime.Date == dateTime.Date)
                {
                    return schedule;
                }
            }

            return null;
        }



        
        
    }
}