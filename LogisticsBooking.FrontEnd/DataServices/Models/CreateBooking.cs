using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class CreateBooking
    {
        public Schedule Schedule { get; set; }
        public Booking Booking { get; set; }
        public Guid IntervalId { get; set; }
    }
}