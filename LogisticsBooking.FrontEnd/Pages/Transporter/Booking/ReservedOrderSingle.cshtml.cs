﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using LogisticsBooking.FrontEnd.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class ReservedOrderSingleModel : PageModel
    {
        private IOrderDataService _orderDataService;
        [BindProperty] public Order Order { get; set; }
        
        public ReservedOrderSingleModel(IOrderDataService orderDataService)
        {
            _orderDataService = orderDataService;
        }

        public async Task OnGetAsync(string id)
        {
            Order = await _orderDataService.GetOrderById(Guid.Parse(id));
            var la = "";
        }

        public async Task<IActionResult> OnPostUpdate(string ViewComment, string ViewCustomerNumber, string ViewOrderNumber, 
            int ViewWareNumber, int ViewBottomPallets, string ViewExternalId, string ViewInOut, string id, int ViewTotalPallets)
        {
            var order = new Order
            {
                Comment = ViewComment, 
                customerNumber = ViewCustomerNumber, 
                orderNumber = ViewOrderNumber, 
                wareNumber = ViewWareNumber, 
                BottomPallets = ViewBottomPallets, 
                ExternalId = ViewExternalId, 
                InOut = ViewInOut,
                id = Guid.Parse(id),
                TotalPallets = ViewTotalPallets
            };

            var result = await _orderDataService.UpdateOrder(order);

            if (!result.IsSuccesfull) return new RedirectToPageResult("Error");
            
            return new RedirectToPageResult("BookingOverviewAdmin");
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            var result = await _orderDataService.DeleteOrder(Guid.Parse(id));

            return new RedirectToPageResult("BookingOverviewAdmin");
        }
        

       
        
    }
}
