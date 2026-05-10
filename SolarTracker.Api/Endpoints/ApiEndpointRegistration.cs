using SolarTracker.Api.Endpoints.InstallationSites;
using SolarTracker.Api.Endpoints.LinearMotors;
using SolarTracker.Api.Endpoints.SolarPanels;

namespace SolarTracker.Api.Endpoints;

internal static class ApiEndpointRegistration
{
    internal static WebApplication MapSolarTrackerApiEndpoints(this WebApplication app)
    {
        app.MapInstallationSiteEndpoints();
        app.MapSolarPanelEndpoints();
        app.MapLinearMotorEndpoints();
        return app;
    }
}
