using Microsoft.AspNetCore.SignalR;

namespace CMCS.Web.Hubs
{
    public class ClaimsHub : Hub
    {
        public async Task Broadcast(string message)
        {
            await Clients.All.SendAsync("ReceiveClaimUpdate", message);
        }
    }
}
