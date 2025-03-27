using BussinessObjects.Models;
using Microsoft.AspNetCore.SignalR;
using Services.Interface;

namespace ClubManagementSystem.Controllers.SignalR
{
    public class SignalRSender
    {
        private readonly IHubContext<ServerHub> _hubContext;
        private readonly IConnectionService _CS;
        private readonly INotificationService _notificationService;

        public SignalRSender(IHubContext<ServerHub> hubContext, IConnectionService CS, INotificationService notificationService)
        {
            _hubContext = hubContext;
            _CS = CS;
            _notificationService = notificationService;
        }

        public async Task Notify(Notification noti, int userID)
        {
            var connection = await _CS.GetConnection(userID);
            await _notificationService.AddNotificationAsync(noti);
            await _hubContext.Clients.Client(connection.ConnectionId).SendAsync("notifyNews", noti);
            //await _hubContext.Clients.All.SendAsync("notifyNews", noti);
        }

    }
}
