using System;
using System.ComponentModel.DataAnnotations;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class OrderViewModel
    {
        public Guid OrderId { get; set; }
        public Guid bookingId { get; set; }
        

        public string Comment { get; set; }
        public string ExternalId { get; set; }
        
        public int createdOrders { get; set; }
        public string customerNumber { get; set; }
        public string orderNumber { get; set; }
        public int wareNumber { get; set; }
        public string InOut { get; set; }
        public int totalPallets { get; set; }
        public int BottomPallets { get; set; }

        public Guid SupplierId { get; set; }
        
        public string SupplierName { get; set; }
        
        public SupplierViewModel SupplierViewModel {get; set; }
    }
}