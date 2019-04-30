using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class Booking
    {
        public int ExternalId { get; set; }
        public int totalPallets { get; set; }
        public DateTime bookingTime { get; set; }
        public string transporterName { get; set; }
        public int port { get; set; }
        public DateTime actualArrival { get; set; }
        public DateTime startLoading { get; set; }
        public DateTime endLoading { get; set; }
        
        
        public Guid internalId { get; set; }
        public string email { get; set; }
        
        public List<Order> Orders { get; set; }
        
        public Guid TransporterId { get; set; }

        public Booking()
        {
            
        }
    }
}
