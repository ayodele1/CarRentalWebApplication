using DomainObjects;
using DomainObjects.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCCarRental.Controllers.Auth
{
    public class AuthController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationUser> _roleManager;

        public AuthController(SignInManager<ApplicationUser> signInMgr, UserManager<ApplicationUser> userMgr, RoleManager<ApplicationUser> roleMgr)
        {
            _signInManager = signInMgr;
            _userManager = userMgr;
            _roleManager = roleMgr;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Reservations");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, true, false);
                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Reservation");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "UserName/Password is Invalid");
            }
            return View();
        }

        public IActionResult Register()
        {
            var regViewModel = new RegistrationViewModel
            {
                HidePasswordField = false,
                ActionName = "Register",
                ControllerName = "Auth",
                submitButtonValue = "REGISTER"
            };
            return View(regViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(rvm.EmailAddress) == null)
                {
                    var user = new ApplicationUser()
                    {
                        FirstName = rvm.FirstName,
                        LastName = rvm.LastName,
                        Email = rvm.EmailAddress,
                        UserName = rvm.EmailAddress,
                        PhoneNumber = rvm.PhoneNumber
                    };
                    await _userManager.AddToRoleAsync(user, "customer");
                    var result = await _userManager.CreateAsync(user, rvm.Password);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty,"Account could not be created");
            }
            return View(rvm);
        }
    }
}
