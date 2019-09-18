using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.TransportersList;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;

namespace LogisticsBooking.FrontEnd.Acquaintance
{
    public interface IBookingDataService
    {
        Task<Response> CreateBooking(CreateBookingCommand booking);
        Task<BookingsListViewModel> GetBookings();
        Task<BookingViewModel> GetBookingById(Guid id);
        Task<Response> UpdateBooking(UpdateBookingCommand booking);
        Task<Response> DeleteBooking(Guid id);
        Task<BookingsListViewModel> GetBookingsInbetweenDates(DateTime from, DateTime to);
    }

    public interface IScheduleDataService
    {
        Task<Response> CreateSchedule(ScheduleViewModel schedule);
        Task<SchedulesListViewModel> GetSchedules();
        Task<ScheduleViewModel> GetScheduleById(Guid id);
        Task<Response> UpdateSchedule(ScheduleViewModel schedule);
        Task<Response> DeleteSchedule(Guid id);
        Task<Response> CreateManySchedule(CreateManyScheduleCommand schedule);
        Task<ScheduleViewModel> GetScheduleBydate(DateTime date);
    }

    public interface IOrderDataService
    {
        Task<Response> CreateOrder(Order order);
        Task<List<Order>> GetOrders();
        Task<Order> GetOrderById(Guid id);
        Task<Response> UpdateOrder(Order order);
        Task<Response> DeleteOrder(Guid id);
    }

    public interface ITransporterDataService
    {
        Task<Response> CreateTransporter(TransporterViewModel _transporter);
        Task<Response> UpdateTransporter(Guid id, TransporterViewModel transporter);
        Task<TransportersListViewModel> ListTransporters(int page, int pageSize);
        Task<TransporterViewModel> GetTransporterById(Guid id);
        Task<Response> DeleteTransporter(Guid id);
        Task<TransporterViewModel> GetTransporterByName(string name);
    }

    public interface ISupplierDataService
    {
        Task<Response> CreateSupplier(CreateSupplierViewModel _supplier);
        Task<Response> UpdateSupplier(Guid id, SupplierViewModel supplier);
        Task<Response> DeleteSupplier(Guid id);
        Task<SupplierViewModel> GetSupplierById(Guid id);
        Task<SuppliersListViewModel> ListSuppliers(int page, int pageSize);
        Task<SupplierViewModel> GetSupplierByName(string name);
    }

    public interface IUtilBookingDataService
    {
        Task<UtilBooking> GetBookingNumber();
    }
}
