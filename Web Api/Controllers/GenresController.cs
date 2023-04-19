using DAL.DTO;
using DAL.IRepositories;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public IActionResult GetAllGenres()
        {
            return Ok(_genreService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetGenre(int id)
        {
            var foundGenre = _genreService.Get(id); 

            if (foundGenre == null) return NotFound();

            return Ok(foundGenre);
        }

        [HttpPost]
        public IActionResult CreateGenre([FromBody] GenreDto genre)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(_genreService.Create(genre));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGenre(int id, [FromBody] GenreDto genre)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var updatedGenre = _genreService.Update(id, genre);

            if (updatedGenre == null) return NotFound();

            return Ok(updatedGenre);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGenre(int id)
        {
            var deletedGenre = _genreService.Delete(id);

            if(deletedGenre == null) return NotFound();

            return Ok(deletedGenre);
        }
    }
}
