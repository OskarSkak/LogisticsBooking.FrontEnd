using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;
using Microsoft.AspNetCore.Mvc;


namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class BookingBuildModel
    {
        
        public int ExternalId { get; set; }
        
        [Required(ErrorMessage = "hdv")]
        public DateTime BookingTime { get; set; }
        
        [BindProperty]
        [Required(ErrorMessage = " tomt")]
        public int TotalPallets { get; set; }
        
        public int PalletsRemaining { get; set; }
        
        
        public string TransporterName { get; set; }
        
        public string email { get; set; }
        
        public List<OrderViewModel> OrderViewModels { get; set; }
        
        public SuppliersListViewModel Suppliers { get; set; }

        public bool isBookingAllowed { get; set; } = true;
        
        public SupplierViewModel SupplierNotAllowed { get; set; }
    }
}