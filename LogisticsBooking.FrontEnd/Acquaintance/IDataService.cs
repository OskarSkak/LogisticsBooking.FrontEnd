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
        Task<Response> UpdateTransporter(Guid id, Transporter transporter);
        Task<IEnumerable<Transporter>> ListTransporters(int page, int pageSize);
    }

    public interface ISupplierDataService
    {
        Task<Response> CreateSupplier(Supplier _supplier);
        Task<Response> UpdateSupplier(Guid id, Supplier supplier);
        Task<Response> DeleteSupplier(Guid id);
        Task<Supplier> GetSupplierById(Guid id);
        Task<IEnumerable<Supplier>> ListSuppliers(int page, int pageSize);
    }
}
