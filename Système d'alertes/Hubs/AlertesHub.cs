using Microsoft.AspNetCore.SignalR;

namespace AlertesApi.Hubs
{
    public class AlertesHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // On ajoute le poste dans le groupe "Tous" à la connexion
            await Groups.AddToGroupAsync(Context.ConnectionId, "Tous");
            await base.OnConnectedAsync();
        }
    }
}