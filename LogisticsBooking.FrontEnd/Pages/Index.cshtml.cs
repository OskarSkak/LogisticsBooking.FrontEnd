using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public IActionResult OnPostSubmit()
        {
            return new RedirectResult("Client/ClientIndex");
        }
    }
}
