using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using LogisticsBooking.FrontEnd.Pages.Client.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoreLinq;
using static MoreLinq.Extensions.LagExtension;

namespace LogisticsBooking.FrontEnd.Pages.Transporter
{
    public class ReservedBookingsIndexModel : PageModel
    {
        private readonly IIntervalDataService _intervalDataService;
        private IBookingDataService BookingDataService { get; set; }
        private ITransporterDataService TransporterDataService { get; set; }
        [BindProperty] public List<BookingViewModel> Bookings { get; set; }
        
        public List<IntervalViewModel> IntervalViewModels { get; set; }

        public ReservedBookingsIndexModel(IBookingDataService _bookingDataService, ITransporterDataService _transporterDataService , IIntervalDataService intervalDataService)
        {
            _intervalDataService = intervalDataService;
            BookingDataService = _bookingDataService;
            TransporterDataService = _transporterDataService;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            Bookings = new List<BookingViewModel>();
            
            var stringId = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            var guid = Guid.Parse(stringId);

            var loggedIn = await TransporterDataService.GetTransporterById(guid);
            
            var allBookings = await BookingDataService.GetBookings();
            var intervalViewModels = new List<IntervalViewModel>();
            foreach (var booking in allBookings.Bookings)
            {
                if (booking.TransporterId == loggedIn.TransporterId)
                {
                    Bookings.Add(booking);
                    intervalViewModels.Add(booking.Interval);
                }
            }

            IntervalViewModels = intervalViewModels.DistinctBy(x => x.IntervalId).ToList();
            return Page();
        }
    }
}