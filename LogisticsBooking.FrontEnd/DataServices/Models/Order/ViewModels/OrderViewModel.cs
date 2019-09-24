using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class OrderViewModel
    {
        
        public string Comment { get; set; }
        
        public int TotalPallets { get; set; }
        public int BottomPallets { get; set; }
        public string ExternalId { get; set; }
        
        public Guid id { get; set; }
        
        
        public Guid bookingId { get; set; }
        public string customerNumber { get; set; }
        public string orderNumber { get; set; }
        public int wareNumber { get; set; }
        public string InOut { get; set; }
        public string SupplierName { get; set; }
        
        public Guid SupplierId { get; set; }
    }
}