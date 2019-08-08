using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;
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
        private readonly IUtilBookingDataService _utilBookingDataService;


        [BindProperty]
        public BookingViewModel BookingViewModel { get; set; }
        
        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }
        
        [BindProperty]
        public List<SelectListItem> Transporters { get; set;}
        
        public bool IsSupplierAllowed { get; set; }
        
        public int palletsRemaining { get; set; }
        
        [BindProperty]
        public Guid id { get; set; }

       

        public orderinformation(ISupplierDataService supplierDataService , IUtilBookingDataService utilBookingDataService)
        {
            _supplierDataService = supplierDataService;
            _utilBookingDataService = utilBookingDataService;
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
          
            var externalId  = await _utilBookingDataService.GetBookingNumber();
            var model = JsonConvert.DeserializeObject<BookingViewModel>(test.ToString());

            
            BookingViewModel = model;

            BookingViewModel.ExternalId = externalId.bookingid;
            BookingViewModel.Suppliers = await _supplierDataService.ListSuppliers(0, 0);
            
            CreateSelectedList(BookingViewModel.Suppliers);
            


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
          
            var externalId  = await _utilBookingDataService.GetBookingNumber();
            var model = JsonConvert.DeserializeObject<BookingViewModel>(test.ToString());
            var nextOrder = HttpContext.Session.GetObject<int>(externalId.bookingid.ToString());
            model.ExternalId = externalId.bookingid;
            List<OrderViewModel> orderViewModels = null;
            model.PalletsRemaining -= orderViewModel.BottomPallets;
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
                        id = Guid.NewGuid(),
                        InOut = orderViewModel.InOut,
                        totalPallets = orderViewModel.totalPallets,
                        wareNumber = orderViewModel.wareNumber,
                        SupplierName = orderViewModel.SupplierName,
                        ExternalId = externalId.bookingid + "-" + nextOrder.ToString("D2"),
                        createdOrders = 2,
                        Comment = orderViewModel.Comment
                        
                        
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
                    id = Guid.NewGuid(),
                    InOut = orderViewModel.InOut,
                    totalPallets = orderViewModel.totalPallets,
                    wareNumber = orderViewModel.wareNumber,
                    SupplierName = orderViewModel.SupplierName,
                    ExternalId = externalId.bookingid + "-" + nextOrder.ToString("D2"),
                    Comment = orderViewModel.Comment,
                    
                    
                    
                });
            }

            

            if (orderViewModels != null)
            {
                model.OrderViewModels = orderViewModels;
            }

            nextOrder++;
            HttpContext.Session.SetObject(externalId.bookingid.ToString() , nextOrder);
      
            HttpContext.Session.SetObject(id , model);
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return new RedirectToPageResult("orderinformation");

        }

        
        public async Task<IActionResult> OnGetDeleteAsync(Guid orderId)
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
            var nextOrder = HttpContext.Session.GetObject<int>(model.ExternalId.ToString());
            nextOrder--;

            var result = model.OrderViewModels.FirstOrDefault(x => x.id.Equals(orderId));
            model.PalletsRemaining += result.BottomPallets;
            model.OrderViewModels.Remove(result);
            HttpContext.Session.SetObject(id , model);
            HttpContext.Session.SetObject(model.ExternalId.ToString() , nextOrder);
            return new RedirectToPageResult("orderinformation");
        }

        public void CreateSelectedList(SuppliersListViewModel suppliers) 
        {
            Transporters = new List<SelectListItem>();

            foreach (var supplier in suppliers.Suppliers)
            {
                Transporters.Add(new SelectListItem{ Value = supplier.Name ,Text = supplier.Name});
            }
        }

        public void CheckIfSupplierChoosenIsAllowed(Guid ui)
        {
            
        }
    }
}