using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;

namespace MVC.Areas.Public.Controllers
{
    [Area("Public")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVideoService _videoService;
        private readonly IImageService _imageService;
        private readonly IGenreService _genreService;

        public HomeController(
            ILogger<HomeController> logger, 
            IVideoService videoService, 
            IImageService imageService, 
            IGenreService genreService)
        {
            _logger = logger;
            _videoService = videoService;
            _imageService = imageService;
            _genreService = genreService;
        }

        public IActionResult Index(int page, int size, string? filterBy, string? filter)
        {
            try
            {
                if (size == 0)
                    size = 1;

                var pagedVideos = _videoService.GetPagedVideos(page, size);
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)_videoService.GetNumberOfVideos() / size);

                if (filter != null && filterBy != "none")
                {
                    pagedVideos = _videoService.GetFilteredVideos(pagedVideos, filterBy, filter);
                }

                var videosVm = pagedVideos.Select(v => new VideoVM
                {
                    Video = v,
                    ImageURL = Url.Action("GetImage", "Video", new { area = "Admin", id = v.ImageId })
                });

                return View(videosVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get videos");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult HomeVideoTableBodyPartial(int page, int size, string? filterBy, string? filter)
        {
            try
            {
                if (size == 0)
                    size = 1;

                var pagedVideos = _videoService.GetPagedVideos(page, size);
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)_videoService.GetNumberOfVideos() / size);

                if (filter != null && filterBy != "none")
                {
                    pagedVideos = _videoService.GetFilteredVideos(pagedVideos, filterBy, filter);
                }

                var videosVm = pagedVideos.Select(v => new VideoVM
                {
                    Video = v,
                    ImageURL = Url.Action("GetImage", "Video", new { area = "Admin", id = v.ImageId })
                });

                return PartialView("_HomeVideosPartial", videosVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get videos");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult VideoDetails(int id)
        {
            if (id == 0) return NotFound();

            try
            {
                var requestedVideo = _videoService.Get(id);

                VideoVM videoVm = new()
                {
                    Video = requestedVideo,
                    ImageURL = Url.Action("GetImage", "Video", new { area = "Admin", id = requestedVideo.ImageId }),
                    Genre = _genreService.Get(requestedVideo.GenreId).Name
                };

                return View(videoVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get requested video");
                return RedirectToAction("Error", "Home");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}