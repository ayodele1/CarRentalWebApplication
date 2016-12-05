using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DomainObjects.ViewModels
{
    [ModelMetadataType(typeof(ReservationLocationValidation))]
    public class ReservationLocationViewModel
    {
        public string StoreLocation { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }

        class ReservationLocationValidation
        {
            [Required]
            public string StoreLocation { get; set; }

            [Required]
            [DataType(DataType.DateTime)]
            public DateTime PickupDate { get; set; }

            [Required]
            [DataType(DataType.DateTime)]
            public DateTime ReturnDate { get; set; }
        }
    }
}
