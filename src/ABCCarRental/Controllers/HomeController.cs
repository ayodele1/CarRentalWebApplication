using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DomainObjects.ViewModels;
using AutoMapper;
using DomainObjects;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DataAccess;

namespace ABCCarRental.Controllers
{
    public class HomeController : Controller
    {
        private ReservationRepository _reservationRepository;
        private VehicleRepository _vehicleRepository;

        public HomeController(ReservationRepository reservationRepo, VehicleRepository vehicleRepo)
        {
            _reservationRepository = reservationRepo;
            _vehicleRepository = vehicleRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ReservationLocationViewModel lsvm)
        {
            if (ModelState.IsValid)
            {
                var mainReservationModel = new ReservationViewModel { InitialSetup = lsvm };
                HttpContext.Session.SetString("reservationWizard", JsonConvert.SerializeObject(mainReservationModel));
                return RedirectToAction("ReservationVehicleSetup");
            }

            return View(lsvm);
        }

        public IActionResult ReservationVehicleSetup()
        {
            var availableVehicles = _vehicleRepository.GetAllAvailableVehicles();
            var vehicleSetupViewModel = new ReservationVehicleViewModel(availableVehicles);            
            return View(vehicleSetupViewModel);
        }

        [HttpPost]
        public IActionResult ReservationVehicleSetup(ReservationVehicleViewModel vsvm)
        {
            if (ModelState.IsValid)
            {
                var sessionString = HttpContext.Session.GetString("reservationWizard");
                var reservationViewModel = JsonConvert.DeserializeObject<ReservationViewModel>(sessionString);
                reservationViewModel.VehicleSetup = vsvm;                
                return RedirectToAction("ReservationContactSetup");
            }
            return View(vsvm);
        }

        public IActionResult ReservationContactSetup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReservationContactSetup(ReservationContactViewModel rcvm)
        {
            if (ModelState.IsValid)
            {
                return View(rcvm);
            }
            return View(rcvm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
