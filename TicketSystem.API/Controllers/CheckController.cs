using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TicketManagementSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public CheckController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    [HttpGet(Name = "Check")]
    public async Task<IActionResult> Check()
    {
        var report = await _healthCheckService.CheckHealthAsync();

        return Ok(new
        {
            Status = report.Status.ToString(),   // Healthy / Unhealthy
            Message = "Ticket Management System API is running.",
            Time = DateTime.UtcNow.ToString("o"),
            Details = report.Entries.ToDictionary(e => e.Key, e => e.Value.Status.ToString())
        });
    }
}