using BL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Models;

namespace MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICountryService _countryService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ICountryService countryService, ILogger<UserController> logger)
        {
            _userService = userService;
            _countryService = countryService;
            _logger = logger;
        }

        public IActionResult AllUsers(int page, int size, string? filterBy, string? filter)
        {
            try
            {
                if (size == 0)
                    size = 5;

                var pagedUsers = _userService.GetPagedUsers(page, size);
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)_userService.GetNumberOfUsers() / size);

                if (filterBy != "none" && filter != null)
                {
                    pagedUsers = _userService.GetFilteredUsers(pagedUsers, filterBy, filter);
                }

                var pagedUsersVM = pagedUsers.Select(u => new UserVM
                {
                    User = u
                });

                return View(pagedUsersVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get users");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult UserTableBodyPartial(int page, int size, string? filterBy, string? filter)
        {
            try
            {
                if (size == 0)
                    size = 5;

                var pagedUsers = _userService.GetPagedUsers(page, size);
                ViewData["page"] = page;
                ViewData["size"] = size;
                ViewData["pages"] = (int)Math.Ceiling((double)_userService.GetNumberOfUsers() / size);

                if (filterBy != "none" && filter != null)
                {
                    pagedUsers = _userService.GetFilteredUsers(pagedUsers, filterBy, filter);
                }

                var pagedUsersVM = pagedUsers.Select(u => new UserVM
                {
                    User = u
                });

                return PartialView("_UserTableBodyPartial", pagedUsersVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get users");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult CreateUser()
        {
            try
            {
                UserRegisterVM userRegisterVm = new()
                {
                    UserForRegister = new(),
                    Countries = _countryService.GetAll().Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                };

                return View(userRegisterVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send data to view for create user");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(UserRegisterVM userVm)
        {
            if (!ModelState.IsValid) return NotFound();

            try
            {
                _userService.Create(userVm.UserForRegister);

                return RedirectToAction("AllUsers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to create user");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        public IActionResult EditUser(int id = 0)
        {
            if(id == 0) return NotFound();

            try
            {
                var userForEdit = _userService.Get(id);

                if (userForEdit == null) return NotFound();

                UserVM userVm = new()
                {
                    User = userForEdit,
                    Countries = _countryService.GetAll().Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                };

                return View(userVm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to send data to view for edit user");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(UserVM userForEdit)
        {
            if(!ModelState.IsValid) return View(userForEdit);

            try
            {
                int id = userForEdit.User.Id ?? 0;

                _userService.Update(id, userForEdit.User);

                return RedirectToAction("AllUsers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to edit user");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            if(id == 0) return NotFound();

            try
            {
                var deletedVideo = _userService.Delete(id);

                if (deletedVideo == null)
                    return Json(new { success = false, message = "Unable to delete user" });

                return Json(new { success = true, message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to delete user");
                return RedirectToAction("Error", "Home", new { area = "Public" });
            }
        }
    }
}
