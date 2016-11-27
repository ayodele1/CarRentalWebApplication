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

        
        public int ReservationId { get; set; }

        [ForeignKey("ReservationId")]
        public ICollection<Reservation> Reservations { get; set; }

        public int RentalId { get; set; }

        [ForeignKey("RentalId")]
        public ICollection<Rental> Rentals { get; set; }
    }
}
