using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Suppliers
{
    public class Supplier_SingleModel : PageModel
    {
        private ISupplierDataService supplierDataService;
        [BindProperty] public Supplier supplier { get; set;}
        
        public Supplier_SingleModel(ISupplierDataService _dataService){supplierDataService = _dataService;}

        public async Task<IActionResult> OnGetAsync(string id)
        {
            supplier = await supplierDataService.GetSupplierByName(id);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdate(string id, int ViewTelephone, string ViewEmail, string ViewName)
        {
            var supplier = await supplierDataService.GetSupplierByName(id);
            var updatedSupplier = new Supplier
            {
                Email = ViewEmail, 
                Name = ViewName, 
                Telephone = ViewTelephone
            };

            var result = await supplierDataService.UpdateSupplier(supplier.ID, updatedSupplier);

            if (result.IsSuccesfull) return new RedirectToPageResult("./Suppliers");
            
            return new RedirectToPageResult("Error");
        }
        
        public async Task<IActionResult> OnPostDelete(string id)
        {
            var supplier = await supplierDataService.GetSupplierByName(id);

            var result = await supplierDataService.DeleteSupplier(supplier.ID);
            
            return new RedirectToPageResult("./Suppliers");
        }
    }
}