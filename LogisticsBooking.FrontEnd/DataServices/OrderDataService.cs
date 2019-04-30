using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices.Utilities
{
    public class OrderDataService : BaseDataService, IOrderDataService
    {
        private string baseurl;

        public OrderDataService(IHttpContextAccessor httpContextAccessor,
            IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/orders/";
        }

        public async Task<Response> CreateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> GetOrders()
        {
            var response = await GetAsync(baseurl);
            var result = await TryReadAsync<List<Order>>(response);
            return result;
        }

        public async Task<Order> GetOrderById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<Order>(result);
        }

        public async Task<Response> UpdateOrder(Order order)
        {
            var endpoint = baseurl + order.id;   
        
            var response = await PutAsync<Order>(endpoint, order);

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

        public async Task<Response> DeleteOrder(Guid id)
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