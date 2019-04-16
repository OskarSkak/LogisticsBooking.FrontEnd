using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Http;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class BookingDataService : BaseDataService, IBookingDataService
    {
        private string _url = "https://localhost:44340/" + "api/bookings/";
        public BookingDataService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            
        }
        public async Task<Response> CreateBooking(BookingViewModel _booking)
        {
            var baseurl = "https://localhost:44340/" + "api/bookings";

            var response = await PostAsync<BookingViewModel>(baseurl, _booking);

            
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return new Response(false);
        }

        public async Task<List<Booking>> GetBookings()
        {
            var baseurl = "https://localhost:44340/" + "api/bookings";

            var response = await GetAsync(baseurl);

            var result = await TryReadAsync<List<Booking>>(response);
            Console.WriteLine(result);

            return result ;
        }

        public async Task<Response> UpdateBooking(Booking booking)
        {
            var endpoint = _url + booking.internalId;

            var response = await PutAsync<Booking>(endpoint, booking);

            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(errorMsg);
                }
                return Response.Unsuccesfull(response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Booking> GetBookingById(Guid id)
        {
            var endpoint = _url + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<Booking>(result);

        }
        


    }
}
