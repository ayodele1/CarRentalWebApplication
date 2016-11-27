using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects
{
    interface IModificationHistory
    {
        DateTime DateModified { get; set; }
        
        DateTime DateCreated { get; set; }
    }
}
