using TTronAlert.Shared.DTOs;

namespace TTronAlert.Desktop.Services;

public interface IToastNotificationService
{
    void ShowToast(AlertDto alert);
}
