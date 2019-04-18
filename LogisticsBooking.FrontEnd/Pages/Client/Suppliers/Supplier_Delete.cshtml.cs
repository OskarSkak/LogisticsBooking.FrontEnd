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
    public class Supplier_DeleteModel : PageModel
    {
        private ISupplierDataService supplierDataService;
        [BindProperty] public Supplier supplier { get; set;}
        
        public Supplier_DeleteModel(ISupplierDataService _dataService){supplierDataService = _dataService;}

        public async Task<IActionResult> OnGetAsync(string id)
        {
            supplier = await supplierDataService.GetSupplierByName(id);
            return Page();
        }
        
        
    }
}