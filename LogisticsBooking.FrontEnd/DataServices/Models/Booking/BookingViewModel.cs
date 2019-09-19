using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Booking
{
    public class BookingViewModel
    {
        public int ExternalId { get; set; }
        public int TotalPallets { get; set; }
        public DateTime BookingTime { get; set; }
        public string TransporterName { get; set; }
        public int Port { get; set; }
        
        
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime ActualArrival { get; set; }
        
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime StartLoading { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime EndLoading { get; set; }
        
        
        public Guid InternalId { get; set; }
        public string Email { get; set; }
        
        public List<Order> Orders { get; set; }
        
        public Guid TransporterId { get; set; }
        
        public int PalletsRemaining { get; set; }
        
        public SuppliersListViewModel SuppliersListViewModel { get; set; }
        
        public List<OrderViewModel> OrderViewModels { get; set; }
        
        public bool IsBookingAllowed { get; set; }
        
        public int PalletsCurrentlyOnBooking { get; set; }

        public BookingViewModel()
        {
            
        }
    }
}
