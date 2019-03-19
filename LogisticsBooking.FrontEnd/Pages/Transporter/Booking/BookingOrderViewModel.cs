using System;
using System.Runtime.Serialization;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class BookingOrderViewModel
    {
        
        public DateTime BookingDate { get; set; }
        public int TotalPallets { get; set; }
    }
}