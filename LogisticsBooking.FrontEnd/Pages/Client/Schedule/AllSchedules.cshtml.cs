using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    public class AllSchedules : PageModel
    {
        private readonly ITransporterDataService _transporterDataService;
        private IScheduleDataService ScheduleDataService { get; }
        [BindProperty]
        public SchedulesListViewModel SchedulesListViewModel { get; set; }
        

        public AllSchedules(IScheduleDataService scheduleDataService , ITransporterDataService transporterDataService)
        {
            _transporterDataService = transporterDataService;
            ScheduleDataService = scheduleDataService;
        }

        public async void OnGetAsync(string id)
        {
            var x = id;
            SchedulesListViewModel = ScheduleDataService.GetSchedules().Result;
        }

        public async void OnPost()
        {
            Console.WriteLine();
        }

        public async Task<JsonResult> OnGetTest()
        {
            var transporters = await ScheduleDataService.GetSchedules();

           
            var json = new JsonResult(transporters);
           
            return json;

        }

       
    }
}