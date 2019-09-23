using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
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

        public bool IsSupplierAllowed = true;
        public int palletsRemaining { get; set; }
        
        [BindProperty]
        public Guid id { get; set; }

       

        public orderinformation(ISupplierDataService supplierDataService , IUtilBookingDataService utilBookingDataService)
        {
            _supplierDataService = supplierDataService;
            _utilBookingDataService = utilBookingDataService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            
            BookingViewModel = HttpContext.Session.GetObject<BookingViewModel>(GetLoggedInUserId());
            
            var result = await _supplierDataService.ListSuppliers(2, 4);
            BookingViewModel.SuppliersListViewModel = result;
            HttpContext.Session.SetObject(GetLoggedInUserId() , BookingViewModel);
            CreateSelectedList(BookingViewModel.SuppliersListViewModel);

            return Page();

        }

        public async Task<IActionResult> OnPostAsync(OrderViewModel orderViewModel)
        {

            /*
             * 1 - Get ID
             * 2 - Get Booking from context
             * 3 - Add order to booking
             * 4 - Add booking to context
             * 5 - Save to context
             */

            // Getting current Booking from HTTPContext
            var currentBookingViewModel = GetBookingFromContext(GetLoggedInUserId());



            // DO Validation before booking creation ---- 

            if (currentBookingViewModel.OrderViewModels != null)
            {
                if (await CheckIfSupplierTimeOverlap(currentBookingViewModel, orderViewModel))
                {
                    await AddOrderToBookingViewModel(orderViewModel, currentBookingViewModel);
                    
                }
                else
                {
                    currentBookingViewModel.IsBookingAllowed = false;
                    
                    HttpContext.Session.SetObject(GetLoggedInUserId() , currentBookingViewModel);
                }
            }
            else
            {
                await AddOrderToBookingViewModel(orderViewModel, currentBookingViewModel);
            }
            

            return new RedirectToPageResult("orderinformation");

        }

        
        public async Task<IActionResult> OnGetDeleteAsync(Guid orderId)
        {
            var currentBookingViewModel = GetBookingFromContext(GetLoggedInUserId());
            
            
            
            var nextOrder = HttpContext.Session.GetObject<int>(currentBookingViewModel.ExternalId.ToString());
            nextOrder--;

            var result = currentBookingViewModel.OrderViewModels.FirstOrDefault(x => x.OrderId.Equals(orderId));
            currentBookingViewModel.PalletsRemaining += result.BottomPallets;
            currentBookingViewModel.PalletsCurrentlyOnBooking -= result.BottomPallets;
            currentBookingViewModel.OrderViewModels.Remove(result);
            HttpContext.Session.SetObject(GetLoggedInUserId() , currentBookingViewModel);
            HttpContext.Session.SetObject(currentBookingViewModel.ExternalId.ToString() , nextOrder);
            return new RedirectToPageResult("orderinformation");
        }

        public void CreateSelectedList(SuppliersListViewModel suppliers) 
        {
            Transporters = new List<SelectListItem>();

            foreach (var supplier in suppliers.Suppliers)
            {
                Transporters.Add(new SelectListItem{ Value = supplier.SupplierId.ToString() ,Text = supplier.Name});
            }
        }

        public async Task<bool> CheckIfSupplierTimeOverlap(BookingViewModel bookingViewModel , OrderViewModel orderViewModel)
        {
            List<SupplierViewModel> orderViewModelsInCurrentBooking = new List<SupplierViewModel>();
            
            foreach (var order in bookingViewModel.OrderViewModels)
            {
                orderViewModelsInCurrentBooking.Add(await _supplierDataService.GetSupplierById(order.SupplierId));
            }
            
            var SupplierTryingToBook = await _supplierDataService.GetSupplierById(orderViewModel.SupplierId);

            orderViewModelsInCurrentBooking.OrderBy(x => x.DeliveryStart);

            bool overlap = true;
            foreach (var supplier in orderViewModelsInCurrentBooking)
            {
                overlap = supplier.DeliveryStart.Hour < SupplierTryingToBook.DeliveryEnd.Hour && SupplierTryingToBook.DeliveryStart.Hour < supplier.DeliveryEnd.Hour;

                if (overlap == false)
                {
                    break;
                }
            }

            return overlap;
            
            
            
        }

        public string GetLoggedInUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            
        }

        public BookingViewModel GetBookingFromContext(string id)
        {
            
            return HttpContext.Session.GetObject<BookingViewModel>(id);
            
            
        }

        public async Task AddOrderToBookingViewModel(OrderViewModel orderViewModel , BookingViewModel bookingViewModel) 
        {
            // Getting Current Order number based on the Booking number from the HttpContext

            
            var nextOrder = HttpContext.Session.GetObject<int>(bookingViewModel.ExternalId.ToString());

            var suppliername = bookingViewModel.SuppliersListViewModel.Suppliers.FirstOrDefault(x => x.SupplierId.Equals(orderViewModel.SupplierId));
            
            List<OrderViewModel> orderViewModels = null;
            bookingViewModel.PalletsRemaining -= orderViewModel.BottomPallets;
            bookingViewModel.PalletsCurrentlyOnBooking += orderViewModel.BottomPallets;
            if (bookingViewModel.OrderViewModels == null)
            {
                orderViewModels = new List<OrderViewModel>
                {
                    new OrderViewModel 
                    {
                        orderNumber = orderViewModel.orderNumber,
                        BottomPallets = orderViewModel.BottomPallets,
                        customerNumber = orderViewModel.customerNumber,
                        InOut = orderViewModel.InOut,
                        totalPallets = orderViewModel.totalPallets,
                        wareNumber = orderViewModel.wareNumber,
                        SupplierName = suppliername.Name,
                        SupplierId = suppliername.SupplierId,
                        ExternalId = bookingViewModel.ExternalId + "-" + nextOrder.ToString("D2"),
                        createdOrders = 2,
                        Comment = orderViewModel.Comment,
                        OrderId = Guid.NewGuid()
                        
                        
                    }    
                };
            }
            else
            {
                bookingViewModel.OrderViewModels.Add(new OrderViewModel
                {
                    orderNumber = orderViewModel.orderNumber,
                    BottomPallets = orderViewModel.BottomPallets,
                    customerNumber = orderViewModel.customerNumber,
                    InOut = orderViewModel.InOut,
                    totalPallets = orderViewModel.totalPallets,
                    wareNumber = orderViewModel.wareNumber,
                    SupplierName = suppliername.Name,
                    SupplierId = suppliername.SupplierId,
                    ExternalId = bookingViewModel.ExternalId + "-" + nextOrder.ToString("D2"),
                    createdOrders = 2,
                    Comment = orderViewModel.Comment
                    
                    
                    
                });
            }

            

            if (orderViewModels != null)
            {
                bookingViewModel.OrderViewModels = orderViewModels;
            }

            bookingViewModel.IsBookingAllowed = true;
            nextOrder++;
            HttpContext.Session.SetObject(bookingViewModel.ExternalId.ToString() , nextOrder);
      
            HttpContext.Session.SetObject(GetLoggedInUserId() , bookingViewModel);

           
        }

        public async Task<IActionResult> OnPostEditOrder(OrderViewModel orderViewModel)
        {
            var currentBookingViewModel = GetBookingFromContext(GetLoggedInUserId());

            var order = currentBookingViewModel.OrderViewModels.Find(x => x.OrderId.Equals(orderViewModel.OrderId));

            
            currentBookingViewModel.PalletsCurrentlyOnBooking -= order.BottomPallets;
            currentBookingViewModel.PalletsRemaining += order.BottomPallets;

            order.Comment = OrderViewModel.Comment;
            order.BottomPallets = OrderViewModel.BottomPallets;
            order.orderNumber = OrderViewModel.orderNumber;
            order.BottomPallets = OrderViewModel.BottomPallets;
            
            
            currentBookingViewModel.PalletsCurrentlyOnBooking += order.BottomPallets;
            currentBookingViewModel.PalletsRemaining -= order.BottomPallets;
            
            HttpContext.Session.SetObject(GetLoggedInUserId() , currentBookingViewModel);
            
            return new RedirectToPageResult("");
  
        }
        
    }
}