using BL.DTO;
using BL.Services;
using DAL.IRepositories;
using Microsoft.AspNetCore.Authorization;
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
            try
            {
                return Ok(_genreService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetGenre(int id)
        {
            try
            {
                var foundGenre = _genreService.Get(id);

                if (foundGenre == null) return NotFound();

                return Ok(foundGenre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateGenre([FromBody] GenreDto genre)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(_genreService.Create(genre));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGenre(int id, [FromBody] GenreDto genre)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedGenre = _genreService.Update(id, genre);

                if (updatedGenre == null) return NotFound();

                return Ok(updatedGenre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGenre(int id)
        {
            try
            {
                var deletedGenre = _genreService.Delete(id);

                if (deletedGenre == null) return NotFound();

                return Ok(deletedGenre);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message); 
            }
        }
    }
}
