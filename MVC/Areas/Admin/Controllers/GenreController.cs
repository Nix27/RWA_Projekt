using AutoMapper;
using BL.DTO;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Collections.Generic;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly ILogger<GenreController> _logger;
        private readonly IMapper _mapper;

        public GenreController(
            IGenreService genreService, 
            ILogger<GenreController> logger,
            IMapper mapper)
        {
            _genreService = genreService;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult AllGenres()
        {
            var allGenres = _genreService.GetAll();
            return View(_mapper.Map<ICollection<GenreVM>>(allGenres));
        }

        public IActionResult GenreTableBodyPartial()
        {
            try
            {
                var allGenres = _mapper.Map<ICollection<GenreVM>>(_genreService.GetAll());
                return PartialView("_GenreTableBodyPartial", allGenres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get genres");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult CreateGenre()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateGenre([FromBody] GenreVM genreVM)
        {
            try
            {
                if (!ModelState.IsValid) return View(genreVM);

                _genreService.Create(_mapper.Map<GenreDto>(genreVM));

                return RedirectToAction("AllGenres");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add new genre");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult EditGenre(int id)
        {
            try
            {
                if (id == 0) return NotFound();

                var genreForEdit = _genreService.Get(id);

                if(genreForEdit == null) return NotFound();

                return View(_mapper.Map<GenreVM>(genreForEdit));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get genre");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpPut]
        public IActionResult EditGenre([FromBody] GenreVM genreVM)
        {
            try
            {
                if (!ModelState.IsValid) return View(genreVM);

                _genreService.Update(genreVM.Id ?? 0, _mapper.Map<GenreDto>(genreVM));

                return RedirectToAction("AllGenres");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add new genre");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpDelete]
        public IActionResult DeleteGenre(int id)
        {
            try
            {
                if (id == 0) return NotFound();

                var deletedGenre = _genreService.Delete(id);

                if (deletedGenre == null)
                    return Json(new { success = false, message = "Unable to delete genre" });

                return Json(new { success = true, message = "Genre deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to delete genre");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }
    }
}
