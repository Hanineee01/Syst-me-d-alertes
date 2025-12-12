using Microsoft.AspNetCore.SignalR;

namespace AlertesApi.Hubs
{
    public class AlertesHub : Hub
    {
        // Rien à faire ici, on n'utilise plus de groupe
        // Le client reçoit tout automatiquement avec Clients.All
    }
}