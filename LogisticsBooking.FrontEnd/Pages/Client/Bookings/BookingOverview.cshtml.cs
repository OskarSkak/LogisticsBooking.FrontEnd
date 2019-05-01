using System;
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
    public class BookingOverviewModel : PageModel
    {
        private IBookingDataService bookingDataService;
        [BindProperty] public List<Booking> Bookings { get; set; }
        
        public BookingOverviewModel(IBookingDataService _bookingDataService)
        {
            bookingDataService = _bookingDataService;
            Bookings = new List<Booking>(); 
            Bookings = bookingDataService.GetBookings().Result;
            foreach (var booking in Bookings)
            {
                if (String.IsNullOrWhiteSpace(booking.transporterName)) booking.transporterName = "N/A";
                if (String.IsNullOrWhiteSpace(booking.email)) booking.email = "N/A";
                
                booking.actualArrival = default(DateTime).Add(booking.actualArrival.TimeOfDay);
                booking.endLoading = default(DateTime).Add(booking.endLoading.TimeOfDay);
                booking.startLoading = default(DateTime).Add(booking.startLoading.TimeOfDay);
                
                foreach (var order in booking.Orders)
                {
                    if (String.IsNullOrWhiteSpace(order.customerNumber)) order.customerNumber = "N/A";
                    if (String.IsNullOrWhiteSpace(order.orderNumber)) order.orderNumber = "N/A";
                    if (String.IsNullOrWhiteSpace(order.InOut)) order.InOut = "N/A";
                }
            }
        }
        
        public void OnGet(){}

        public async Task<IActionResult> OnPostUpdate(DateTime actualArrival, DateTime startLoading, DateTime endLoading, string id)
        {
            var idConverted = Guid.Parse(id);

            var bookingToUpdate =  await bookingDataService.GetBookingById(idConverted);
            bookingToUpdate.actualArrival = actualArrival;
            bookingToUpdate.startLoading = startLoading;
            bookingToUpdate.endLoading = endLoading;

            var response = await bookingDataService.UpdateBooking(bookingToUpdate);

            if (!response.IsSuccesfull) return new RedirectToPageResult("~Error");
            
            return new RedirectToPageResult("./BookingOverview");
        }
        

       
        
    }
/*
    public class ExcelUtil
    {
        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                        prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }

        public void ExportListFromTsv(List<Booking> bookings)
        {
            var excelOrders = new List<ExcelOrder>();
            foreach (var booking in bookings)
            {
                foreach (var order in booking.Orders)
                {
                    var excelOrder = new ExcelOrder(order, booking.bookingTime, 
                        booking.totalPallets, booking.transporterName, booking.email, 
                        booking.port, booking.actualArrival, booking.startLoading, 
                        booking.endLoading);
                    excelOrders.Add(excelOrder);
                }
            }

            ExcelOrder[] data = new ExcelOrder[excelOrders.Count];
            
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = excelOrders[i];
            }

            HttpContext.Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Contact.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteTsv(data, Response.Output);
            Response.End();
        }
        
        
    }
    
    class ExcelOrder
    {
        public string customerNumber { get; set; }
        public string orderNumber { get; set; }
        public int wareNumber { get; set; }
        public string InOut { get; set; }
        public DateTime bookingTime { get; set; }
        public int Pallets { get; set; }
        public string TransporterName { get; set; }
        public string email { get; set; }
        public int port { get; set; }
        public DateTime actual_arrival { get; set; }
        public DateTime start_loading { get; set; }
        public DateTime end_loading { get; set; }
        
        public ExcelOrder(Order order, DateTime bookingTime, int pallets, 
            string transporterName, string email, int port, 
            DateTime actual_arrival, DateTime start_loading,
            DateTime end_loading)
        {
            this.customerNumber = order.customerNumber;
            this.orderNumber = order.orderNumber;
            this.wareNumber = order.wareNumber;
            this.InOut = order.InOut;
            this.bookingTime = bookingTime;
            this.Pallets = pallets;
            this.TransporterName = transporterName;
            this.email = email;
            this.port = port;
            this.actual_arrival = actual_arrival;
            this.start_loading = start_loading;
            this.end_loading = end_loading;
        }
    }*/
}
