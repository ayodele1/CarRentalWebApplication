using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    interface IRentalInfo
    {
        int Id { get; set; }

        int CustomerId { get; set; }

        //Foreign Key
        Customer Customer { get; set; }

        double TotalCost { get; set; }

        //Foreign Key
        Vehicle Vehicle { get; set; }

        DateTime PickupDate { get; set; }

        DateTime PickupTime { get; set; }

        DateTime ReturnDate { get; set; }

        DateTime ReturnTime { get; set; }
    }
}
