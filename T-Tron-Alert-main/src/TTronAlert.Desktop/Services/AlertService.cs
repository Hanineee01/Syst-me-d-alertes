using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using TTronAlert.Shared.DTOs;

namespace TTronAlert.Desktop.Services;

public class AlertService : IAlertService
{
    private readonly HubConnection _connection;

    public event EventHandler<AlertDto>? AlertReceived;

    public bool IsConnected => _connection.State == HubConnectionState.Connected;

    public AlertService()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5177/hubs/alerts")
            .WithAutomaticReconnect()
            .Build();

        _connection.On<AlertDto>("ReceiveAlert", alert =>
        {
            AlertReceived?.Invoke(this, alert);
        });

        _connection.Reconnecting += error =>
        {
            Console.WriteLine($"Connection lost. Reconnecting... {error?.Message}");
            return Task.CompletedTask;
        };

        _connection.Reconnected += connectionId =>
        {
            Console.WriteLine($"Reconnected. Connection ID: {connectionId}");
            return Task.CompletedTask;
        };

        _connection.Closed += error =>
        {
            Console.WriteLine($"Connection closed. {error?.Message}");
            return Task.CompletedTask;
        };
    }

    public async Task StartAsync()
    {
        try
        {
            await _connection.StartAsync();
            Console.WriteLine("Connected to alert hub");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to hub: {ex.Message}");
        }
    }

    public async Task StopAsync()
    {
        await _connection.StopAsync();
        await _connection.DisposeAsync();
    }
}
