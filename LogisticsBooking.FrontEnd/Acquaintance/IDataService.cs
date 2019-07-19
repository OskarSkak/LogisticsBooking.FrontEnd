using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;

namespace LogisticsBooking.FrontEnd.Acquaintance
{
    public interface IBookingDataService
    {
        Task<Response> CreateBooking(CreateBooking booking);
        Task<List<Booking>> GetBookings();
        Task<Booking> GetBookingById(Guid id);
        Task<Response> UpdateBooking(Booking booking);
        Task<Response> DeleteBooking(Guid id);
        Task<List<Booking>> GetBookingsInbetweenDates(DateTime from, DateTime to);
    }

    public interface IScheduleDataService
    {
        Task<Response> CreateSchedule(Schedule schedule);
        Task<List<Schedule>> GetSchedules();
        Task<Schedule> GetScheduleById(Guid id);
        Task<Response> UpdateSchedule(Schedule schedule);
        Task<Response> DeleteSchedule(Guid id);
        Task<Response> CreateManySchedule(ManySchedules schedule);
        Task<Schedule> GetScheduleBydate(DateTime date);
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
        Task<Response> CreateTransporter(Transporter _transporter);
        Task<Response> UpdateTransporter(Guid id, Transporter transporter);
        Task<IEnumerable<Transporter>> ListTransporters(int page, int pageSize);
        Task<Transporter> GetTransporterById(Guid id);
        Task<Response> DeleteTransporter(Guid id);
        Task<Transporter> GetTransporterByName(string name);
    }

    public interface ISupplierDataService
    {
        Task<Response> CreateSupplier(Supplier _supplier);
        Task<Response> UpdateSupplier(Guid id, Supplier supplier);
        Task<Response> DeleteSupplier(Guid id);
        Task<Supplier> GetSupplierById(Guid id);
        Task<IEnumerable<Supplier>> ListSuppliers(int page, int pageSize);
        Task<Supplier> GetSupplierByName(string name);
    }

    public interface IUtilBookingDataService
    {
        Task<UtilBooking> GetBookingNumber();
    }
}
