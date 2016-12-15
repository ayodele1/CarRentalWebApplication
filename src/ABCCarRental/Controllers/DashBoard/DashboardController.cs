using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using DomainObjects;
using DomainObjects.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ABCCarRental.Controllers.DashBoard
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ReservationRepository _reservationRepository;
        private UserManager<ApplicationUser> _userManager;

        // GET: /<controller>/
        public DashboardController(ReservationRepository reservationRepo, UserManager<ApplicationUser> userMgr)
        {
            _reservationRepository = reservationRepo;
            _userManager = userMgr;
        }
        public IActionResult Index()
        {
            var signedInUserId = _userManager.GetUserId(HttpContext.User);
            var userReservations = _reservationRepository.GetReservationsForUser(signedInUserId);
            var viewModel = new ReservationListViewModel { Reservations = userReservations };
            return View(viewModel);
        }
    }
}
