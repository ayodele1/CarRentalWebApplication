using AutoMapper;
using DomainObjects;
using DomainObjects.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        private RoleManager<IdentityRole> _roleManager;

        public AuthController(SignInManager<ApplicationUser> signInMgr, UserManager<ApplicationUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _signInManager = signInMgr;
            _userManager = userMgr;
            _roleManager = roleMgr;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
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
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "UserName/Password is Invalid");
                }
            }
            return View(lvm);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(rvm.Email) == null)
                {
                    var newUser = Mapper.Map<ApplicationUser>(rvm);

                    var result = await _userManager.CreateAsync(newUser, rvm.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, "customer");
                        await _signInManager.SignInAsync(newUser, false);
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Account could not be created");
            }
            
            return View(rvm);
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
