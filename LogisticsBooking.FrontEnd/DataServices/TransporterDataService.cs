using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class TransporterDataService : BaseDataService, ITransporterDataService
    {

        public string ApiURL { get; set; }
        public TransporterDataService(IHttpContextAccessor httpContextAccessor , IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor , config)
        {
            baseurl = _APIServerURL + "/api/transporters/";
        }

        public string baseurl;

        public async Task<Response> CreateTransporter(Transporter _transporter)
        {
            var response = await PostAsync<Transporter>(baseurl, _transporter);

            if (!response.IsSuccessStatusCode)
            {
                if(response.Content != null)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(errorMessage);
                }
                return Response.Unsuccesfull(response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Transporter> GetTransporterById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<Transporter>(result);
        }
        
        public async Task<Transporter> GetTransporterByName(String name)
        {
            var service = this;
            var TransporterEnumerable = await service.ListTransporters(0, 0);
            var TransporterList = (List<Transporter>) TransporterEnumerable;
            
            foreach(var item in TransporterList)
                if (item.Name == name)
                    return item;

            return null;
        }

        public async Task<IEnumerable<Transporter>> ListTransporters(int page, int pageSize)
        {
            var result = await GetAsync(baseurl);
            if (!result.IsSuccessStatusCode)
            {
                
            }
            return await TryReadAsync<IEnumerable<Transporter>>(result); 
        }

        public async Task<Response> UpdateTransporter(Guid id, Transporter transporter)
        {
            var endpoint = baseurl + id;

            var response = await PutAsync<Transporter>(endpoint, new Transporter(
                transporter.Email, transporter.Telephone, transporter.Address, transporter.Name, transporter.ID));
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

        

        public async Task<Response> DeleteTransporter(Guid id)
        {
            var endpoint = baseurl + id;

            var response = await DeleteAsync(endpoint);
            
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
    }
}
