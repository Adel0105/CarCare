using CarCare.Application.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetAsync(CancellationToken cancellationToken)
    {
        var dashboard = await _dashboardService.GetDashboardAsync(cancellationToken);
        return Ok(dashboard);
    }
}
