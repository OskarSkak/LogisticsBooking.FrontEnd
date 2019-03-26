using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class Order
    {
        public Guid id { get; set; }
        
        
        public Guid bookingId { get; set; }
        public string customerNumber { get; set; }
        public string orderNumber { get; set; }
        public int wareNumber { get; set; }
        public string InOut { get; set; }
    }
}