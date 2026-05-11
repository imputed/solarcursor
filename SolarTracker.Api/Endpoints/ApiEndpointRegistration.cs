using SolarTracker.Api.Endpoints.CurrentMeasuringUnits;
using SolarTracker.Api.Endpoints.InstallationSites;
using SolarTracker.Api.Endpoints.LinearMotors;
using SolarTracker.Api.Endpoints.SolarPanels;
using SolarTracker.Api.Endpoints.SolarTrackingConfiguration;
using SolarTracker.Api.Endpoints.TiltMeasuringUnits;

namespace SolarTracker.Api.Endpoints;

internal static class ApiEndpointRegistration
{
    internal static WebApplication MapSolarTrackerApiEndpoints(this WebApplication app)
    {
        app.MapInstallationSiteEndpoints();
        app.MapSolarPanelEndpoints();
        app.MapSolarTrackingConfigurationEndpoints();
        app.MapLinearMotorEndpoints();
        app.MapTiltMeasuringUnitEndpoints();
        app.MapCurrentMeasuringUnitEndpoints();
        return app;
    }
}
