using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LogisticsBooking.FrontEnd.Pages.Client
{
    public class SuppliersModel : PageModel
    {
        private ISupplierDataService _supplierDataService;
        [BindProperty] public List<Supplier> Suppliers { get; set; }

        public SuppliersModel(ISupplierDataService supplierDataService)
        {
            _supplierDataService = supplierDataService;
            Suppliers = PopulateList(supplierDataService).Result; 
        }
        
        public void OnGet()
        {
        }

        private static async Task<List<Supplier>> PopulateList(ISupplierDataService supplierDataService)
        {
            var suppliersEnumerable = await supplierDataService.ListSuppliers(0, 0);
            var suppliersList = (List<Supplier>) suppliersEnumerable;
            return suppliersList;
        }
    }
}