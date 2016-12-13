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
    public class ReservationLocationViewModel : IFormHyperLinkService,IChangeTracker
    {
        private string _controllerName = string.Empty;
        private string _actionName = string.Empty;
        private bool _isDirty = false;
        private string _userLocation = string.Empty;
        private string _storeLocation = string.Empty;
        private DateTime _pickupDate = DateTime.Now;
        private DateTime _returnDate = DateTime.Now;

        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

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

        public string UserLocation
        {
            get { return _userLocation; }
            set
            {
                _userLocation = value;
                if (string.Compare(value, _userLocation) != 0)
                    _isDirty = true;
            }
        }
        public string StoreLocation
        {
            get { return _storeLocation; }
            set
            {
                _storeLocation = value;
                if (string.Compare(value, _storeLocation) != 0)
                    _isDirty = true;
            }
        }

        public DateTime PickupDate
        {
            get { return _pickupDate; }
            set
            {
                _pickupDate = value;
                if (DateTime.Compare(value, _pickupDate) != 0)
                    _isDirty = true;
            }
        }

        public DateTime ReturnDate
        {
            get { return _returnDate; }
            set
            {
                _returnDate = value;
                if (DateTime.Compare(value, _pickupDate) != 0)
                    _isDirty = true;
            }
        }

        public List<string> StoreLocations { get; set; }

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
