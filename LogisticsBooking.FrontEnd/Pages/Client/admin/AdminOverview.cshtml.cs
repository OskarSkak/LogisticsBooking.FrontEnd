using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.admin
{
    
    [Authorize(Roles = "admin")]
    public class AdminOverview : PageModel
    {
        
        public List<Claim> claims { get; set; }
        public void OnGet()
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine(claim.Type + "value : " + claim.Value);
            }

            claims = User.Claims.ToList();
        }
    }
}