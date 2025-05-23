using ClassLibrary.Exceptions;
using ClassLibrary.Repository;
using ClassLibrary.Model;
using ClassLibrary.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml.Linq;
using System.Diagnostics;

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

        [HttpGet]
        public IActionResult Register()
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

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string mail, string name, DateOnly birth_date, string cnp, string selected_blood_type, string emergency_contact, double weight, int height)
        {
            try
            {
                BloodType? selected_blood_type_fin = null;
                string? selected_tag = selected_blood_type;
                    if (selected_tag != null)
                    {
                        switch (selected_tag.Trim())
                        {
                            case "A_POSITIVE":
                                selected_blood_type_fin = BloodType.A_POSITIVE;
                                break;
                            case "A_NEGATIVE":
                                selected_blood_type_fin = BloodType.A_NEGATIVE;
                                break;
                            case "B_POSITIVE":
                                selected_blood_type_fin = BloodType.B_POSITIVE;
                                break;
                            case "B_NEGATIVE":
                                selected_blood_type_fin = BloodType.B_NEGATIVE;
                                break;
                            case "AB_POSITIVE":
                                selected_blood_type_fin = BloodType.AB_POSITIVE;
                                break;
                            case "AB_NEGATIVE":
                                selected_blood_type_fin = BloodType.AB_NEGATIVE;
                                break;
                            case "O_POSITIVE":
                                selected_blood_type_fin = BloodType.O_POSITIVE;
                                break;
                            case "O_NEGATIVE":
                                selected_blood_type_fin = BloodType.O_NEGATIVE;
                                break;
                        }
                    }

                if (selected_blood_type_fin == null)
                {
                    throw new AuthenticationException("Invalid Blood Type!");
                }

                UserCreateAccountModel create_account_model = new UserCreateAccountModel(username, password, mail, name, birth_date, cnp, (BloodType)selected_blood_type_fin, emergency_contact, weight, height);
                Debug.WriteLine(create_account_model.ToString());
                bool isValid = await _auth_service.createAccount(create_account_model);

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

                await _auth_service.logAction(ActionType.CREATE_ACCOUNT);
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