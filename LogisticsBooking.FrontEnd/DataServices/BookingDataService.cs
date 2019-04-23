﻿using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class BookingDataService : BaseDataService, IBookingDataService
    {
        private string _url;
        private string baseurl;
        public BookingDataService(IHttpContextAccessor httpContextAccessor , IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor , config)
        {
            baseurl = _APIServerURL + "/api/bookings/";
        }
        public async Task<Response> CreateBooking(BookingViewModel _booking)
        {
            

            var response = await PostAsync<BookingViewModel>(baseurl, _booking);

            
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return new Response(false);
        }

        public async Task<List<Booking>> GetBookings()
        {
           

            var response = await GetAsync(baseurl);
            Console.WriteLine(baseurl);
    
            var result = await TryReadAsync<List<Booking>>(response);
            Console.WriteLine(result);

            return result ;
        }

        public async Task<Response> UpdateBooking(Booking booking)
        {
            

            var response = await PutAsync<Booking>(baseurl, booking);

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
            
            var result = await GetAsync(baseurl);
            return await TryReadAsync<Booking>(result);

        }
        


    }
}
