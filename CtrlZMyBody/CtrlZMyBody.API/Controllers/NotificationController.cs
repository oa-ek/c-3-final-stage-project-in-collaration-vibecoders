using CtrlZMyBody.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CtrlZMyBody.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public NotificationController(INotificationService notificationService) =>
            _notificationService = notificationService;

        // GET api/notification
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _notificationService.GetUserNotificationsAsync(CurrentUserId);
            return Ok(list);
        }

        // GET api/notification/unread
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnread()
        {
            var list = await _notificationService.GetUnreadAsync(CurrentUserId);
            return Ok(list);
        }

        // POST api/notification/{id}/read
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return Ok();
        }
    }
}
