using DAL.DTO;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;

        public TagController(ITagService tagService, ILogger<TagController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        public IActionResult AllTags()
        {
            try
            {
                var allTags = _tagService.GetAll();
                return View(allTags);
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
        public IActionResult CreateTag(TagDto newTag)
        {
            if(!ModelState.IsValid) return View(newTag);

            try
            {
                _tagService.Create(newTag);

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
            if(id == 0) return NotFound();

            try
            {
                var tagForEdit = _tagService.Get(id);

                if (tagForEdit == null) return NotFound();

                return View(tagForEdit);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send data to view for edit tag");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTag(TagDto tag)
        {
            if(!ModelState.IsValid) return View(tag);

            try
            {
                int id = tag.Id ?? 0;

                _tagService.Update(id, tag);

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
            if (id == 0) return NotFound();

            try
            {
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
