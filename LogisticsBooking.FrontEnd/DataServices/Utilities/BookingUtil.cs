using System;
using LogisticsBooking.FrontEnd.DataServices.Models;

namespace LogisticsBooking.FrontEnd.DataServices.Utilities
{
    public class BookingUtil
    {
        public static Booking RemoveDates(Booking booking)
        {
            booking.actualArrival = default(DateTime).Add(booking.actualArrival.TimeOfDay);
            booking.endLoading = default(DateTime).Add(booking.endLoading.TimeOfDay);
            booking.startLoading = default(DateTime).Add(booking.startLoading.TimeOfDay);

            return booking;
        }
    }
}