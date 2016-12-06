using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    public class Customer : ApplicationUser
    {
        public Customer()
            :base()
        {
        }
    }
}
