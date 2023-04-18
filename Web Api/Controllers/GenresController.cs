using DAL.DTO;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAllGenres()
        {
            return Ok(_unitOfWork.Genre.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetGenre(int id)
        {
            var requestedGenre = _unitOfWork.Genre.GetFirstOrDefault(g => g.Id == id);

            if (requestedGenre == null) return NotFound();

            return Ok(requestedGenre);
        }

        [HttpPost]
        public IActionResult CreateGenre([FromBody] Genre genre)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            _unitOfWork.Genre.Add(genre);
            _unitOfWork.Save();

            return Ok(genre);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGenre(int id, [FromBody] Genre genre)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            _unitOfWork.Genre.Update(genre);
            _unitOfWork.Save();

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGenre(int id)
        {
            var genreForDelete = _unitOfWork.Genre.GetFirstOrDefault(g => g.Id == id);

            if(genreForDelete == null)
                return NotFound();

            _unitOfWork.Genre.Delete(genreForDelete);
            _unitOfWork.Save();

            return Ok(genreForDelete);
        }
    }
}
