using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
namespace TrackingAPI.Hubs
{
    public class TrackingHub : Hub
    {
        //public async Task SendMessage(string user, string message, string connectionId)
        //{
        //    await Clients.All.SendAsync("BookingAccepted", user, message);
        //    //await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
        //}
        public override Task OnConnectedAsync()
        {
            //Count++;
            base.OnConnectedAsync();
            Clients.Client(Context.ConnectionId).SendAsync("connected", Context.ConnectionId);
            return Task.CompletedTask;
        }

       
    }
}
