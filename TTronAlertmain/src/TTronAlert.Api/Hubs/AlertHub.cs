using Microsoft.AspNetCore.SignalR;

namespace TTronAlert.Api.Hubs;

public class AlertHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var workstationId = Context.GetHttpContext()?.Request.Query["workstationId"];
        if (!string.IsNullOrEmpty(workstationId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Workstation_{workstationId}");
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, "AllWorkstations");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var workstationId = Context.GetHttpContext()?.Request.Query["workstationId"];
        if (!string.IsNullOrEmpty(workstationId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Workstation_{workstationId}");
        }
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "AllWorkstations");
        await base.OnDisconnectedAsync(exception);
    }
}