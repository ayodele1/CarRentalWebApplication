using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainObjects.ModelServices
{
    public class FormSubmissionService : IFormHyperLinkService
    {
        public string FormAction
        {
            get;set;
        }

        public string FormController
        {
            get;set;
        }
    }
}
