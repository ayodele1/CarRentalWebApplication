using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    [ModelMetadataType(typeof(RegistrationViewModel.RegisterationValidation))]
    public class ReservationContactViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get { return Email; } }

        public string PhoneNumber { get; set; }

        public long ConfirmationNumber { get; set; }

        public double TotalCost { get; set; }

        public DateTime DateCreated { get { return DateTime.Now; } }

        public DateTime DateModified { get { return DateTime.Now; } }

        public bool HidePasswordField { get; set; }

    }
}
