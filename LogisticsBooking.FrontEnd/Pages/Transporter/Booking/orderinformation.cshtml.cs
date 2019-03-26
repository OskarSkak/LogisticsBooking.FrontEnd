using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class orderinformation : PageModel
    {
        private readonly ISupplierDataService _supplierDataService;
        


        public BookingViewModel BookingViewModel { get; set; }
        
        public OrderViewModel OrderViewModel { get; set; }
        
        public List<SelectListItem> Transporters { get; set;}
        
        
        


        public orderinformation(ISupplierDataService supplierDataService)
        {
            _supplierDataService = supplierDataService;
            
        }
        public async Task OnGetAsync()
        {
            var id = "";
            
            try
            {
                id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex);
            }
            
            var test = HttpContext.Session.GetObject<Object>(id);
          

            var model = JsonConvert.DeserializeObject<BookingViewModel>(test.ToString());

            BookingViewModel = model;

            BookingViewModel.Suppliers = await _supplierDataService.ListSuppliers(0, 0);
            
            CreateSelectedList(BookingViewModel.Suppliers.ToList());
            


        }

        public async Task<IActionResult> OnPostAsync(OrderViewModel orderViewModel ) 
        {

            Console.WriteLine(orderViewModel);
            
            var id = "";
            
            try
            {
                id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex);
            }
            
            var test = HttpContext.Session.GetObject<Object>(id);
          

            var model = JsonConvert.DeserializeObject<BookingViewModel>(test.ToString());

            List<OrderViewModel> orderViewModels = null;
            
            if (model.OrderViewModels == null)
            {
                orderViewModels = new List<OrderViewModel>
                {
                    new OrderViewModel 
                    {
                        orderNumber = orderViewModel.orderNumber,
                        bookingId = orderViewModel.bookingId,
                        BottomPallets = orderViewModel.BottomPallets,
                        customerNumber = orderViewModel.customerNumber,
                        id = orderViewModel.id,
                        InOut = orderViewModel.InOut,
                        totalPallets = orderViewModel.totalPallets,
                        wareNumber = orderViewModel.wareNumber,
                        SupplierName = orderViewModel.SupplierName
                        
                    }    
                };
            }
            else
            {
                model.OrderViewModels.Add(new OrderViewModel
                {
                    orderNumber = orderViewModel.orderNumber,
                    bookingId = orderViewModel.bookingId,
                    BottomPallets = orderViewModel.BottomPallets,
                    customerNumber = orderViewModel.customerNumber,
                    id = orderViewModel.id,
                    InOut = orderViewModel.InOut,
                    totalPallets = orderViewModel.totalPallets,
                    wareNumber = orderViewModel.wareNumber,
                    SupplierName = orderViewModel.SupplierName
                    
                });
            }


            if (orderViewModels != null)
            {
                model.OrderViewModels = orderViewModels;
            }
           
            
      
            HttpContext.Session.SetObject(id , model);
            
            return new RedirectToPageResult("orderinformation");

        }

        public void CreateSelectedList(List<DataServices.Models.Supplier> transporters) 
        {
            Transporters = new List<SelectListItem>();

            foreach (var transporter in transporters)
            {
                Transporters.Add(new SelectListItem{ Value = transporter.Name ,Text = transporter.Name});
            }
        }
    }
}