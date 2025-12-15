using Microsoft.AspNetCore.SignalR;
using TTronAlert.Api.Data;
using Microsoft.EntityFrameworkCore;
using TTronAlert.Shared.Extensions;

namespace TTronAlert.Api.Hubs;

public class AlertHub : Hub
{
    private readonly AppDbContext _context;

    public AlertHub(AppDbContext context)
    {
        _context = context;
    }

    public override async Task OnConnectedAsync()
    {
        var workstationId = Context.GetHttpContext()?.Request.Query["workstationId"].ToString();
        if (!string.IsNullOrEmpty(workstationId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Workstation_{workstationId}");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, "AllWorkstations");

        var pending = await _context.Alerts
            .AsNoTracking()
            .Where(a => !a.IsArchived && !a.IsRead && (a.TargetWorkstationId == null || a.TargetWorkstationId == workstationId))
            .OrderBy(a => a.CreatedAt)
            .ToListAsync();

        foreach (var alert in pending)
        {
            await Clients.Caller.SendAsync("ReceiveAlert", alert.ToDto());
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var workstationId = Context.GetHttpContext()?.Request.Query["workstationId"].ToString();
        if (!string.IsNullOrEmpty(workstationId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Workstation_{workstationId}");
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "AllWorkstations");
        await base.OnDisconnectedAsync(exception);
    }
}