using BL.DTO;
using BL.Services;
using DAL.IRepositories;
using Microsoft.AspNetCore.Authorization;
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
            try
            {
                return Ok(_tagService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTag(int id)
        {
            try
            {
                var foundTag = _tagService.Get(id);

                if (foundTag == null) return NotFound();

                return Ok(foundTag);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateTag([FromBody] TagDto tag)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(_tagService.Create(tag));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTag(int id, [FromBody] TagDto tag)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedTag = _tagService.Update(id, tag);

                if (updatedTag == null) return NotFound();

                return Ok(updatedTag);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteTag(int id)
        {
            try
            {
                var deletedTag = _tagService.Delete(id);

                if (deletedTag == null) return NotFound();

                return Ok(deletedTag);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
