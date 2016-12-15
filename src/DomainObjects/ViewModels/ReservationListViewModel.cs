using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    public class ReservationListViewModel
    {
        public IEnumerable<Reservation> Reservations { get; set; }

        public string SignedInUserId { get; set; }
    }
}
