using DomainObjects.ModelServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    public class ReservationVehicleViewModel : IFormHyperLinkService,IChangeTracker
    {
        private bool _isDirty = false;
        private Vehicle _reservationVehicle = null;
        public ReservationVehicleViewModel()
        {

        }
        public ReservationVehicleViewModel(IEnumerable<Vehicle> availableVehicles)
        {
            Vehicles = availableVehicles;
        }
        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        public string FormAction { get; set; }

        public string FormController { get; set; }

        public Vehicle ReservationVehicle
        {
            get { return _reservationVehicle; }
            set
            {
                _reservationVehicle = value;
                if (value.Id != _reservationVehicle.Id)
                    _isDirty = true;
            }
        }

        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
