using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class BookingViewModel
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
        
        public IEnumerable<DataServices.Models.Supplier> Suppliers { get; set; }
    }
}