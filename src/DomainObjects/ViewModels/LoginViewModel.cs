using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    [ModelMetadataType(typeof(LoginValidation))]
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        class LoginValidation
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
