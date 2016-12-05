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
            //For testing purposes. Remember to remove this.
            ReservationLocationViewModel rlvm = new ReservationLocationViewModel { PickupDate = DateTime.Now, ReturnDate = DateTime.Now, StoreLocation = "jkdjkjsjk" };
            return View(rlvm);
        }

        [HttpPost]
        public IActionResult Index(ReservationLocationViewModel lsvm)
        {
            if (ModelState.IsValid)
            {
                var mainReservationModel = new ReservationViewModel { InitialSetup = lsvm };
                SaveReservationToSession(mainReservationModel);
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
                var reservationViewModel = GetReservationFromSession();
                vsvm.ReservationVehicle = _vehicleRepository.GetVehicleById(vsvm.ReservationVehicle.Id);
                reservationViewModel.VehicleSetup = vsvm;
                SaveReservationToSession(reservationViewModel);                
                return RedirectToAction("ReservationReviewSetup");
            }
            return View(vsvm);
        }

        public IActionResult ReservationReviewSetup()
        {
            //create a new ReviewAndContact ViewModel
            RegistrationViewModel rvm = new RegistrationViewModel { ControllerName="Home",ActionName= "ReservationReviewSetup",submitButtonValue="MAKE pRESERVATION" };
            var reservationViewModel = GetReservationFromSession();
            if (reservationViewModel != null)
            {

                reservationViewModel.ReviewAndContactSetup = rvm;
                return View(reservationViewModel);
            }
            else
            {
                ModelState.AddModelError(string.Empty,"Error Loading This Page");
                return View();
            }
                
        }

        [HttpPost]
        public IActionResult ReservationReviewSetup(ReservationViewModel rvm)
        {
            return View();
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

        private ReservationViewModel GetReservationFromSession()
        {
            var sessionString = HttpContext.Session.GetString("reservationWizard");
            return JsonConvert.DeserializeObject<ReservationViewModel>(sessionString);
        }

        private void SaveReservationToSession(ReservationViewModel rvm)
        {
            HttpContext.Session.SetString("reservationWizard", JsonConvert.SerializeObject(rvm));
        }
    }
}
