using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    public abstract class User : IModificationHistory
    {
        public User(string firstname, string lastname, string emailaddress)
        {
            FirstName = firstname;
            LastName = lastname;
            EmailAddress = emailaddress;
        }


        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
