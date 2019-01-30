using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class BookingDataService : BaseDataService, IBookingDataService
    {
        public async Task<Response> CreateBooking(Booking _booking)
        {
            var baseurl = "https://localhost:44340/" + "api/orders";

            var response = await PostAsync<Booking>(baseurl, _booking); 


            return null;
        }


    }
}
