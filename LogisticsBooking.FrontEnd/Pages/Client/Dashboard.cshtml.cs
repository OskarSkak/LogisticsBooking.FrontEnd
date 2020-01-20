using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Dashboard;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoreLinq.Extensions;

namespace LogisticsBooking.FrontEnd.Pages.Client
{
    public class Dashboard : PageModel
    {
        private readonly IMasterScheduleDataService _masterScheduleDataService;
        private readonly IDashboardDataService _dashboardDataService;


        [BindProperty]
        public MasterSchedulesStandardViewModel MasterSchedulesStandardViewModel { get; set; }

        [BindProperty]
        public DashboardViewModel DashboardViewModel { get; set; }

        public Dashboard(IMasterScheduleDataService masterScheduleDataService , IDashboardDataService dashboardDataService)
        {
            _masterScheduleDataService = masterScheduleDataService;
            _dashboardDataService = dashboardDataService;
            
        }
        
        public async Task OnGet()
        {
           
            var result = await _masterScheduleDataService.GetActiveMasterSchedule();

            MasterSchedulesStandardViewModel = CreateMasterSchedules(result);

            DashboardViewModel = await _dashboardDataService.GetDashboard();


            var percent = ((24 - DashboardViewModel.TimeToNextDelivery.Hours) / 24) *100;
            Console.WriteLine();
        }


        private MasterSchedulesStandardViewModel CreateMasterSchedules(MasterSchedulesStandardViewModel masterSchedulesStandardViewModel)
        {
            List<MasterIntervalStandardViewModel> DayIntervale = new List<MasterIntervalStandardViewModel>();
            List<MasterIntervalStandardViewModel> NightInterval = new List<MasterIntervalStandardViewModel>();

            foreach (var masterScheduleStandardView in masterSchedulesStandardViewModel.MasterScheduleStandardViewModels)
            {
                if (masterScheduleStandardView.Shifts == Shift.Day)
                {
                    DayIntervale =
                        masterScheduleStandardView.MasterIntervalStandardViewModels.OrderBy(e => e.StartTime).ToList();

                    masterScheduleStandardView.MasterIntervalStandardViewModels = DayIntervale;
                }
                else
                {
                    NightInterval = masterScheduleStandardView.MasterIntervalStandardViewModels.OrderByDescending(e => e.StartTime)
                        .ToList();

                    masterScheduleStandardView.MasterIntervalStandardViewModels = NightInterval;
                }

                
            }

            return masterSchedulesStandardViewModel;
        }

        
    }
}