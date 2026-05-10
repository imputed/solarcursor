using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Infrastructure.Persistence;
using SolarTracker.Infrastructure.Repositories.Concrete;

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

        services.AddScoped<IInstallationSiteService, InstallationSiteService>();
        services.AddScoped<ISolarPanelService, SolarPanelService>();
        services.AddScoped<ILinearMotorService, LinearMotorService>();

        return services;
    }
}
