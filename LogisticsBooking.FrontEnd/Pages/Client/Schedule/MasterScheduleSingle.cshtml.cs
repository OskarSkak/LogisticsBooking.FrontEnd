using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    public class MasterScheduleSingle : PageModel
    {
        private readonly IScheduleDataService _scheduleDataService;
        

        public MasterScheduleSingle(IScheduleDataService scheduleDataService)
        {
            _scheduleDataService = scheduleDataService;
        }
        public void OnGet()
        {
            
        }

        
        
    }
}