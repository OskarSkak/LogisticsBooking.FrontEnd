using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter
{
    public class ReservedOrdersIndexModel : PageModel
    {
        private IOrderDataService OrderDataService { get; set; }
        [BindProperty] public List<Order> Orders { get; set; }
        
        public ReservedOrdersIndexModel(IOrderDataService _orderDataService)
        {
            OrderDataService = _orderDataService;
        }
        
        public async Task<IActionResult> OnGetAsync(string id)
        {
            var guid = Guid.Parse(id);
            
            Orders = new List<Order>();

            var allOrders = await OrderDataService.GetOrders();

            foreach (var order in allOrders)
            {
                if (order.bookingId == guid) Orders.Add(order);
            }

            return Page();
        }
    }
}