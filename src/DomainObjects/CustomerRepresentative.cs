using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    public class CustomerRepresentative : User
    {
        public CustomerRepresentative(string firstname, string lastname, string emailaddress)
            :base(firstname, lastname,emailaddress)
        {

        }

        //Add other properties for internal use.
    }
}
