using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    public class ReservationViewModel
    {
        public ReservationLocationViewModel InitialSetup { get; set; }

        public ReservationVehicleViewModel VehicleSetup { get; set; }

        public ReservationContactViewModel ReviewAndContactSetup { get; set; }

        public double StatesTax { get { return 22.67; } set { } }
    }
}
