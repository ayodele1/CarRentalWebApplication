using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    public class Rental : IRentalInfo,IModificationHistory
    {
        public int Id { get; set; }

        public int isActive { get; set; }

        [NotMapped]
        public bool IsActive
        {
            get { return isActive != 0; }
            set { isActive = value ? 1 : 0; }
        }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        //Foreign Key
        public Customer Customer { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        //Foreign Key
        public Vehicle Vehicle { get; set; }

        public double TotalCost { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime PickupTime { get; set; }

        public DateTime ReturnDate { get; set; }

        public DateTime ReturnTime { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

    }
}
