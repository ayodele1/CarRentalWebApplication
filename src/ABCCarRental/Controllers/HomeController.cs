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

        public HomeController(ReservationRepository repository)
        {
            _reservationRepository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LocationSetupViewModel lsvm)
        {
            if (ModelState.IsValid)
            {
                var mainReservationModel = new ReservationViewModel { InitialSetup = lsvm };
                HttpContext.Session.SetString("reservationWizard", JsonConvert.SerializeObject(mainReservationModel));
                return View("VehicleSetup");
            }

            return View(lsvm);
        }

        public IActionResult VehicleSetup()
        {
            var sessionString = HttpContext.Session.GetString("reservationWizard");
            if (sessionString != null)
            {
                var reservationViewModel = JsonConvert.DeserializeObject<ReservationViewModel>(sessionString);
                return View(reservationViewModel.VehicleSetup);
            }
            return View();
        }

        [HttpPost]
        public IActionResult VehicleSetup(VehicleSetupViewModel vsvm)
        {
            if (ModelState.IsValid)
            {
                vsvm.VehicleId = 1;
                var reservationViewModel = JsonConvert.DeserializeObject<ReservationViewModel>(HttpContext.Session.GetString("reservationWizard"));
                reservationViewModel.VehicleSetup = vsvm;
                var newReservation = new Reservation
                {
                    ConfirmationNumber = 69878784,
                    CustomerId = 1,
                    PickupDate = reservationViewModel.InitialSetup.PickupDate,
                    ReturnDate = reservationViewModel.InitialSetup.ReturnDate,
                    StoreLocation = reservationViewModel.InitialSetup.StoreLocation,
                    TotalCost = 70,
                    VehicleId = vsvm.VehicleId
                                
                };
                try
                {
                    _reservationRepository.AddNewReservation(newReservation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("supin");
                    throw;
                }
                //            return View("NextSetup", mainReservationModel);
                return RedirectToAction("Index");
            }
            return View(vsvm);



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
