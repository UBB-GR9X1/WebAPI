using ClassLibrary.Exceptions;
using ClassLibrary.Repository;
using ClassLibrary.Model;
using ClassLibrary.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _auth_service;

        public AccountController(IAuthService authService)
        {
            _auth_service = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                await _auth_service.loadUserByUsername(username);

                bool isValid = await _auth_service.verifyPassword(password);
                if (!isValid)
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View();
                }

                var user = _auth_service.allUserInformation;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, user.role),
                    new Claim(ClaimTypes.NameIdentifier, user.userId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                    });

                await _auth_service.logAction(ActionType.LOGIN);

                if (user.role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                /*else if (user.role == "Patient")
                {
                    return RedirectToAction("Dashboard", "Patient");
                }
                else if (user.role == "Doctor")
                {
                    return RedirectToAction("Dashboard", "Doctor");
                }*/

                return RedirectToAction("Index", "Home");
            }
            catch (AuthenticationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _auth_service.logAction(ActionType.LOGOUT);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}