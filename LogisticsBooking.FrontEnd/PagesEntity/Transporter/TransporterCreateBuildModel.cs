using System;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;

namespace LogisticsBooking.FrontEnd.PagesEntity.Transporter
{
    public class TransporterCreateBuildModel : IHaveCustomMapping
    {

        public String Name { get; set; }
        
        public string Email { get; set; }
        
        public int Telephone { get; set; }
        
        public string Address { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<TransporterCreateBuildModel, TransporterViewModel>();
            configuration.CreateMap<TransporterViewModel, TransporterCreateBuildModel>();
        }
    }
}