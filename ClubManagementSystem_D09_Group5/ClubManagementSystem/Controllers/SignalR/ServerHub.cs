using Microsoft.AspNetCore.SignalR;
using Services.Interface;
using Services.Implementation;
using BussinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClubManagementSystem.Controllers.SignalR
{
    public class ServerHub : Hub
    {
        private readonly IConnectionService _connectionService;

        public ServerHub(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Connected");
            var userID = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(userID);
            Console.WriteLine(Context.ConnectionId);
            if(userID != null)
            {
                Console.WriteLine("Connected");
                var latest = await _connectionService.GetConnection(Int32.Parse(userID));
                if (latest != null && latest.ConnectionId != Context.ConnectionId)
                {
                    var con = new Connection
                    {
                        ConnectionId = Context.ConnectionId,
                        UserId = Int32.Parse(userID)
                    };
                    await _connectionService.AddConnection(con);
                }
            }
        }

        public override System.Threading.Tasks.Task OnDisconnectedAsync(System.Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
