using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Suppliers
{
    public class Supplier_SingleModel : PageModel
    {
        private ISupplierDataService supplierDataService;
        [BindProperty] public SupplierViewModel supplier { get; set;}
        
        public Supplier_SingleModel(ISupplierDataService _dataService){supplierDataService = _dataService;}

        public async Task<IActionResult> OnGetAsync(string id)
        {
            supplier = await supplierDataService.GetSupplierByName(id);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdate(string id, int ViewTelephone, string ViewEmail, string ViewName , DateTime ViewDeliveryStart , DateTime ViewDeliveryEnd)
        {
            var supplier = await supplierDataService.GetSupplierByName(id);
            var updatedSupplier = new SupplierViewModel
            {
                Email = ViewEmail, 
                Name = ViewName, 
                Telephone = ViewTelephone,
                DeliveryEnd = ViewDeliveryEnd,
                DeliveryStart = ViewDeliveryStart,
                SupplierId = supplier.SupplierId
            };
            updatedSupplier.DeliveryStart = ViewDeliveryStart;
            updatedSupplier.DeliveryEnd = ViewDeliveryEnd;

            var result = await supplierDataService.UpdateSupplier(supplier.SupplierId, updatedSupplier);

            if (result.IsSuccesfull) return new RedirectToPageResult("./Suppliers");
            
            return new RedirectToPageResult("Error");
        }
        
        public async Task<IActionResult> OnPostDelete(string id)
        {
            var supplier = await supplierDataService.GetSupplierByName(id);

            var result = await supplierDataService.DeleteSupplier(supplier.SupplierId);
            
            return new RedirectToPageResult("./Suppliers");
        }
    }
}