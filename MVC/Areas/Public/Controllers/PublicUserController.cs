using AutoMapper;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MVC.Models;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace MVC.Areas.Public.Controllers
{
    [Area("Public")]
    [Authorize(Roles = "Admin,User")]
    public class PublicUserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        private readonly ILogger<PublicUserController> _logger;

        public PublicUserController(
            IUserService userService, 
            ICountryService countryService, 
            ILogger<PublicUserController> logger,
            IMapper mapper)
        {
            _userService = userService;
            _countryService = countryService;
            _logger = logger;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public IActionResult Registration()
        {
            try
            {
                UserRegisterVM userRegisterVm = new()
                {
                    UserForRegister = new(),
                    Countries = _countryService.GetAll().Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                };

                return View(userRegisterVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send data to view for user registration");
                return RedirectToAction("Error", "Home");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(UserRegisterVM userRegisterVM)
        {
            if (!ModelState.IsValid) return View(userRegisterVM);

            try
            {
                _userService.Create(userRegisterVM.UserForRegister);
                return RedirectToAction("Index", "Home");
            }
            catch(InvalidOperationException ex)
            {
                ModelState.AddModelError("AlreadyExist", "User with entered e-mail already exists");
                return View(userRegisterVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to register user");
                return RedirectToAction("Error", "Home");
            }
        }

        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogIn(LoginRequestVM loginRequest)
        {
            try
            {
                if (!ModelState.IsValid) return View(loginRequest);

                var request = _mapper.Map<LoginRequest>(loginRequest);
                var user = _userService.GetConfirmedUser(request);

                if (user == null) 
                {
                    ModelState.AddModelError("WrongRequest", "Wrong e-mail or password");
                    return View(loginRequest);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties()).Wait();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authentication failed");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult UserProfile(int id)
        {
            try
            {
                var user = _userService.Get(id);

                if (user == null) return NotFound();

                UserVM userVM = new UserVM()
                {
                    User = user
                };

                return View(userVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send user data to view");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult ChangePassword(string email)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ChangePasswordVM changePasswordVM = new()
            {
                Email = email,
                UserId = int.Parse(userId)
            };

            return View(changePasswordVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            try
            {
                if(!ModelState.IsValid) return View(changePasswordVM);

                var request = _mapper.Map<ChangePasswordRequest>(changePasswordVM);
                _userService.ChangePass(request);

                return RedirectToAction("UserProfile", new { id = changePasswordVM.UserId });
            }
            catch(InvalidOperationException)
            {
                ModelState.AddModelError("WrongData", "Wrong old password");
                return View(changePasswordVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to change password");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Index", "Home");
        }
    }
}
