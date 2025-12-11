using Microsoft.AspNetCore.Mvc;
using TTronAlert.Api.Services;

namespace TTronAlert.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IDatabaseHealthService _healthService;

    public HealthController(IDatabaseHealthService healthService)
    {
        _healthService = healthService;
    }

    /// <summary>
    /// Vérifie l'état de la connexion à la base de données
    /// </summary>
    /// <returns>Statut détaillé de la connexion à la base de données</returns>
    [HttpGet("database")]
    public async Task<ActionResult<DatabaseHealthStatus>> GetDatabaseHealth()
    {
        var status = await _healthService.GetHealthStatusAsync();
        
        if (!status.IsConnected)
        {
            return StatusCode(503, status); // Service Unavailable
        }

        return Ok(status);
    }

    /// <summary>
    /// Vérifie rapidement si la base de données est accessible
    /// </summary>
    /// <returns>true si connecté, false sinon</returns>
    [HttpGet("database/ping")]
    public async Task<ActionResult<bool>> PingDatabase()
    {
        var isConnected = await _healthService.CheckDatabaseConnectionAsync();
        return Ok(isConnected);
    }
}
