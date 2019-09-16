using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    public class AllSchedules : PageModel
    {
        private IScheduleDataService ScheduleDataService { get; }
        [BindProperty]
        public SchedulesListViewModel SchedulesListViewModel { get; set; }
        

        public AllSchedules(IScheduleDataService scheduleDataService)
        {
            ScheduleDataService = scheduleDataService;
            
        }

        public async void OnGetAsync(string id)
        {
            var x = id;
            SchedulesListViewModel = ScheduleDataService.GetSchedules().Result;
        }
    }
}