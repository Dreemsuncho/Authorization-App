using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using InventoryControl.Repositories;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using Microsoft.AspNetCore.Authorization;

namespace InventoryControl.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            if (!_userRepository.ValidateLogin(userName, password))
            {
                ViewBag.ErrorMessage = "Invalid Login";
                return View();
            }

            var userPrincipal = _userRepository.Get(userName);
            await HttpContext.SignInAsync(Constants.MiddlewareScheme, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                IsPersistent = false,
                AllowRefresh = false
            });

            return RedirectToLocal(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Constants.MiddlewareScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}