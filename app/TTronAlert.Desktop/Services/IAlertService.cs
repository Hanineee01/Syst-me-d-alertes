using System;
using System.Threading.Tasks;
using TTronAlert.Shared.DTOs;

namespace TTronAlert.Desktop.Services;

public interface IAlertService
{
    event EventHandler<AlertDto>? AlertReceived;
    event EventHandler<bool>? ConnectionStateChanged; // true = connected, false = disconnected
    Task StartAsync();
    Task StopAsync();
    bool IsConnected { get; }
}
