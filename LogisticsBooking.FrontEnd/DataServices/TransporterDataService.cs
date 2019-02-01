using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class TransporterDataService : BaseDataService, ITransporterDataService
    {
        public async Task<Response> CreateTransporter(Transporter _transporter)
        {
            var baseurl = "https://localhost:44340/" + "api/transporters";
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
    }
}
