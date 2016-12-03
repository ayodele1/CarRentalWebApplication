using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    public class Customer : User
    {
        public Customer(string firstname, string lastname, string emailaddress) 
            : base(firstname, lastname, emailaddress)
            
        {
            this.Reservations = new HashSet<Reservation>();
            this.Rentals = new HashSet<Rental>();
        }

        public string PhoneNumber { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
