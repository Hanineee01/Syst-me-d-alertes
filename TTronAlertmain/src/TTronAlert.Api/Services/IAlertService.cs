using TTronAlert.Shared.DTOs;
using TTronAlert.Shared.Models;

namespace TTronAlert.Api.Services;

public interface IAlertService
{
    Task<IEnumerable<Alert>> GetAllAsync();
    Task<Alert?> GetByIdAsync(int id);
    Task<Alert> CreateAsync(CreateAlertDto dto);
    Task<Alert?> UpdateAsync(int id, UpdateAlertDto dto);
    Task<bool> DeleteAsync(int id);
}
