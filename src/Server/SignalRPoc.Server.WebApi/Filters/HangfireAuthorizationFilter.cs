namespace SignalRPoc.Server.WebApi.Filters;

using Hangfire.Dashboard;

public sealed class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}