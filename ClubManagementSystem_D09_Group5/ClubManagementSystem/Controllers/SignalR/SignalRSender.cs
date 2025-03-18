using BussinessObjects.Models;
using Microsoft.AspNetCore.SignalR;
using Services.Interface;

namespace ClubManagementSystem.Controllers.SignalR
{
    public class SignalRSender
    {
        private readonly IHubContext<ServerHub> _hubContext;
        private readonly IConnectionService _CS;

        public SignalRSender(IHubContext<ServerHub> hubContext, IConnectionService CS)
        {
            _hubContext = hubContext;
            _CS = CS;
        }

        public async Task Notify(Notification noti, int userID)
        {
            var connection = await _CS.GetConnection(userID);
            await _hubContext.Clients.Client(connection.ConnectionId).SendAsync("notifyNews", noti);
        }

    }
}
