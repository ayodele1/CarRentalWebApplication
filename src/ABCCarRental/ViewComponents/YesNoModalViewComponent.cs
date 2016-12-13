using DomainObjects.ModelServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCarRental.ViewComponents
{
    public class YesNoModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(FormSubmissionService fss)
        {
            return View(fss);
        }
    }
}
