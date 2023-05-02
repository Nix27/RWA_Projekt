using DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public IActionResult AllCountries()
        {
            var allCountries = _countryService.GetAll();
            return View(allCountries);
        }
    }
}
