using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarTracker.Application.Interfaces.Calculators;
using SolarTracker.Application.Interfaces.Hardware;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Infrastructure.Calculators;
using SolarTracker.Infrastructure.Hardware;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.QueryHandlers.CurrentMeasuringUnit;
using SolarTracker.Infrastructure.QueryHandlers.InstallationSite;
using SolarTracker.Infrastructure.QueryHandlers.LinearMotor;
using SolarTracker.Infrastructure.QueryHandlers.SolarPanel;
using SolarTracker.Infrastructure.QueryHandlers.TiltMeasuringUnit;
using SolarTracker.Infrastructure.Repositories.Concrete;
using SolarTracker.Infrastructure.Services;

namespace SolarTracker.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddSolarInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SolarTracker")
            ?? "Data Source=solartracker.db";

        services.AddDbContext<SolarTrackerDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IInstallationSiteRepository, InstallationSiteRepository>();
        services.AddScoped<ISolarPanelRepository, SolarPanelRepository>();
        services.AddScoped<ILinearMotorRepository, LinearMotorRepository>();
        services.AddScoped<ITiltMeasuringUnitRepository, TiltMeasuringUnitRepository>();
        services.AddScoped<ICurrentMeasuringUnitRepository, CurrentMeasuringUnitRepository>();
        services.AddScoped<ISolarOptimizationScheduleConfigurationRepository, SolarOptimizationScheduleConfigurationRepository>();
        services.AddScoped<ISolarPanelOptimizationStateRepository, SolarPanelOptimizationStateRepository>();
        services.AddScoped<ISolarTrackingConfigurationRepository, SolarTrackingConfigurationRepository>();
        services.AddScoped<IInstallationSiteQueryHandler, InstallationSiteQueryHandler>();
        services.AddScoped<ISolarPanelQueryHandler, SolarPanelQueryHandler>();
        services.AddScoped<ILinearMotorQueryHandler, LinearMotorQueryHandler>();
        services.AddScoped<ITiltMeasuringUnitQueryHandler, TiltMeasuringUnitQueryHandler>();
        services.AddScoped<ICurrentMeasuringUnitQueryHandler, CurrentMeasuringUnitQueryHandler>();
        services.AddScoped<LinearMotorMovementService>();
        services.AddScoped<ISolarPanelCalculator, SolarPanelCalculator>();
        services.AddSingleton(TimeProvider.System);
        services.AddHostedService<SolarPanelOptimizationBackgroundService>();

        bool? configuredUseFakeGpio = bool.TryParse(configuration["RaspberryPi:UseFakeGpio"], out bool parsedUseFakeGpio)
            ? parsedUseFakeGpio
            : null;
        bool useFakeGpio = configuredUseFakeGpio
            ?? !(OperatingSystem.IsLinux() &&
                 (RuntimeInformation.ProcessArchitecture == Architecture.Arm ||
                  RuntimeInformation.ProcessArchitecture == Architecture.Arm64));

        if (useFakeGpio)
        {
            services.AddSingleton<ILinearMotorActuator, FakeLinearMotorActuator>();
            services.AddSingleton<ITiltMeasuringUnitPositionReader, FakeTiltMeasuringUnitPositionReader>();
        }
        else
        {
            services.AddSingleton<ILinearMotorActuator, RaspberryPiLinearMotorActuator>();
            services.AddSingleton<ITiltMeasuringUnitPositionReader, RaspberryPiTiltMeasuringUnitPositionReader>();
        }

        return services;
    }
}
