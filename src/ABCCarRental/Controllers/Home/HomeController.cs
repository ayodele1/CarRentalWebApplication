using System;
using Microsoft.AspNetCore.Mvc;
using DomainObjects.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DataAccess;
using DomainObjects;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ABCCarRental.Controllers
{
    public class HomeController : Controller
    {
        private ReservationRepository _reservationRepository;
        private VehicleRepository _vehicleRepository;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public HomeController(ReservationRepository reservationRepo, VehicleRepository vehicleRepo, UserManager<ApplicationUser> userMgr, SignInManager<ApplicationUser> signInMgr)
        {
            _reservationRepository = reservationRepo;
            _vehicleRepository = vehicleRepo;
            _userManager = userMgr;
            _signInManager = signInMgr;
        }
        public IActionResult Index()
        {
            //For testing purposes. Remember to remove this.
            ReservationLocationViewModel rlvm = new ReservationLocationViewModel { FormController = "Home", FormAction = "Index", PickupDate = DateTime.Now, ReturnDate = DateTime.Now, StoreLocation = "jkdjkjsjk" };
            return View(rlvm);
        }

        [HttpPost]
        public IActionResult Index(ReservationLocationViewModel lsvm)
        {
            if (ModelState.IsValid)
            {
                var mainReservationModel = new ReservationViewModel { InitialSetup = lsvm };
                SaveObjectToSession(mainReservationModel, "reservationwizard");
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
                var reservationViewModel = GetReservationViewModelFromSession("reservationwizard");
                vsvm.ReservationVehicle = _vehicleRepository.GetVehicleById(vsvm.ReservationVehicle.Id);
                reservationViewModel.VehicleSetup = vsvm;
                SaveObjectToSession(reservationViewModel, "reservationwizard");
                return RedirectToAction("ReservationReviewSetup");
            }
            return View(vsvm);
        }

        public IActionResult ReservationReviewSetup()
        {
            var currModel = GetReservationViewModelFromSession("reservationwizard");
            var reviewAndContactSetup = new ReservationContactViewModel
            {
                TotalCost = CalculateReservationCost(currModel.VehicleSetup.ReservationVehicle.PricePerDay, currModel.InitialSetup.PickupDate,
                currModel.InitialSetup.ReturnDate, currModel.StatesTax)
            };
            currModel.ReviewAndContactSetup = reviewAndContactSetup;
            return View(currModel);
        }

        [HttpPost]
        public async Task<IActionResult> ReservationReviewSetup(ReservationViewModel rvm)
        {
            var reservationViewModel = GetReservationViewModelFromSession("reservationwizard");
            reservationViewModel.ReviewAndContactSetup = rvm.ReviewAndContactSetup;
            string userId = null;
            ApplicationUser currentUser = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = _userManager.GetUserId(HttpContext.User);
                currentUser = await _userManager.FindByIdAsync(userId);
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(rvm);
                }
                var user = await _userManager.FindByEmailAsync(rvm.ReviewAndContactSetup.Email);
                if (user != null)
                {
                    userId = user.Id;
                    currentUser = user;
                }
                else
                {
                    var newUser = Mapper.Map<ApplicationUser>(rvm.ReviewAndContactSetup);
                    try
                    {
                        var userCreation = await _userManager.CreateAsync(newUser);
                        if (userCreation.Succeeded)
                        {
                            currentUser = newUser;
                            var roleAddition = await _userManager.AddToRoleAsync(newUser, "noncustomer");
                            userId = newUser.Id;
                        }
                    }
                    catch (Exception)
                    {
                        //Log the Error
                        ModelState.AddModelError(string.Empty, "Reservation Could not be Added");
                    }
                }


            }

            if (userId != null)
            {
                var newReservation = new Reservation()
                {
                    ConfirmationNumber = GetConfirmationNumber(),
                    VehicleId = reservationViewModel.VehicleSetup.ReservationVehicle.Id,
                    PickupDate = reservationViewModel.InitialSetup.PickupDate,
                    ReturnDate = reservationViewModel.InitialSetup.ReturnDate,
                    StoreLocation = reservationViewModel.InitialSetup.StoreLocation,
                    TotalCost = CalculateReservationCost(reservationViewModel.VehicleSetup.ReservationVehicle.PricePerDay,
                                                         reservationViewModel.InitialSetup.PickupDate, reservationViewModel.InitialSetup.ReturnDate, rvm.StatesTax),
                };
                if (!_reservationRepository.AddNewReservation(newReservation, userId))
                {
                    //Log The Error
                    ModelState.AddModelError(string.Empty, "Reservation Could not be Added");
                    return View(rvm);
                }
                reservationViewModel.CurrentUserId = currentUser.Id;
                reservationViewModel.ConfirmationNumber = newReservation.ConfirmationNumber;
                SaveObjectToSession(reservationViewModel, "reservationwizard");
                //Send Email to User
            }
            return RedirectToAction("ReservationComplete");
        }

        public IActionResult ReservationComplete()
        {
            var currReservationViewModel = GetReservationViewModelFromSession("reservationwizard");
            return View(currReservationViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ReservationComplete(ReservationViewModel rvm)
        {
            var reservationViewModel = GetReservationViewModelFromSession("reservationwizard");
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _userManager.FindByIdAsync(reservationViewModel.CurrentUserId);
                    var passwordUpdate = await _userManager.AddPasswordAsync(currentUser, rvm.QuickSignUp.Password);
                    if (passwordUpdate.Succeeded)
                    {
                        await _userManager.RemoveFromRoleAsync(currentUser, "noncustomer");
                        await _userManager.AddToRoleAsync(currentUser, "customer");
                        await _signInManager.SignInAsync(currentUser, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Your account wasn't created successfully");
                }
            }

            return View();
        }

        public IActionResult SearchReservation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchReservation(SearchReservationViewModel srvm)
        {
            Reservation reservationToFind = null;
            if (!ModelState.IsValid)
            {
                return View(srvm);
            }
            var reservationOwner = await _userManager.FindByEmailAsync(srvm.Email);
            if (reservationOwner != null)
            {
                reservationToFind = _reservationRepository.FindReservationByUser(srvm.ConfirmationNumber, reservationOwner);
            }
            if (reservationOwner == null || reservationToFind == null)
            {
                ModelState.AddModelError(string.Empty, "Reservation Could not be Found");
                return View();
            }
            return RedirectToAction("UpdateCancelReservation", reservationToFind);
        }

        public async Task<IActionResult> UpdateCancelReservation(Reservation reservation)
        {
            var reservationOwner = await _userManager.FindByIdAsync(reservation.ApplicationUserId);
            ReservationLocationViewModel rlvm = new ReservationLocationViewModel
            {
                PickupDate = reservation.PickupDate,
                ReturnDate = reservation.ReturnDate,
                StoreLocation = reservation.StoreLocation,             
            };

            ReservationContactViewModel rcvm = new ReservationContactViewModel
            {
                Email = reservationOwner.Email,
                FirstName = reservationOwner.FirstName,
                LastName = reservationOwner.LastName,
                PhoneNumber = reservationOwner.PhoneNumber,
                TotalCost = reservation.TotalCost              
            };
            ReservationVehicleViewModel rvvm = new ReservationVehicleViewModel
            {
                ReservationVehicle = _vehicleRepository.GetVehicleById(reservation.VehicleId),
                Vehicles = _vehicleRepository.GetAllAvailableVehicles()                
            };
            ReservationViewModel rvm = new ReservationViewModel
            {
                InitialSetup = rlvm,
                VehicleSetup = rvvm,
                ReviewAndContactSetup = rcvm,
                CurrentUserId = reservation.ApplicationUserId,
                ConfirmationNumber = reservation.ConfirmationNumber                
            };
            SaveObjectToSession(rvm, "updatewizard");
            SaveObjectToSession(reservation, "updatecancelwizard");
            return View(rvm);
        }

        public IActionResult CancelReservation()
        {
            var reservationToCancel = GetReservationFromSession("updatecancelwizard");
            if (reservationToCancel != null)
            {
                try
                {
                    if (!_reservationRepository.DeleteReservation(reservationToCancel))
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Reservation Could not be deleted");
                    return RedirectToAction("UpdateCancelReservation", reservationToCancel);
                }
            }
            return RedirectToAction("CancelReservationComplete", reservationToCancel);
        }

        public IActionResult CancelReservationComplete(Reservation r)
        {
            ViewBag.ReservationConfirmationNumber = r.ConfirmationNumber;
            return View();
        }

        public IActionResult UpdateReservationInitial()
        {
            var rvmToUpdate = GetReservationViewModelFromSession("updatewizard");
            return View(rvmToUpdate.InitialSetup);
        }

        [HttpPost]
        public IActionResult UpdateReservationInitial(ReservationLocationViewModel rlvm)
        {
            if (ModelState.IsValid)
            {
                var rvmToUpdate = GetReservationViewModelFromSession("updatewizard");
                rvmToUpdate.InitialSetup = rlvm;
                SaveObjectToSession(rvmToUpdate, "updatewizard");
                return RedirectToAction("UpdateReservationVehicle");
            }

            return View(rlvm);
        }

        public IActionResult UpdateReservationVehicle()
        {
            var availableVehicles = _vehicleRepository.GetAllAvailableVehicles();
            var rvmToUpdate = GetReservationViewModelFromSession("updatewizard");
            return View(rvmToUpdate.VehicleSetup);
        }

        [HttpPost]
        public IActionResult UpdateReservationVehicle(ReservationVehicleViewModel rvvm)
        {
            if (ModelState.IsValid)
            {
                var rvmToUpdate = GetReservationViewModelFromSession("updatewizard");
                rvvm.ReservationVehicle = _vehicleRepository.GetVehicleById(rvvm.ReservationVehicle.Id);
                rvmToUpdate.VehicleSetup = rvvm;
                SaveObjectToSession(rvmToUpdate, "updatewizard");
                return RedirectToAction("UpdateReservationReview");
            }
            return View(rvvm);
        }

        public IActionResult UpdateReservationReview()
        {
            var rvmToUpdate = GetReservationViewModelFromSession("updatewizard");
            rvmToUpdate.ReviewAndContactSetup.TotalCost = CalculateReservationCost(rvmToUpdate.VehicleSetup.ReservationVehicle.PricePerDay, rvmToUpdate.InitialSetup.PickupDate,
                rvmToUpdate.InitialSetup.ReturnDate, rvmToUpdate.StatesTax);

            return View(rvmToUpdate);
        }

        [HttpPost]
        public IActionResult UpdateReservation(ReservationViewModel rvm)
        {
            var rvmToUpdate = GetReservationViewModelFromSession("updatewizard");
            rvmToUpdate.ReviewAndContactSetup = rvm.ReviewAndContactSetup;
            if (rvmToUpdate.IsDirty())
            {
                var reservationToUpdate = _reservationRepository.FindReservationByConfirmationNumber(rvmToUpdate.ConfirmationNumber);
                if (!_reservationRepository.UpdateReservation(reservationToUpdate))
                {
                    ModelState.AddModelError(string.Empty, "Error Occured while updating Database");
                }
            }
            return RedirectToAction("UpdateReservationComplete", rvmToUpdate);
        }

        public IActionResult UpdateReservationComplete(ReservationViewModel rvm)
        {
            return View(rvm);
        }

        public IActionResult CarInventory(VehicleType filterId)
        {
            var filteredVehicles = _vehicleRepository.GetVehicleByFilter(filterId);            
            var vlvm = new VehicleListViewModel { Vehicles = filteredVehicles, SelectedFilter = filterId, VehicleFilters = GetVehicleFilters() };
            return View(vlvm);
        }

        [HttpPost]
        public IActionResult CarInventory(VehicleListViewModel vlvm)
        {            
            vlvm.Vehicles = _vehicleRepository.GetVehicleByFilter(vlvm.SelectedFilter);
            vlvm.VehicleFilters = GetVehicleFilters();
            return View(vlvm);
        }

        private SelectList GetVehicleFilters()
        {
            var filters = new List<VehicleFilter>()
            {
                new VehicleFilter {Id=4, FilterName = "All" },
                new VehicleFilter {Id=0, FilterName="Car"},
                new VehicleFilter {Id=1, FilterName="SUV" },
                new VehicleFilter {Id=2, FilterName="Truck" },
                new VehicleFilter {Id=3, FilterName="Luxury" }               
            };
            return new SelectList(filters, "Id", "FilterName");
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

        private ReservationViewModel GetReservationViewModelFromSession(string sessionKey)
        {
            var sessionString = HttpContext.Session.GetString(sessionKey);
            return JsonConvert.DeserializeObject<ReservationViewModel>(sessionString);
        }

        private Reservation GetReservationFromSession(string sessionKey)
        {
            var sessionString = HttpContext.Session.GetString(sessionKey);
            return JsonConvert.DeserializeObject<Reservation>(sessionString);
        }

        private void SaveObjectToSession(object rvm, string sessionKey)
        {
            HttpContext.Session.SetString(sessionKey, JsonConvert.SerializeObject(rvm, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        private double CalculateReservationCost(double pricePerDay, DateTime PickupDate, DateTime ReturnDate, params double[] Taxes)
        {
            var totalVehicleCost = pricePerDay;
            var totalRentalDays = Convert.ToInt32((ReturnDate - PickupDate).TotalDays);
            if (totalRentalDays > 0)
            {
                totalVehicleCost = pricePerDay * totalRentalDays;
            }
            var totalRentalCost = totalVehicleCost;
            Taxes.ToList().ForEach(tax => { totalRentalCost = totalRentalCost + tax; });
            return Math.Round(totalRentalCost,2);
        }

        private long GetConfirmationNumber()
        {
            Random r = new Random();
            return Convert.ToInt64(r.Next(0, 1000000).ToString("D10"));
        }
    }
}
