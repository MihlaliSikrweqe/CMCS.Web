using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CMCS.Web.Hubs
{
    public class ClaimsHub : Hub
    {
        public async Task NotifyClaimUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveClaimUpdate", message);
        }
    }
}

