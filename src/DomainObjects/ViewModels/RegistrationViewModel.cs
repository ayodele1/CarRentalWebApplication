using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    [ModelMetadataType(typeof(RegisterationValidation))]
    public class RegistrationViewModel
    {
        public string FirstName { get; set; }

        public string  LastName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public bool HidePasswordField { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string submitButtonValue { get; set; }

        class RegisterationValidation
        {
            [Required(ErrorMessage = "FirstName is Required")]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required (ErrorMessage = "Email Address is Required")]
            [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter a Valid Email Address")]
            public string EmailAddress { get; set; }

            [Required(ErrorMessage = "Phone Number is required")]
            [DataType(DataType.PhoneNumber)]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "Password Must be at least 6 characters")]
            public string Password { get; set; }
        }
    }
}
