using DAL.DTO;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var foundUser = _userService.Get(id);

            if (foundUser == null) return NotFound();

            return Ok(foundUser);
        }

        [HttpPost("[action]")]
        public IActionResult RegisterUser([FromBody] UserRegisterRequest user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(_userService.Create(user));
        }

        [HttpPost("[action]")]
        public IActionResult LogIn([FromBody] LoginRequest request)
        {
            try
            {
               var jwtToken = _userService.GetToken(request);

                return Ok(jwtToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDto user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedUser = _userService.Update(id, user);

            if (updatedUser == null) return NotFound();

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deletedUser = _userService.Delete(id);

            if (deletedUser == null) return NotFound();

            return Ok(deletedUser);
        }

        [HttpPost("[action]")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                _userService.ChangePass(request);

                return Ok("Password changed successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
