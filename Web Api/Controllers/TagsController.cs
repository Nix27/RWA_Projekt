using DAL.DTO;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAllTags()
        {
            return Ok(_unitOfWork.Tag.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetTag(int id)
        {
            var requestedTag = _unitOfWork.Tag.GetFirstOrDefault(t => t.Id == id);

            if(requestedTag == null) return NotFound();

            return Ok(requestedTag);
        }

        [HttpPost]
        public IActionResult CreateTag([FromBody] TagDto tag)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newtag = new Tag
            {
                Name = tag.Name
            };

            _unitOfWork.Tag.Add(newtag);
            _unitOfWork.Save();

            return Ok(tag);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTag(int id, [FromBody] Tag tag)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            _unitOfWork.Tag.Update(tag);
            _unitOfWork.Save();

            return Ok(tag);
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteTag(int id)
        {
            var tagForDelete = _unitOfWork.Tag.GetFirstOrDefault(t => t.Id == id);

            if (tagForDelete == null) return NotFound();

            _unitOfWork.Tag.Delete(tagForDelete);
            _unitOfWork.Save();

            return Ok(tagForDelete);
        }
    }
}
