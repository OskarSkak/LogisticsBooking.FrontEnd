using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class SupplierDataService : BaseDataService, ISupplierDataService
    {
        public SupplierDataService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            
        }
        
        
        private string baseurl = "https://localhost:44340/" + "api/suppliers/";
        public async Task<Response> CreateSupplier(Supplier _supplier)
        {
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

        public async Task<Response> DeleteSupplier(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Supplier> GetSupplierById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Supplier>> ListSuppliers(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> UpdateSupplier(Guid id, Supplier supplier)
        {
            var endPointUrl = baseurl + id;
            return null;
        }
    }
}
