﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    public class SearchReservationViewModel
    {
        [Required(ErrorMessage = "Please Enter a Confirmation Number")]
        [DataType(DataType.Text)]
        public long ConfirmationNumber { get; set; }

        [Required(ErrorMessage = "Email is required to search for the Reservation")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
