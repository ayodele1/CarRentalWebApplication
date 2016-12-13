using DomainObjects.ModelServices;
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

        public QuickSignUpViewModel QuickSignUp { get; set; }

        public double StatesTax { get { return 22.67; } set { } }

        public string CurrentUserId { get; set; }

        public long ConfirmationNumber { get; set; }

        public bool IsDirty()
        {
            if (InitialSetup.IsDirty || VehicleSetup.IsDirty || ReviewAndContactSetup.IsDirty)
            {
                return true;
            }
            return false;
        }
    }
}
