using DomainObjects.ModelServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ViewModels
{
    [ModelMetadataType(typeof(RegistrationViewModel.RegisterationValidation))]
    public class ReservationContactViewModel : IFormHyperLinkService,IChangeTracker
    {
        private bool _isDirty = false;
        private string _lastName = string.Empty;
        private string _firstName = string.Empty;
        private string _userName = string.Empty;
        private string _email = string.Empty;
        private string _phoneNumber = string.Empty;
        private long _confirmationNumber = 0;
        private double _totalCost = 0;

        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                if (string.Compare(_firstName, value, false) != 0)
                    _isDirty = true;
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                if (string.Compare(_lastName, value, false) != 0)
                    _isDirty = true;
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                if (string.Compare(_email, value, false) != 0)
                    _isDirty = true;
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                if (string.Compare(_userName, value, false) != 0)
                    _isDirty = true;
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                if (string.Compare(_phoneNumber, value, false) != 0)
                    _isDirty = true;
            }
        }

        public long ConfirmationNumber
        {
            get { return _confirmationNumber; }
            set
            {
                _confirmationNumber = value;
                if (value != _confirmationNumber)
                    _isDirty = true;
            }
        }

        public double TotalCost
        {
            get { return _totalCost; }
            set
            {
                _totalCost = value;
                if (value != TotalCost)
                    _isDirty = true;
            }
        }

        public string FormController { get; set; }

        public string FormAction { get; set; }
    }
}
