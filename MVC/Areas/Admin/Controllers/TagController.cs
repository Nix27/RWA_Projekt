using AutoMapper;
using BL.DTO;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Security.Cryptography.Xml;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;
        private readonly IMapper _mapper;

        public TagController(
            ITagService tagService, 
            ILogger<TagController> logger,
            IMapper mapper)
        {
            _tagService = tagService;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult AllTags()
        {
            try
            {
                var allTags = _tagService.GetAll();
                return View(_mapper.Map<ICollection<TagVM>>(allTags));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get tags");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult CreateTag()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTag(TagVM newTag)
        {
            try
            {
                if (!ModelState.IsValid) return View(newTag);

                _tagService.Create(_mapper.Map<TagDto>(newTag));

                return RedirectToAction("AllTags");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to create tag");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult EditTag(int id)
        {
            try
            {
                if (id == 0) return NotFound();

                var tagForEdit = _tagService.Get(id);

                if (tagForEdit == null) return NotFound();

                return View(_mapper.Map<TagVM>(tagForEdit));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send data to view for edit tag");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTag(TagVM tag)
        {
            try
            {
                if (!ModelState.IsValid) return View(tag);

                int id = tag.Id ?? 0;

                _tagService.Update(id, _mapper.Map<TagDto>(tag));

                return RedirectToAction("AllTags");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to edit tag");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpDelete]
        public IActionResult DeleteTag(int id)
        {
            try
            {
                if (id == 0) return NotFound();

                var deletedTag = _tagService.Delete(id);

                if (deletedTag == null)
                    return Json(new { success = false, message = "Unable to delete tag" });

                return Json(new { success = true, message = "Tag deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to delete tag");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }
    }
}
