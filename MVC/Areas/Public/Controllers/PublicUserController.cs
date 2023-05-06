using DAL.Models;
using DAL.Services;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Areas.Public.Controllers
{
    [Area("Public")]
    public class PublicUserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICountryService _countryService;
        private readonly ILogger<PublicUserController> _logger;

        public PublicUserController(IUserService userService, ICountryService countryService, ILogger<PublicUserController> logger)
        {
            _userService = userService;
            _countryService = countryService;
            _logger = logger;
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
        public IActionResult LogIn(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid) return View(loginRequest);

            try
            {
                var jwtToken = _userService.GetToken(loginRequest);

                var username = _userService.GetAll().Where(u => u.Email == loginRequest.Email).First().UserName;

                var data = new { success = true, username = username, jwtToken = jwtToken};

                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Authentication failed");
                return Json(new { success = false, message = "Wrong email or password" });
            }
        }
    }
}
