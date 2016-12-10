using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    public class QuickSignUpViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
