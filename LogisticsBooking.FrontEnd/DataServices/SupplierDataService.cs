using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.DataServices.RequestModels;
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
            var endpoint = baseurl + id;
            var response = await DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull();
                }
                return Response.Unsuccesfull(response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Supplier> GetSupplierById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<Supplier>(result);
        }

        public async Task<IEnumerable<Supplier>> ListSuppliers(int page, int pageSize)
        {
            var result = await GetAsync(baseurl);
            return await TryReadAsync<IEnumerable<Supplier>>(result);
        }

        public async Task<Supplier> GetSupplierByName(string name)
        {
            var suppliersEnumerable = await ListSuppliers(0, 0);
            var suppliersList = (List<Supplier>) suppliersEnumerable;
            foreach (var item in suppliersList)
                if (item.Name == name) return item;

            return null;
        }

        public async Task<Response> UpdateSupplier(Guid id, Supplier supplier)
        {
            var endpoint = baseurl + id;
            
            var response = await PutAsync<SupplierUpdateModel>(endpoint, new SupplierUpdateModel(
                supplier.Email, supplier.Telephone, supplier.Name));
            
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
