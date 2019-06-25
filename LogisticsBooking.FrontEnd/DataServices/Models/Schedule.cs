using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Design;

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
        public string Name { get; set; }
        public Guid ScheduleId { get; set; }
        public Shift shift { get; set; }
    }
}