using DAL.DTO;
using DAL.Models;
using DAL.Services;
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
            return Ok(_userService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var foundUser = _userService.Get(id);

            if (foundUser == null) return NotFound();

            return Ok(foundUser);
        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody] UserRegisterRequest user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(_userService.Create(user));
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
    }
}
