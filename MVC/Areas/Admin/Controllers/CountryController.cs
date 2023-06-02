using AutoMapper;
using BL.DTO;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(
            ICountryService countryService, 
            ILogger<CountryController> logger,
            IMapper mapper)
        {
            _countryService = countryService;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult AllCountries(int page, int size)
        {
            try
            {
                if (size == 0)
                    size = 5;

                var pagedCountries = _countryService.GetPagedCountries(page, size);
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)_countryService.GetNumberOfCountries() / size);

                return View(_mapper.Map<IEnumerable<CountryVM>>(pagedCountries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get countries");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult CountryTableBodyPartial(int page, int size)
        {
            try
            {
                if (size == 0)
                    size = 1;

                var pagedCountries = _countryService.GetPagedCountries(page, size);
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = _countryService.GetNumberOfCountries() / size;

                return PartialView("_CountryTableBodyPartial", _mapper.Map<IEnumerable<CountryVM>>(pagedCountries));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get countries");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult CreateCountry()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCountry(CountryVM countryVM)
        {
            try
            {
                if (!ModelState.IsValid) return View(countryVM);

                _countryService.Create(_mapper.Map<CountryDto>(countryVM));

                return RedirectToAction("AllCountries");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add country");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult EditCountry(int id)
        {
            try
            {
                if (id == 0) return NotFound();

                var countryForEdit = _countryService.Get(id);

                if (countryForEdit == null) return NotFound();

                return View(_mapper.Map<CountryVM>(countryForEdit));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send data to view for edit country");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCountry(CountryVM countryVM)
        {
            try
            {
                if (!ModelState.IsValid) return View(countryVM);

                _countryService.Update(countryVM.Id ?? 0, _mapper.Map<CountryDto>(countryVM));

                return RedirectToAction("AllCountries");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to edit country");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpDelete]
        public IActionResult DeleteCountry(int id)
        {
            try
            {
                if (id == 0) return NotFound();

                var deletedCountry = _countryService.Delete(id);

                if (deletedCountry == null)
                    return Json(new { success = false, message = "Unable to delete country" });

                return Json(new { success = true, message = "Country deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to delete country");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }
    }
}
