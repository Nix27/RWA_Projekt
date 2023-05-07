using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;
using System.Security.Claims;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VideoController : Controller
    {
        private readonly IVideoService _videoService;
        private readonly IGenreService _genreService;
        private readonly IImageService _imageService;
        private readonly ITagService _tagService;
        private readonly ILogger<VideoController> _logger;

        public VideoController(
            IVideoService videoService, 
            IGenreService genreService, 
            IImageService imageService, 
            ITagService tagService,
            ILogger<VideoController> logger)
        {
            _videoService = videoService;
            _genreService = genreService;
            _imageService = imageService;
            _tagService = tagService;
            _logger = logger;
        }

        public IActionResult AllVideos()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = claimsIdentity.Name;

            try
            {
                var allVideos = _videoService.GetAllForView();
                return View(allVideos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get videos");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult CreateVideo()
        {
            try
            {
                VideoVM videoVM = new()
                {
                    Video = new(),
                    Genres = _genreService.GetAll().Select(g => new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString()
                    }),
                    Images = _imageService.GetAll().Select(i => new SelectListItem
                    {
                        Text = i.Content?.Substring(i.Content.LastIndexOf('/') + 1),
                        Value = i.Id.ToString()
                    }),
                    Tags = _tagService.GetAll().Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.Name
                    })
                };

                return View(videoVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send data to view for create video");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateVideo(VideoVM newVideo)
        {
            if(!ModelState.IsValid) return View(newVideo);

            try
            {
                _videoService.Create(newVideo.Video);

                return RedirectToAction("AllVideos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to create video");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult EditVideo(int id)
        {
            if(id == 0) return NotFound();

            try
            {
                var videoForEdit = _videoService.Get(id);

                if (videoForEdit == null) return NotFound();

                VideoVM videoVM = new()
                {
                    Video = videoForEdit,
                    Genres = _genreService.GetAll().Select(g => new SelectListItem
                    {
                        Text = g.Name,
                        Value = g.Id.ToString()
                    }),
                    Images = _imageService.GetAll().Select(i => new SelectListItem
                    {
                        Text = i.Content?.Substring(i.Content.LastIndexOf('/') + 1),
                        Value = i.Id.ToString()
                    }),
                    Tags = _tagService.GetAll().Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.Name
                    })
                };

                return View(videoVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send data to view for edit video");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditVideo(VideoVM videoForEdit)
        {
            if(!ModelState.IsValid) return View(videoForEdit);

            try
            {
                int id = videoForEdit.Video.Id ?? 0;

                _videoService.Update(id, videoForEdit.Video);

                return RedirectToAction("AllVideos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to edit video");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpDelete]
        public IActionResult DeleteVideo(int id)
        {
            if(id == 0) return NotFound();

            try
            {
                var deletedVideo = _videoService.Delete(id);

                if (deletedVideo == null)
                    return Json(new { success = false, message = "Unable to delete video" });

                return Json(new { success = true, message = "Video deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to delete video");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }
    }
}
