using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Threading;
using TTronAlert.Desktop.ViewModels;
using TTronAlert.Desktop.Views;
using TTronAlert.Shared.DTOs;

namespace TTronAlert.Desktop.Services;

public class ToastNotificationService : IToastNotificationService
{
    private readonly List<AlertToastWindow> _activeToasts = new();
    private readonly object _lock = new();

    public void ShowToast(AlertDto alert)
    {
        Dispatcher.UIThread.Post(() =>
        {
            lock (_lock)
            {
                var alertViewModel = new AlertItemViewModel(alert);
                var toastWindow = new AlertToastWindow();

                toastWindow.SetAlert(alertViewModel);
                toastWindow.PositionToast(_activeToasts.Count);

                toastWindow.Closed += (s, e) =>
                {
                    lock (_lock)
                    {
                        _activeToasts.Remove(toastWindow);
                        RepositionToasts();
                    }
                };

                _activeToasts.Add(toastWindow);
                toastWindow.Show();
            }
        });
    }

    private void RepositionToasts()
    {
        for (int i = 0; i < _activeToasts.Count; i++)
        {
            _activeToasts[i].PositionToast(i);
        }
    }
}
