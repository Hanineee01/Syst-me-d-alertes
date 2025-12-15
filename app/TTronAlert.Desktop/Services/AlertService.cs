using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using TTronAlert.Desktop.Configuration;
using TTronAlert.Shared.DTOs;

namespace TTronAlert.Desktop.Services;

public class AlertService : IAlertService, IAsyncDisposable
{
    private readonly HubConnection _connection;
    private readonly AlertSystemSettings _settings;
    private readonly HttpClient _http;
    private readonly HashSet<int> _shown = new();
    private CancellationTokenSource? _pollCts;

    public string WorkstationId => _settings.WorkstationId;

    public event EventHandler<AlertDto>? AlertReceived;
    public event EventHandler<bool>? ConnectionStateChanged;

    public bool IsConnected => _connection.State == HubConnectionState.Connected;

    public AlertService(IOptions<AlertSystemSettings> settings)
    {
        _settings = settings.Value;
        _http = new HttpClient { BaseAddress = new Uri(_settings.ApiBaseUrl) };

        var builder = new HubConnectionBuilder()
            .WithUrl(_settings.GetHubUrl());

        if (_settings.AutoReconnect)
        {
            builder.WithAutomaticReconnect();
        }

        _connection = builder.Build();

        _connection.On<AlertDto>("ReceiveAlert", async alert =>
        {
            Console.WriteLine($"[AlertService] ReceiveAlert: {alert.Title}");
            if (!_shown.Contains(alert.Id))
            {
                _shown.Add(alert.Id);
                AlertReceived?.Invoke(this, alert);
                await MarkAsReadAsync(alert.Id);
            }
        });

        _connection.Reconnecting += error =>
        {
            Console.WriteLine($"[AlertService] Connection lost. Reconnecting... {error?.Message}");
            ConnectionStateChanged?.Invoke(this, false);
            return Task.CompletedTask;
        };

        _connection.Reconnected += connectionId =>
        {
            Console.WriteLine($"[AlertService] Reconnected. Connection ID: {connectionId}");
            ConnectionStateChanged?.Invoke(this, true);
            return Task.CompletedTask;
        };

        _connection.Closed += error =>
        {
            Console.WriteLine($"[AlertService] Connection closed. {error?.Message}");
            ConnectionStateChanged?.Invoke(this, false);
            return Task.CompletedTask;
        };
    }

    public async Task StartAsync()
    {
        try
        {
            Console.WriteLine($"[AlertService] Starting connection to {_settings.GetHubUrl()}");
            await _connection.StartAsync();
            Console.WriteLine("[AlertService] Connected to alert hub");
            ConnectionStateChanged?.Invoke(this, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AlertService] Error connecting to hub: {ex.Message}");
            ConnectionStateChanged?.Invoke(this, false);
        }

        _pollCts = new CancellationTokenSource();
        _ = PollPendingAsync(_pollCts.Token);
    }

    public async Task StopAsync()
    {
        Console.WriteLine("[AlertService] Stopping connection");
        _pollCts?.Cancel();
        await _connection.StopAsync();
        await _connection.DisposeAsync();
        ConnectionStateChanged?.Invoke(this, false);
    }

    private async Task PollPendingAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var url = $"/api/alerts/pending?workstationId={Uri.EscapeDataString(WorkstationId)}";
                var pending = await _http.GetFromJsonAsync<IEnumerable<AlertDto>>(url, ct);
                if (pending != null)
                {
                    foreach (var alert in pending)
                    {
                        if (_shown.Add(alert.Id))
                        {
                            AlertReceived?.Invoke(this, alert);
                            await MarkAsReadAsync(alert.Id);
                        }
                    }
                }
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                Console.WriteLine($"[AlertService] Poll error: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), ct);
        }
    }

    private async Task MarkAsReadAsync(int id)
    {
        try
        {
            var payload = JsonContent.Create(new { isRead = true });
            await _http.PutAsync($"/api/alerts/{id}", payload);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AlertService] MarkAsRead error: {ex.Message}");
        }
    }

    public async ValueTask DisposeAsync()
    {
        _pollCts?.Cancel();
        _http.Dispose();
        await (_connection.DisposeAsync());
    }
}