using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    public class ApplicationUser : IdentityUser,IModificationHistory
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; }

    }
}
