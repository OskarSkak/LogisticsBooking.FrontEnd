using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
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

        [BindProperty] 
        public DateTime date { get; set; }
        
        public int currentMonth { get; set; }
        public int CurrentYear { get; set; }


        public Calendar(IScheduleDataService scheduleDataService)
        {
            _scheduleDataService = scheduleDataService;
            date = DateTime.Today;
        }
      
        public async Task<IActionResult> OnGet()
        {

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
        }


        [ValidateAntiForgeryToken]
        [EnableCors("MyPolicy")]
        public async Task<IActionResult> OnPost([FromBody]string[] value)
        {
            List<DateTime> list = new List<DateTime>(); 
            var result = HttpContext.Session.GetObject<DataServices.Models.Schedule>("v");
            
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != null)
                {
                    list.Add(DateTime.Parse(value[i]));
                    
                }
            }
            
            List<DataServices.Models.Schedule> schedules = new List<DataServices.Models.Schedule>();
            foreach (var date in list)
            {
                var schedule = new DataServices.Models.Schedule
                {
                    CreatedBy = result.CreatedBy,
                    Intervals = result.Intervals,
                    MischellaneousPallets = result.MischellaneousPallets,
                    Name = result.Name,
                    ScheduleDay = date,
                    ScheduleId = Guid.NewGuid(),
                    shift = result.shift
                };
                
                schedules.Add(schedule);

            }

            Console.WriteLine(schedules);

            var d = await _scheduleDataService.CreateManySchedule(new ManySchedules {Schedules = schedules});



            return new ObjectResult(HttpStatusCode.OK);

        }

        public IActionResult OnPostForward()
        {
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
                
                return new RedirectToPageResult("Calendar");
        }
    

        public void getDate()
        {
            
        }

        public IActionResult OnPostBack()
        {
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


            return new RedirectToPageResult("Calendar");
        }
        
        
    }
}