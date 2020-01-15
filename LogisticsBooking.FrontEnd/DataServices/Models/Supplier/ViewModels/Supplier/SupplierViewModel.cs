using System;
using System.ComponentModel.DataAnnotations;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier
{
    public class SupplierViewModel
    {
        public string Email { get; set; }
        public int Telephone { get; set; }
        
        [Display(Name = "Name"), Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime ArriveTime { get; set; }
        // Leverings vindue for kunden
        public DateTime DeliveryStart { get; set; }
        public DateTime DeliveryEnd { get; set; }
    }
}