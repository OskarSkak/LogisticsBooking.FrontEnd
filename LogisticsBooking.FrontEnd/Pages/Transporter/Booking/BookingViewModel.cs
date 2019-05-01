using System;
using System.Collections.Generic;


namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class BookingViewModel
    {
        
        public int ExternalId { get; set; }
        public string BookingTime { get; set; }
        public int TotalPallets { get; set; }
        
        public int PalletsRemaining { get; set; }
        
        
        public string TransporterName { get; set; }
        
        public string email { get; set; }
        
        public List<OrderViewModel> OrderViewModels { get; set; }
        
        public IEnumerable<DataServices.Models.Supplier> Suppliers { get; set; }
    }
}