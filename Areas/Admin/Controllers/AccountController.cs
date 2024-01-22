﻿using Mairala.Areas.Admin.ViewModels;
using Mairala.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mairala.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            AppUser user = await _userManager.FindByNameAsync(loginVm.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVm.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError(String.Empty, "Username, email or password is incorrect.");
                    return View(loginVm);
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, loginVm.IsRemembered, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(String.Empty, "Your account is offline pls try later.");
                    return View(loginVm);
                }
                ModelState.AddModelError(String.Empty, "Username, email or password is incorrect.");
                return View(loginVm);
            }

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (role == "Admin")
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home", new { area = "" });

        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVm);
            }
            AppUser user = new AppUser
            {
                Name = registerVm.UserName,
                Surname = registerVm.Surname,
                Email = registerVm.Email,
                UserName = registerVm.UserName
            };

            var result = await _userManager.CreateAsync(user, registerVm.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, item.Description);
                }
                return View(registerVm);
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public async Task<IActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = "Admin"
            });
            return RedirectToAction("Index", "Home", new { area = "" });
        }


    }
}
