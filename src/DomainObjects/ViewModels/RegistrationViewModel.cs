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

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string UserName { get { return Email; } }

        public DateTime DateCreated { get { return DateTime.Now; } }

        public DateTime DateModified { get { return DateTime.Now; } }

        public class RegisterationValidation
        {
            [Required(ErrorMessage = "FirstName is Required")]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required (ErrorMessage = "Email Address is Required")]
            [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter a Valid Email Address")]
            public string Email { get; set; }

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
