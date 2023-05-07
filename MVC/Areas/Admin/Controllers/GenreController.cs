using BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly ILogger<GenreController> _logger;

        public GenreController(IGenreService genreService, ILogger<GenreController> logger)
        {
            _genreService = genreService;
            _logger = logger;
        }

        public IActionResult AllGenres()
        {
            var allGenres = _genreService.GetAll();

            return Json(new { genres = allGenres });
        }
    }
}
