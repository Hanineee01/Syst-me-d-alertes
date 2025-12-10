namespace TTronAlert.Shared.DTOs;

public record UpdateAlertDto(
    bool? IsRead = null,
    bool? IsArchived = null
);
