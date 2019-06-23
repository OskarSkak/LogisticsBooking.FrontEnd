using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class Interval
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBooked { get; set; }
        public int BottomPallets { get; set; }
        public Guid BookingId { get; set; }
        public Guid TransporterId { get; set; }
        public int RemainingPallets { get; set; }
        public Guid IntervalId { get;}
    }
}