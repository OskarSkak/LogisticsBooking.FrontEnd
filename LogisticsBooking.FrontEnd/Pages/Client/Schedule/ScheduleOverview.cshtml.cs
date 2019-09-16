using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    
    public class ScheduleOverviewIndexModel : PageModel
    {
        
        [TempData]
        public String Message { get; set; }
        
        public void OnGet()
        {
            var la = "";
        }

        
    }
}