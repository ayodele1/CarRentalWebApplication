using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ModelServices
{
    interface IChangeTracker
    {
        bool IsDirty { get; set; }
    }
}
