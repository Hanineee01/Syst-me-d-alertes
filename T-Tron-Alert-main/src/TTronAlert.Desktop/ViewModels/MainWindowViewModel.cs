using System;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using ReactiveUI;
using TTronAlert.Desktop.Services;
using TTronAlert.Shared.DTOs;

namespace TTronAlert.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase, IDisposable
{
    private readonly IAlertService _alertService;
    private readonly IToastNotificationService _toastService;
    private readonly System.Timers.Timer _connectionStatusTimer;
    private string _connectionStatus = "Disconnected";
    private bool _isDarkTheme;
    private bool _disposed;

    public ObservableCollection<AlertItemViewModel> Alerts { get; } = new();

    public string ConnectionStatus
    {
        get => _connectionStatus;
        set
        {
            this.RaiseAndSetIfChanged(ref _connectionStatus, value);
            UpdateTrayIconTooltip();
        }
    }

    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set
        {
            this.RaiseAndSetIfChanged(ref _isDarkTheme, value);
            UpdateTheme();
        }
    }

    public MainWindowViewModel(IAlertService alertService, IToastNotificationService toastService)
    {
        _alertService = alertService;
        _toastService = toastService;
        _alertService.AlertReceived += OnAlertReceived;
        
        _connectionStatusTimer = new System.Timers.Timer(2000);
        _connectionStatusTimer.Elapsed += (s, e) => UpdateConnectionStatus();
        _connectionStatusTimer.Start();
        
        UpdateConnectionStatus();
    }

    private void OnAlertReceived(object? sender, AlertDto alert)
    {
        Dispatcher.UIThread.Post(() =>
        {
            var alertViewModel = new AlertItemViewModel(alert);
            Alerts.Insert(0, alertViewModel);

            _toastService.ShowToast(alert);
        });
    }

    private void UpdateConnectionStatus()
    {
        var newStatus = _alertService.IsConnected ? "Connected" : "Disconnected";
        if (ConnectionStatus != newStatus)
        {
            Dispatcher.UIThread.Post(() => ConnectionStatus = newStatus);
        }
    }

    private void UpdateTrayIconTooltip()
    {
        if (App.AppTrayIcon != null)
        {
            App.AppTrayIcon.ToolTipText = $"T-Tron Alert - {ConnectionStatus}";
        }
    }

    private void UpdateTheme()
    {
        if (Avalonia.Application.Current != null)
        {
            Avalonia.Application.Current.RequestedThemeVariant =
                _isDarkTheme ? Avalonia.Styling.ThemeVariant.Dark : Avalonia.Styling.ThemeVariant.Light;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _connectionStatusTimer?.Stop();
            _connectionStatusTimer?.Dispose();
            _alertService.AlertReceived -= OnAlertReceived;
            _disposed = true;
        }
    }
}
