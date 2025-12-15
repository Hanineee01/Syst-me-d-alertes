using Microsoft.EntityFrameworkCore;
using TTronAlert.Api.Data;

namespace TTronAlert.Api.Services;

public interface IDatabaseHealthService
{
    Task<bool> CheckDatabaseConnectionAsync();
    Task<DatabaseHealthStatus> GetHealthStatusAsync();
}

public class DatabaseHealthService : IDatabaseHealthService
{
    private readonly AppDbContext _context;
    private readonly ILogger<DatabaseHealthService> _logger;

    public DatabaseHealthService(AppDbContext context, ILogger<DatabaseHealthService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CheckDatabaseConnectionAsync()
    {
        try
        {
            return await _context.Database.CanConnectAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la vérification de la connexion à la base de données");
            return false;
        }
    }

    public async Task<DatabaseHealthStatus> GetHealthStatusAsync()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (!canConnect)
            {
                return new DatabaseHealthStatus
                {
                    IsConnected = false,
                    Message = "Impossible de se connecter à la base de données",
                    LastChecked = DateTime.UtcNow
                };
            }

            // Test a simple query to ensure database is responsive
            var alertCount = await _context.Alerts.CountAsync();

            return new DatabaseHealthStatus
            {
                IsConnected = true,
                Message = $"Connecté à la base de données - {alertCount} alerte(s) en base",
                LastChecked = DateTime.UtcNow,
                AlertCount = alertCount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la vérification de l'état de santé de la base de données");
            return new DatabaseHealthStatus
            {
                IsConnected = false,
                Message = $"Erreur: {ex.Message}",
                LastChecked = DateTime.UtcNow
            };
        }
    }
}

public class DatabaseHealthStatus
{
    public bool IsConnected { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime LastChecked { get; set; }
    public int? AlertCount { get; set; }
}
