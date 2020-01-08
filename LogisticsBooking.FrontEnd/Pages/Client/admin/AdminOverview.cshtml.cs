using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.admin
{
    
    
    public class AdminOverview : PageModel
    {
        
        public List<Claim> claims { get; set; }
        public void OnGet()
        {
            claims = new List<Claim>();
            foreach (var claim in User.Claims)
            {
                Console.WriteLine(claim.Type + "value : " + claim.Value);
                claims.Add(claim);
            }

            
        }
    }
}