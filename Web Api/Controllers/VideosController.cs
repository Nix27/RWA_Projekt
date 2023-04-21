using DAL.DTO;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpGet]
        public IActionResult GetAllVideos()
        {
            return Ok(_videoService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetVideo(int id)
        {
            var foundVideo = _videoService.Get(id);

            if(foundVideo == null) return NotFound();

            return Ok(foundVideo);
        }

        [HttpGet("[action]")]
        public IActionResult SearchVideos(int size, int page, string? filterNames, string? orderBy, string? direction)
        {
            return Ok(_videoService.Search(size, page, filterNames, orderBy, direction));
        }

        [HttpPost]
        public IActionResult CreateVideo([FromBody] VideoDto video)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(_videoService.Create(video));
        }
        
        [HttpPut("{id}")]
        public IActionResult UpdateVideo(int id, [FromBody] VideoDto video)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedVideo = _videoService.Update(id, video);

            if(updatedVideo == null) return NotFound();

            return Ok(updatedVideo);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deletedVideo = _videoService.Delete(id);

            if(deletedVideo == null) return NotFound();

            return Ok(deletedVideo);
        }
    }
}
