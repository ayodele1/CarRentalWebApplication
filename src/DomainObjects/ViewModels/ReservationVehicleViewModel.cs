using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    public class ReservationVehicleViewModel
    {
        public ReservationVehicleViewModel()
        {

        }
        public ReservationVehicleViewModel(IEnumerable<Vehicle> availableVehicles)
        {
            Vehicles = availableVehicles;
        }
        public int VehicleId { get; set; }

        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
