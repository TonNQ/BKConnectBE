

using Microsoft.AspNetCore.SignalR;
using System.Xml.Linq;

namespace WebApplication1.ChatHub;

public class ChatHub : Hub
{
    private static Dictionary<string, string> users = new Dictionary<string, string>();

    public override Task OnConnectedAsync()
    {
        var httpContext = this.Context.GetHttpContext();
        var name = httpContext.Request.Query["name"].ToString();
        if (!string.IsNullOrEmpty(name) && !users.TryGetValue(name, out string cnnId))
        {
            users.Add(name, Context.ConnectionId);
            Clients.All.SendAsync("NotifyOnline", name).Wait();
        }
        else if(string.IsNullOrEmpty(name))
        {
            return Task.CompletedTask;
        }
        else
        {
            throw new Exception("Co roi");
        }
        return Task.CompletedTask;
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendOnGo(string user)
    {
        await Clients.All.SendAsync("OnGo", user);
    }

    public async Task SendOnDungGo(string user)
    {
        await Clients.All.SendAsync("OnDungGo", user);
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        var u = users.FirstOrDefault(u => u.Value == Context.ConnectionId);
        if (u.Key != null){
            users.Remove(u.Key);
            Clients.All.SendAsync("NotifyOffline", u.Key).Wait();
        }
        return Task.CompletedTask;
    }
}
