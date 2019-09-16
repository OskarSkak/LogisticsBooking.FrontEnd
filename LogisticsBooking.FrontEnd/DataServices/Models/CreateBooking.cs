using System;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class CreateBooking
    {
        public ScheduleViewModel Schedule { get; set; }
        public BookingViewModel Booking { get; set; }
        public Guid IntervalId { get; set; }
    }
}