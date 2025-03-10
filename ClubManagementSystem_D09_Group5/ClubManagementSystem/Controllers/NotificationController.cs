using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace ClubManagementSystem.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _NS;

        public NotificationController(INotificationService NS)
        {
            _NS = NS;
        }

        public async Task<IActionResult> Index()
        {
            var notifications = await _NS.GetNotificationsAsync(1);
            return View(notifications);
        }
    }
}
