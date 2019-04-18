using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class Transporter
    {
        public string Email { get; set; }
        public int Telephone { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }

        public Transporter() { }

        public Transporter(string email, int telephone, string address, string name, Guid iD)
        {
            Email = email;
            Telephone = telephone;
            Address = address;
            Name = name;
            ID = iD;
        }
    }
}
