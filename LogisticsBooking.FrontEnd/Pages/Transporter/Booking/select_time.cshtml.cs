using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class select_time : PageModel
    {
        private readonly IBookingDataService _bookingDataService;
        private readonly IScheduleDataService _scheduleDataService;
        
        [BindProperty]
        public Schedule schedule { get; set; }
        
        [BindProperty]
        public Interval interval { get; set; }

        public select_time(IBookingDataService bookingDataService , IScheduleDataService scheduleDataService)
        {
            _bookingDataService = bookingDataService;
            _scheduleDataService = scheduleDataService;
        }
        
        public void OnGet()
        {
            Console.WriteLine("");
            
        }

        public async Task<IActionResult> OnPostAsync()
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

            var CurrentBooking = HttpContext.Session.GetObject<BookingViewModel>(id);

            var result = await _scheduleDataService.GetScheduleBydate(CurrentBooking.BookingTime);

            schedule = result;
           
            return Page();
        }

        public async Task<IActionResult> OnPostSelectedTime(Interval interval , Schedule schedule)
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

            var CurrentBooking = HttpContext.Session.GetObject<BookingViewModel>(id);

            var scheduleChosen = await _scheduleDataService.GetScheduleById(schedule.ScheduleId);

            var intervalChosen = scheduleChosen.Intervals.FirstOrDefault(x => x.IntervalId == interval.IntervalId);


            var booking = new DataServices.Models.Booking
            {

                actualArrival = new DateTime(),
                bookingTime = CurrentBooking.BookingTime,
                email = CurrentBooking.email,
                endLoading = new DateTime(),
                ExternalId = CurrentBooking.ExternalId,
                startLoading = new DateTime(),
                totalPallets = CurrentBooking.TotalPallets,
                transporterName = CurrentBooking.TransporterName

            };
            
            List<Order> orders = new List<Order>();

            foreach (var order in CurrentBooking.OrderViewModels)
            {
                orders.Add(new Order
                {
                    bookingId = order.bookingId,
                    BottomPallets = order.BottomPallets,
                    Comment = order.Comment,
                    customerNumber = order.customerNumber,
                    ExternalId = order.ExternalId,
                    id = order.id,
                    InOut = order.InOut,
                    orderNumber = order.orderNumber,
                    SupplierName = order.SupplierName,
                    TotalPallets = order.totalPallets,
                    wareNumber = order.wareNumber
                });
            }

            booking.Orders = orders;

            
            _bookingDataService.CreateBooking(new CreateBooking
            {
                Booking = booking,
                IntervalId = interval.IntervalId,
                Schedule = schedule
            });
            
            
            return RedirectToPage("confirm");
        }
    }
}