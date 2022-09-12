using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Hubs
{
    public class ChatHub: Hub
    {
        public async Task SendMessage(string user, string message, string connectionId)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
            //await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
        }
        public override Task OnConnectedAsync()
        {
            //Count++;


            base.OnConnectedAsync();
            Clients.Client(Context.ConnectionId).SendAsync("connected", Context.ConnectionId);
            return Task.CompletedTask;
        }

        public async Task userTyped(string userId, string userName, string groupId,string type)
        {
            await Clients.All.SendAsync("ReceiveUserTypeInfo", userId, userName, groupId, type);
        }
    }
}
