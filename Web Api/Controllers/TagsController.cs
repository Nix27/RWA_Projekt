using DAL.DTO;
using DAL.IRepositories;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public IActionResult GetAllTags()
        {
            return Ok(_tagService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetTag(int id)
        {
            var foundTag = _tagService.Get(id);

            if (foundTag == null) return NotFound();

            return Ok(foundTag);
        }

        [HttpPost]
        public IActionResult CreateTag([FromBody] TagDto tag)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(_tagService.Create(tag));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTag(int id, [FromBody] TagDto tag)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedTag = _tagService.Update(id, tag);

            if (updatedTag == null) return NotFound();

            return Ok(updatedTag);
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteTag(int id)
        {
            var deletedTag = _tagService.Delete(id);

            if (deletedTag == null) return NotFound();

            return Ok(deletedTag);
        }
    }
}
