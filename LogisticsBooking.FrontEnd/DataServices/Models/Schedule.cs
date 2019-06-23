using System;
using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public enum Shift
    {
        Day, Night
    }
    public class Schedule
    {
        public List<Interval> Intervals { get; set; }
        public DateTime ScheduleDay { get; set; }
        public Guid CreatedBy { get; set; }
        public int MischellaneousPallets { get; set; }
        
        public Guid ScheduleId { get; }
    }
}