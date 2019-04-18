using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.RequestModels;
using LogisticsBooking.FrontEnd.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private ITransporterDataService transporterDataService;
        public IndexModel(ITransporterDataService _transporterDataService)
        {
            transporterDataService = _transporterDataService;
        }
        public void OnGet()
        {
            
        }
        
        public async Task  OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");

            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //
            return new RedirectResult("User/Home");
        }
    }
}
