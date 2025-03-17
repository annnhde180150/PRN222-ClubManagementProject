using BussinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using ClubManagementSystem.Controllers.SignalR;

namespace ClubManagementSystem.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _NS;
        private readonly IConnectionService _CS;
        private readonly IHubContext<ServerHub> _hubContext;

        public NotificationController(INotificationService NS, IConnectionService CS, IHubContext<ServerHub> hubContext)
        {
            _NS = NS;
            _CS = CS;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var notifications = await _NS.GetNotificationsAsync(Int32.Parse(UserID));
            return View(notifications);
        }

        public async Task<IActionResult> UpdateAsync(int NotiID) 
        {
            var notification = await _NS.GetNotificationAsync(NotiID);
            notification.IsRead = true;
            await _NS.UpdateNotificationAsync(notification);
            return View("Index",notification);
        }

    }
}
