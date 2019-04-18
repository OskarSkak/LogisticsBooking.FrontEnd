using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.DataServices.RequestModels
{
    public class TransporterUpdateModel
    {
        public string Email { get; set; }
        public int Telephone { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }

        public TransporterUpdateModel() { }

        public TransporterUpdateModel(string email, int telephone, string address, string name)
        {
            Email = email;
            Telephone = telephone;
            Address = address;
            Name = name;
        }
    }
}
