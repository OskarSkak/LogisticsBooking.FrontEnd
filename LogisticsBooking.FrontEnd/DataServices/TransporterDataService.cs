using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.RequestModels;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class TransporterDataService : BaseDataService, ITransporterDataService
    {

        public string baseurl = "https://localhost:44340/" + "api/transporters/";

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

        public async Task<IEnumerable<Transporter>> ListTransporters(int page, int pageSize)
        {
            var result = await GetAsync(baseurl);
            return await TryReadAsync<IEnumerable<Transporter>>(result); 
        }

        public async Task<Response> UpdateTransporter(Guid id, Transporter transporter)
        {
            var endpoint = baseurl + id;

            var response = await PutAsync<TransporterUpdateModel>(endpoint, new TransporterUpdateModel(
                transporter.Email, transporter.Telephone, transporter.Address, transporter.Name));
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
