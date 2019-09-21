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
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.Documents;
using LogisticsBooking.FrontEnd.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
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
        [BindProperty] public BookingsListViewModel BookingsListViewModel { get; set; } = new BookingsListViewModel();
        public bool InBetweenDates { get; set; }
        public int NumberOfDays { get; set; }
        
        public BookingOverviewModel(IBookingDataService _bookingDataService)
        {
            bookingDataService = _bookingDataService;
        }

        public async void OnGet()
        {
            string id = "7";
            var numberOfDays = 0;
            if(id != null) {
                numberOfDays = Int32.Parse(id);
            }
            NumberOfDays = numberOfDays;
            BookingsListViewModel.Bookings = new List<BookingViewModel>();

            if (numberOfDays != 0)
            { 
                BookingsListViewModel = bookingDataService.GetBookingsInbetweenDates(DateTime.Now,
                    DateTime.Now.AddDays(numberOfDays)).Result;
            }
            else
            {

                BookingsListViewModel = await bookingDataService.GetBookings();

                if (BookingsListViewModel != null)
                {
                    foreach (var booking in BookingsListViewModel.Bookings)
                    {
                        if (String.IsNullOrWhiteSpace(booking.TransporterName)) booking.TransporterName = "N/A";
                        if (String.IsNullOrWhiteSpace(booking.Email)) booking.Email = "N/A";

                        booking.ActualArrival = default(DateTime).Add(booking.ActualArrival.TimeOfDay);
                        booking.EndLoading = default(DateTime).Add(booking.EndLoading.TimeOfDay);
                        booking.StartLoading = default(DateTime).Add(booking.StartLoading.TimeOfDay);

                        foreach (var order in booking.Orders)
                        {
                            if (String.IsNullOrWhiteSpace(order.customerNumber)) order.customerNumber = "N/A";
                            if (String.IsNullOrWhiteSpace(order.orderNumber)) order.orderNumber = "N/A";
                            if (String.IsNullOrWhiteSpace(order.InOut)) order.InOut = "N/A";
                        }
                    }

                    for (int i = BookingsListViewModel.Bookings.Count - 1; i >= 0; i--)
                    {
                        if (BookingsListViewModel.Bookings[i].EndLoading.Date != default(DateTime))
                        {
                            BookingsListViewModel.Bookings.Remove(BookingsListViewModel.Bookings[i]);
                        }
                    }
                }


            }
        }

        public async Task<IActionResult> OnPostUpdate(DateTime actualArrival, DateTime startLoading, DateTime endLoading, string id)
        {
            var idConverted = Guid.Parse(id);
            var bookingToUpdate =  await bookingDataService.GetBookingById(idConverted);
            
            bookingToUpdate.ActualArrival = actualArrival;
            bookingToUpdate.StartLoading = startLoading;
            bookingToUpdate.EndLoading = endLoading;

            
            
            var response = await bookingDataService.UpdateBooking(CreateUpdateBookingCommand(bookingToUpdate));
            if (!response.IsSuccesfull) return new RedirectToPageResult("~Error");
            
            return new RedirectToPageResult("BookingOverview" );
        }

        private UpdateBookingCommand CreateUpdateBookingCommand(BookingViewModel bookingToUpdate)
        {
            return new UpdateBookingCommand
            {
                Email = bookingToUpdate.Email,
                Port = bookingToUpdate.Port,
                ActualArrival = bookingToUpdate.ActualArrival,
                BookingTime = bookingToUpdate.ActualArrival,
                EndLoading = bookingToUpdate.ActualArrival,
                ExternalId = bookingToUpdate.ExternalId,
                InternalId = bookingToUpdate.InternalId,
                StartLoading = bookingToUpdate.StartLoading,
                TotalPallets = bookingToUpdate.TotalPallets,
                TransporterId = bookingToUpdate.TransporterId,
                TransporterName = bookingToUpdate.TransporterName
            };
        }

        public async Task<ActionResult> OnPostExportExcel()
        {
            BookingsListViewModel.Bookings = new List<BookingViewModel>(); 
            BookingsListViewModel = await bookingDataService.GetBookings();

            var from = DateTime.Now;
            var endDate = DateTime.Now.Date.ToShortDateString();
            
            foreach (var booking in BookingsListViewModel.Bookings)
            {
                if (booking.BookingTime < from) from = booking.BookingTime;
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
            
            foreach (var booking in BookingsListViewModel.Bookings)
            {
                foreach (var order in booking.Orders)
                {
                    worksheet.Cell(cellY, cellX++).SetValue(order.customerNumber);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.BookingTime.ToShortDateString());
                    worksheet.Cell(cellY, cellX++).SetValue(order.orderNumber);
                    worksheet.Cell(cellY, cellX++).SetValue(order.TotalPallets);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.TransporterName);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.Email);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.BookingTime.ToShortTimeString());
                    worksheet.Cell(cellY, cellX++).SetValue(booking.Port);
                    worksheet.Cell(cellY, cellX++).SetValue(order.SupplierName);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.ActualArrival.ToShortTimeString());
                    worksheet.Cell(cellY, cellX++).SetValue(booking.StartLoading.ToShortTimeString());
                    worksheet.Cell(cellY, cellX++).SetValue(booking.EndLoading.ToShortTimeString());
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


        public async Task<IActionResult> OnGetTest()
        {

            var bookings = await bookingDataService.GetBookings();
            
            
            var json = new JsonResult(bookings);
           
            return json;

        }
        
        [ValidateAntiForgeryToken]
        [EnableCors("MyPolicy")]
        public async Task<IActionResult> OnPostTest(string end)
        {
            
            Console.WriteLine();
            return Page();
        }
    }
}
