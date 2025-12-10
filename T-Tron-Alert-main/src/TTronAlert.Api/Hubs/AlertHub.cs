using Microsoft.AspNetCore.SignalR;

namespace TTronAlert.Api.Hubs;

public class AlertHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "AllWorkstations");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "AllWorkstations");
        await base.OnDisconnectedAsync(exception);
    }
}
