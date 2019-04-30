using System;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class OrderViewModel
    {
        public Guid id { get; set; }
        public int bookingId { get; set; }
        public string customerNumber { get; set; }
        public string orderNumber { get; set; }
        public int wareNumber { get; set; }
        public string InOut { get; set; }
        public int totalPallets { get; set; }
        public int BottomPallets { get; set; }
        
        public string SupplierName { get; set; }
    }
}