using Microsoft.Extensions.DependencyInjection;
using SolarTracker.Application.Interfaces.Services;

namespace SolarTracker.IoC;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddSolarApplication(this IServiceCollection services)
    {
        services.AddScoped<IInstallationSiteService, InstallationSiteService>();
        services.AddScoped<ISolarPanelService, SolarPanelService>();
        services.AddScoped<ISolarTrackingConfigurationService, SolarTrackingConfigurationService>();
        services.AddScoped<ILinearMotorService, LinearMotorService>();
        services.AddScoped<ITiltMeasuringUnitService, TiltMeasuringUnitService>();
        services.AddScoped<ICurrentMeasuringUnitService, CurrentMeasuringUnitService>();
        services.AddScoped<ILinearMotorMovementService, LinearMotorMovementService>();

        return services;
    }
}
