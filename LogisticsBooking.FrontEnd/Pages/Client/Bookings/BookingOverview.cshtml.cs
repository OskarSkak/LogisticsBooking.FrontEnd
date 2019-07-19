using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using LogisticsBooking.FrontEnd.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2.HPack;

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class BookingOverviewModel : PageModel
    {
        [BindProperty] public int WEEK { get; } = 7;
        [BindProperty] public int MONTH { get; } = 31;
        [BindProperty] public string NameOfFile { get; set; }
        
        private IBookingDataService bookingDataService;
        [BindProperty] public List<Booking> Bookings { get; set; }
        public bool InBetweenDates { get; set; }
        public int NumberOfDays { get; set; }
        
        public BookingOverviewModel(IBookingDataService _bookingDataService)
        {
            bookingDataService = _bookingDataService;
        }

        public async void OnGet(string id)
        {
            var numberOfDays = 0;
            if(id != null) {
                numberOfDays = Int32.Parse(id);
            }
            NumberOfDays = numberOfDays;
            Bookings = new List<Booking>();

            if (numberOfDays != 0)
            { 
                Bookings = bookingDataService.GetBookingsInbetweenDates(DateTime.Now.AddDays(- NumberOfDays),
                    DateTime.Now).Result;
            }
            else{Bookings = bookingDataService.GetBookings().Result;}
    
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

            for (int i = Bookings.Count - 1; i >= 0; i--)
            {
                if (Bookings[i].endLoading != default(DateTime))
                {
                    Bookings.Remove(Bookings[i]);
                }
            }
            
        }

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

        public ActionResult OnPostExportExcel()
        {
            Bookings = new List<Booking>(); 
            Bookings = bookingDataService.GetBookings().Result;

            var from = DateTime.Now;
            var endDate = DateTime.Now.Date.ToShortDateString();
            
            foreach (var booking in Bookings)
            {
                if (booking.bookingTime < from) from = booking.bookingTime;
            }

            var fromDateString = from.ToShortDateString();
            
            System.IO.Stream spreadsheetStream = new System.IO.MemoryStream();
            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Orders");

            worksheet.Cell(1, 1).SetValue("KUNNR");
            worksheet.Cell(1, 2).SetValue("DATO");
            worksheet.Cell(1, 3).SetValue("ORDNR");
            worksheet.Cell(1, 4).SetValue("PALLER");
            worksheet.Cell(1, 5).SetValue("TRANSPORTØR");
            worksheet.Cell(1, 6).SetValue("KONTAKTOPLYSNINGER");
            worksheet.Cell(1, 7).SetValue("BOOKETTID");
            worksheet.Cell(1, 8).SetValue("PORT");
            worksheet.Cell(1, 9).SetValue("LEVERANDØR");
            worksheet.Cell(1, 10).SetValue("FAKTISK_\nANKOMST");
            worksheet.Cell(1, 11).SetValue("START_\nLÆSNING");
            worksheet.Cell(1, 12).SetValue("SLUT_\nLÆSNING");

            var cellX = 1;
            var cellY = 2;
            
            foreach (var booking in Bookings)
            {
                foreach (var order in booking.Orders)
                {
                    worksheet.Cell(cellY, cellX++).SetValue(order.customerNumber);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.bookingTime.ToShortDateString());
                    worksheet.Cell(cellY, cellX++).SetValue(order.orderNumber);
                    worksheet.Cell(cellY, cellX++).SetValue(order.TotalPallets);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.transporterName);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.email);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.bookingTime.ToShortTimeString());
                    worksheet.Cell(cellY, cellX++).SetValue(booking.port);
                    worksheet.Cell(cellY, cellX++).SetValue(order.SupplierName);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.actualArrival.ToShortTimeString());
                    worksheet.Cell(cellY, cellX++).SetValue(booking.startLoading.ToShortTimeString());
                    worksheet.Cell(cellY, cellX++).SetValue(booking.endLoading.ToShortTimeString());
                    cellY++;
                    cellX = 1;
                }
            }
            //Fixed column width - comes out messy if not fixed
            worksheet.ColumnWidth = 20;
            workbook.SaveAs(spreadsheetStream);
            spreadsheetStream.Position = 0;
            var fileName = "Ordre_FRA_" + fromDateString + "_TIL_" + endDate;
            
            return new FileStreamResult(spreadsheetStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = fileName + ".xlsx" };
        }
    }
}
