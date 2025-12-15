using Microsoft.EntityFrameworkCore;
using TTronAlert.Api.Data;
using TTronAlert.Shared.DTOs;
using TTronAlert.Shared.Models;

namespace TTronAlert.Api.Services;

public class AlertService : IAlertService
{
    private readonly AppDbContext _context;

    public AlertService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Alert>> GetAllAsync()
    {
        return await _context.Alerts
            .AsNoTracking()
            .Where(a => !a.IsArchived)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Alert>> GetPendingAsync(string? workstationId)
    {
        return await _context.Alerts
            .AsNoTracking()
            .Where(a => !a.IsArchived && !a.IsRead && (a.TargetWorkstationId == null || a.TargetWorkstationId == workstationId))
            .OrderBy(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Alert?> GetByIdAsync(int id)
    {
        return await _context.Alerts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Alert> CreateAsync(CreateAlertDto dto)
    {
        var alert = new Alert
        {
            Title = dto.Title,
            Message = dto.Message,
            Level = dto.Level,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            IsArchived = false,
            TargetWorkstationId = dto.TargetWorkstationId
        };

        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();

        return alert;
    }

    public async Task<Alert?> UpdateAsync(int id, UpdateAlertDto dto)
    {
        var alert = await _context.Alerts.FindAsync(id);
        if (alert == null) return null;

        if (dto.IsRead.HasValue)
            alert.IsRead = dto.IsRead.Value;

        if (dto.IsArchived.HasValue)
            alert.IsArchived = dto.IsArchived.Value;

        await _context.SaveChangesAsync();
        return alert;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var alert = await _context.Alerts.FindAsync(id);
        if (alert == null) return false;

        _context.Alerts.Remove(alert);
        await _context.SaveChangesAsync();
        return true;
    }
}