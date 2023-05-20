using AutoMapper;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using System.Security.Claims;

namespace MVC.Areas.Public.Controllers
{
    [Area("Public")]
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to register user");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogIn(LoginRequestVM loginRequest)
        {
            try
            {
                if (!ModelState.IsValid) return View(loginRequest);

                var user = _userService.GetConfirmedUser(_mapper.Map<LoginRequest>(loginRequest));

                if (user == null) 
                {
                    ModelState.AddModelError("WrongRequest", "Wrong e-mail or password");
                    return View(loginRequest);
                }

                var claims = new List<Claim>
                {
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

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Index", "Home");
        }
    }
}
