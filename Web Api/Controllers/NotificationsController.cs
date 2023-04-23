using DAL.DTO;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult GetAllNotifications()
        {
            return Ok(_notificationService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetNotification(int id)
        {
            var foundNotification = _notificationService.Get(id);

            if(foundNotification== null) return NotFound();

            return Ok(foundNotification);
        }

        [HttpPost]
        public IActionResult CreateNotification([FromBody] NotificationDto notification)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(_notificationService.Create(notification));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNotification(int id, [FromBody] NotificationDto notification)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedNotification = _notificationService.Update(id, notification);

            if(updatedNotification == null) return NotFound();

            return Ok(updatedNotification); 
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(int id)
        {
            var deletedNotification = _notificationService.Delete(id);

            if(deletedNotification == null) return NotFound();

            return Ok(deletedNotification);
        }
    }
}
