using DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryService countryService, ILogger<CountryController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        public IActionResult AllCountries()
        {
            try
            {
                var allCountries = _countryService.GetAll();
                return View(allCountries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get countries");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }
    }
}
