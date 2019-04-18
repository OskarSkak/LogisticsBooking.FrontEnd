using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class Supplier
    {
        public string Email { get; set; }
        public int Telephone { get; set; }
        public string Name { get; set; }
        public Guid ID { get; set; }

        public Supplier() { }

        public Supplier(string email, int telephone,  string name, Guid iD)
        {
            Email = email;
            Telephone = telephone;
            Name = name;
            ID = iD;
        }
    }
}
