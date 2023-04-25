using DAL.DTO;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            try
            {
                return Ok(_userService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var foundUser = _userService.Get(id);

                if (foundUser == null) return NotFound();

                return Ok(foundUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult RegisterUser([FromBody] UserRegisterRequest user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(_userService.Create(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public IActionResult ValidateEmail([FromQuery]ValidateEmilRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
               _userService.ValidateEmail(request);
               return Ok("Verified");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
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

            try
            {
                var updatedUser = _userService.Update(id, user);

                if (updatedUser == null) return NotFound();

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var deletedUser = _userService.Delete(id);

                if (deletedUser == null) return NotFound();

                return Ok(deletedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
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
