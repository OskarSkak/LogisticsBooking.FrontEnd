using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class SupplierDataService : BaseDataService, ISupplierDataService
    {
        public async Task<Response> CreateSupplier(Supplier _supplier)
        {
            var baseurl = "https://localhost:44340/" + "api/suppliers";
            var response = await PostAsync<Supplier>(baseurl, _supplier);

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
