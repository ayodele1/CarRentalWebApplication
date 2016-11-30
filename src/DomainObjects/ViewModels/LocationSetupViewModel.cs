using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    public class LocationSetupViewModel
    {
        public string StoreLocation { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }
    }
}
