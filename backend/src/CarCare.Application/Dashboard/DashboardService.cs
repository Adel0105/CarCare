using CarCare.Application.Dashboard.Interfaces;

namespace CarCare.Application.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IDashboardReadStore _dashboardReadStore;

    public DashboardService(IDashboardReadStore dashboardReadStore)
    {
        _dashboardReadStore = dashboardReadStore;
    }

    public Task<DashboardDto> GetDashboardAsync(CancellationToken cancellationToken = default)
    {
        return _dashboardReadStore.GetDashboardAsync(cancellationToken);
    }
}
