﻿using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
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
        public async Task<Response> CreateBooking(CreateBooking booking)
        {
            

            var response = await PostAsync<CreateBooking>(baseurl, booking);

            
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return new Response(false);
        }

        public async Task<BookingsListViewModel> GetBookings()
        {
            var response = await GetAsync(baseurl);
            var result = await TryReadAsync<BookingsListViewModel>(response);
            Console.WriteLine(result);

            return result;
        }

        public async Task<BookingsListViewModel> GetBookingsInbetweenDates(DateTime from, DateTime to)
        {
            var endpoint = baseurl + from.ToString("MM-dd-yyyy") + "/" + to.ToString("MM-dd-yyyy");
            var response = await GetAsync(endpoint);
            var result = await TryReadAsync<BookingsListViewModel>(response);
            return result;
        }

        public async Task<Response> UpdateBooking(BookingViewModel booking)
        {
            var endpoint = baseurl + booking.internalId;   
        
            var response = await PutAsync<BookingViewModel>(endpoint, booking);

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

        public async Task<BookingViewModel> GetBookingById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<BookingViewModel>(result);

        }
        
        public async Task<Response> DeleteBooking(Guid id)
        {
            var endpoint = baseurl + id;
            var response = await DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(errorMessage);
                }
                return Response.Unsuccesfull(response.ReasonPhrase);
            }
            return Response.Succes();
        }

    }
}
