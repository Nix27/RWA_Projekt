using DAL.Services;
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

        public HomeController(ILogger<HomeController> logger, IVideoService videoService)
        {
            _logger = logger;
            _videoService = videoService;
        }

        public IActionResult Index()
        {
            var allVideos = _videoService.GetAll();
            return View(allVideos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}