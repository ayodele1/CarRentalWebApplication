using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    public class Rental : IModificationHistory
    {
        public int Id { get; set; }

        public int isActive { get; set; }

        [NotMapped]
        public bool IsActive
        {
            get { return isActive != 0; }
            set { isActive = value ? 1 : 0; }
        }
        
        public int VehicleId { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        //Foreign Key

        [ForeignKey("VehicleId")]
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
