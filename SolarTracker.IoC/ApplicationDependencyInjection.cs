using Microsoft.Extensions.DependencyInjection;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Services;

namespace SolarTracker.IoC;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddSolarApplication(this IServiceCollection services)
    {
        services.AddScoped<IInstallationSiteService, InstallationSiteService>();
        services.AddScoped<ISolarPanelService, SolarPanelService>();
        services.AddScoped<ISolarPanelOptimizationStateService, SolarPanelOptimizationStateService>();
        services.AddScoped<ISolarOptimizationScheduleConfigurationService, SolarOptimizationScheduleConfigurationService>();
        services.AddScoped<ISolarTrackingConfigurationService, SolarTrackingConfigurationService>();
        services.AddScoped<ILinearMotorService, LinearMotorService>();
        services.AddScoped<ITiltMeasuringUnitService, TiltMeasuringUnitService>();
        services.AddScoped<ICurrentMeasuringUnitService, CurrentMeasuringUnitService>();
        return services;
    }
}
