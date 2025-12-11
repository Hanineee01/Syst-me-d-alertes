using System.ComponentModel.DataAnnotations;
using TTronAlert.Shared.Models;

namespace TTronAlert.Shared.DTOs;

public record CreateAlertDto(
    [Required(ErrorMessage = "Le titre est requis")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Le titre doit contenir entre 1 et 200 caractères")]
    string Title,

    [Required(ErrorMessage = "Le message est requis")]
    [StringLength(2000, MinimumLength = 1, ErrorMessage = "Le message doit contenir entre 1 et 2000 caractères")]
    string Message,

    [Required(ErrorMessage = "Le niveau d'alerte est requis")]
    AlertLevel Level,

    [StringLength(100, ErrorMessage = "L'ID du poste cible doit contenir au maximum 100 caractères")]
    string? TargetWorkstationId = null
);