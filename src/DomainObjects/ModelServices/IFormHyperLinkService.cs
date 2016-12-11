using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ModelServices
{
    /// <summary>
    /// Implement this Interface if the View component has a Form
    /// </summary>
    interface IFormHyperLinkService
    {
        string FormController { get; set; }

        string FormAction { get; set; }
    }
}
