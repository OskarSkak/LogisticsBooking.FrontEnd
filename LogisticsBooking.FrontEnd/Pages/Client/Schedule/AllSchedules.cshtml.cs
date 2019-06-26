using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    public class AllSchedules : PageModel
    {
        private IScheduleDataService ScheduleDataService { get; }
        [BindProperty]
        public List<DataServices.Models.Schedule> Schedules { get; set; }
        

        public AllSchedules(IScheduleDataService scheduleDataService)
        {
            ScheduleDataService = scheduleDataService;
            
        }

        public async void OnGetAsync(string id)
        {
            var x = id;
            Schedules = ScheduleDataService.GetSchedules().Result;
        }
    }
}