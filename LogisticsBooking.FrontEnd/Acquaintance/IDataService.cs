using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.Acquaintance
{
    public interface IBookingDataService
    {
        Task<Response> CreateBooking(Booking _booking);
    }

    public interface ITransporterDataService
    {
        Task<Response> CreateTransporter(Transporter _transporter);
    }

    public interface ISupplierDataService
    {
        Task<Response> CreateSupplier(Supplier _supplier);
    }
}
