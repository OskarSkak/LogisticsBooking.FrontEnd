using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Booking
{
    public class BookingViewModel
    {
        public int ExternalId { get; set; }
        public int totalPallets { get; set; }
        public DateTime bookingTime { get; set; }
        public string transporterName { get; set; }
        public int port { get; set; }
        
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime actualArrival { get; set; }
        
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime startLoading { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime endLoading { get; set; }
        
        
        public Guid internalId { get; set; }
        public string email { get; set; }
        
        public List<Order> Orders { get; set; }
        
        public Guid TransporterId { get; set; }

        public BookingViewModel()
        {
            
        }
    }
}
