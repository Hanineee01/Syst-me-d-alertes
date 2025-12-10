using TTronAlert.Shared.DTOs;
using TTronAlert.Shared.Models;

namespace TTronAlert.Shared.Extensions;

public static class AlertExtensions
{
    /// <summary>
    /// Converts an Alert entity to AlertDto
    /// </summary>
    public static AlertDto ToDto(this Alert alert)
    {
        return new AlertDto(
            alert.Id,
            alert.Title,
            alert.Message,
            alert.Level,
            alert.CreatedAt,
            alert.IsRead,
            alert.IsArchived
        );
    }

    /// <summary>
    /// Converts a collection of Alert entities to AlertDto collection
    /// </summary>
    public static IEnumerable<AlertDto> ToDto(this IEnumerable<Alert> alerts)
    {
        return alerts.Select(a => a.ToDto());
    }
}
