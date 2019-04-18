using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Suppliers
{
    public class Supplier_CreateModel : PageModel
    {
        private ISupplierDataService _dataService { get; set; }
        public void OnGet()
        {
        }

        public Supplier_CreateModel(ISupplierDataService DS)
        {
            _dataService = DS;
        }

        public async Task<ActionResult> OnPost(string Name, string Email, int Telephone)
        {
            var result = await _dataService.CreateSupplier(new DataServices.Models.Supplier
            {
                Email = Email,
                Telephone = Telephone,
                Name = Name
            });

            if (!result.IsSuccesfull)
            {
                return new RedirectResult("Error");
            }

            return new RedirectResult("Suppliers");
        }
    }
}