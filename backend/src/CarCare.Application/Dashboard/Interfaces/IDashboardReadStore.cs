namespace CarCare.Application.Dashboard.Interfaces;

public interface IDashboardReadStore
{
    Task<DashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default);
}
