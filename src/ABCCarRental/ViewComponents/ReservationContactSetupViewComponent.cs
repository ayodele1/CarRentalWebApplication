﻿using DomainObjects.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCarRental.ViewComponents
{
    public class ReservationContactSetupViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ReservationViewModel rvm)
        {
            return View(rvm);
        }
    }
}
