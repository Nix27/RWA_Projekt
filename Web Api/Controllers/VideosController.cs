using BL.DTO;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllVideos()
        {
            try
            {
                return Ok(_videoService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetVideo(int id)
        {
            try
            {
                var foundVideo = _videoService.Get(id);

                if (foundVideo == null) return NotFound();

                return Ok(foundVideo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult SearchVideos(int size, int page, string? filterNames, string? orderBy, string? direction)
        {
            try
            {
                return Ok(_videoService.Search(size, page, filterNames, orderBy, direction));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateVideo([FromBody] VideoDto video)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(_videoService.Create(video));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("{id}")]
        public IActionResult UpdateVideo(int id, [FromBody] VideoDto video)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedVideo = _videoService.Update(id, video);

                if (updatedVideo == null) return NotFound();

                return Ok(updatedVideo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVideo(int id)
        {
            try
            {
                var deletedVideo = _videoService.Delete(id);

                if (deletedVideo == null) return NotFound();

                return Ok(deletedVideo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
