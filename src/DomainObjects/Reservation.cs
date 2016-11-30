using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    public class Reservation : IModificationHistory,IRentalInfo
    {
        public int Id { get; set; }

        public int ConfirmationNumber { get; set; }

        public int CustomerId { get; set; }

        //Foreign Key
        public Customer Customer { get; set; }

        public double TotalCost { get; set; }

        public string StoreLocation { get; set; }

        //Foreign Key
        public Vehicle Vehicle { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime PickupTime { get; set; }

        public DateTime ReturnDate { get; set; }

        public DateTime ReturnTime { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
