using DomainObjects.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCarRental.ViewComponents
{
    public class ReservationVehicleSetupViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ReservationVehicleViewModel rvvm)
        {
            return View(rvvm);
        }
    }
}
