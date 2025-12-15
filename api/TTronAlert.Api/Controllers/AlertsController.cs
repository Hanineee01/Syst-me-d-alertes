using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TTronAlert.Api.Hubs;
using TTronAlert.Api.Services;
using TTronAlert.Shared.DTOs;
using TTronAlert.Shared.Extensions;

namespace TTronAlert.Api.Controllers;

[ApiController]
[Route("api/alerts")]
public class AlertsController : ControllerBase
{
    private readonly IAlertService _alertService;
    private readonly IHubContext<AlertHub> _hubContext;

    public AlertsController(IAlertService alertService, IHubContext<AlertHub> hubContext)
    {
        _alertService = alertService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlertDto>>> GetAll()
    {
        var alerts = await _alertService.GetAllAsync();
        return Ok(alerts.ToDto());
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<AlertDto>>> GetPending([FromQuery] string? workstationId)
    {
        var pending = await _alertService.GetPendingAsync(workstationId);
        return Ok(pending.ToDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlertDto>> GetById(int id)
    {
        var alert = await _alertService.GetByIdAsync(id);
        if (alert == null) return NotFound();
        return Ok(alert.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<AlertDto>> Create([FromBody] CreateAlertDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var alert = await _alertService.CreateAsync(dto);
        var alertDto = alert.ToDto();

        if (string.IsNullOrEmpty(alert.TargetWorkstationId))
        {
            Console.WriteLine("[AlertsController] Sending to AllWorkstations");
            await _hubContext.Clients.Group("AllWorkstations").SendAsync("ReceiveAlert", alertDto);
        }
        else
        {
            var group = $"Workstation_{alert.TargetWorkstationId}";
            Console.WriteLine($"[AlertsController] Sending to group {group}");
            await _hubContext.Clients.Group(group).SendAsync("ReceiveAlert", alertDto);
        }

        return CreatedAtAction(nameof(GetById), new { id = alert.Id }, alertDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AlertDto>> Update(int id, UpdateAlertDto dto)
    {
        var alert = await _alertService.UpdateAsync(id, dto);
        if (alert == null) return NotFound();
        return Ok(alert.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _alertService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}