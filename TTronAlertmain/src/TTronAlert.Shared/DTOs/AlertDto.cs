namespace TTronAlert.Shared.DTOs;

using TTronAlert.Shared.Models;

public record AlertDto(
    int Id,
    string Title,
    string Message,
    AlertLevel Level,
    DateTime CreatedAt,
    bool IsRead,
    bool IsArchived,
    string? TargetWorkstationId  // Correction : string? au lieu de int?
);