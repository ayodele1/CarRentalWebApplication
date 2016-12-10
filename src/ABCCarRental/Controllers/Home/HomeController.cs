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
            var currModel = GetReservationFromSession();
            return View(currModel);
        }

        [HttpPost]
        public async Task<IActionResult> ReservationReviewSetup(ReservationViewModel rvm)
        {
            var reservationViewModel = GetReservationFromSession();
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
                    ConfirmationNumber = reservationViewModel.ReviewAndContactSetup.ConfirmationNumber,
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
            }
            reservationViewModel.CurrentUserId = currentUser.Id;
            SaveReservationToSession(reservationViewModel);
            return RedirectToAction("ReservationComplete");
            //Send Email to User
        }

        public IActionResult ReservationComplete()
        {
            var currReservationViewModel = GetReservationFromSession();
            return View(currReservationViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ReservationComplete(ReservationViewModel rvm)
        {
            var reservationViewModel = GetReservationFromSession();
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
            HttpContext.Session.SetString("reservationWizard", JsonConvert.SerializeObject(rvm, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
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
            return totalRentalCost;
        }
    }
}
