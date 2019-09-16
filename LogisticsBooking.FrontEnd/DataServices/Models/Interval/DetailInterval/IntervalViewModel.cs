using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval
{
    public class IntervalViewModel
    {
        public Guid IntervalId { get; set;  }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsBooked { get; set; }
        public int BottomPallets { get; set; }
        public int RemainingPallets { get; set; }
        
        
        
        // ************************************* Relations ******************************

        
        public Guid ScheduleId { get; set; }

        public List<BookingViewModel> Bookings { get; set; }


    }
}