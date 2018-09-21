using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signManager;
        private UserManager<User> _userManager;
        public AccountController(
            SignInManager<User> signManager,
            UserManager<User> userManager)
        {
            _signManager = signManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.UserName };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new LoginViewModel() { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid login attempt");

            if (string.IsNullOrWhiteSpace(model.ReturnUrl))
            {
                model.ReturnUrl = "";
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Employee");
        }
    }

    public class RegisterViewModel
    {
        [Required, MaxLength(64)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required, MaxLength(64)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }

    }
}