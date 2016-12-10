using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    public class Reservation : IModificationHistory
    {
        public int Id { get; set; }

        public long ConfirmationNumber { get; set; }

        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }

        public double TotalCost { get; set; }

        public string StoreLocation { get; set; }
        
        public int VehicleId { get; set; }

        //Foreign Key
        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime PickupTime { get; set; }

        public DateTime ReturnDate { get; set; }

        public DateTime ReturnTime { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
