using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule
{
    
    public enum Shift
    {
        Day,Night
    }

    
    public class ScheduleViewModel
    {
        public Guid ScheduleId { get; set; }
        
        public DateTime? ScheduleDay { get; set; }
        public Guid CreatedBy { get; set; }
        public int MischellaneousPallets { get; set; }
        public Shift Shifts { get; set; }
        public string Name { get; set; }
        
        public  List<IntervalViewModel> Intervals { get; set; }


    }
}