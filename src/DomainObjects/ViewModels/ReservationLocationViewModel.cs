using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DomainObjects.ModelServices;

namespace DomainObjects.ViewModels
{
    [ModelMetadataType(typeof(ReservationLocationValidation))]
    public class ReservationLocationViewModel : IFormHyperLinkService
    {
        private string _controllerName = string.Empty;
        private string _actionName = string.Empty;

        public string FormController
        {
            get { return _controllerName; }
            set { _controllerName = value; }
        }

        public string FormAction
        {
            get { return _actionName; }

            set { _actionName = value; }
        }
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
