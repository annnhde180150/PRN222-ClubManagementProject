using Microsoft.AspNetCore.SignalR;
using Services.Interface;
using Services.Implementation;
using BussinessObjects.Models;

namespace ClubManagementSystem.Controllers.SignalR
{
    public class ServerHub : Hub
    {
        private readonly IConnectionService _connectionService;

        public ServerHub(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public override System.Threading.Tasks.Task OnConnectedAsync()
        {
            var con = new Connection
            {
                ConnectionId = Context.ConnectionId,
                //UserId = int.Parse(clam)
            };
            return base.OnConnectedAsync();
        }
    }
}
