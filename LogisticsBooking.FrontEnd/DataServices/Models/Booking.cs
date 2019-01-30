using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class Booking
    {
        string orderName;

        public Booking(string name)
        {
            orderName = name;
        }
    }
}
