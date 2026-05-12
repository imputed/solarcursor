using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Logging;

namespace SolarTracker.Infrastructure.Services;

public sealed class SolarPanelOptimizationBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<SolarPanelOptimizationBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            int intervalMinutes = SolarOptimizationScheduleConfiguration.DefaultIntervalMinutes;

            try
            {
                using IServiceScope scope = scopeFactory.CreateScope();
                var scheduleRepository =
                    scope.ServiceProvider.GetRequiredService<ISolarOptimizationScheduleConfigurationRepository>();
                var stateRepository =
                    scope.ServiceProvider.GetRequiredService<ISolarPanelOptimizationStateRepository>();
                var solarPanelQueryHandler = scope.ServiceProvider.GetRequiredService<ISolarPanelQueryHandler>();
                var installationSiteService = scope.ServiceProvider.GetRequiredService<IInstallationSiteService>();

                SolarOptimizationScheduleConfiguration schedule =
                    await scheduleRepository.GetAsync(stoppingToken);
                intervalMinutes = schedule.IntervalMinutes;

                IReadOnlyList<int> solarPanelIds =
                    await stateRepository.ListEnabledSolarPanelIdsAsync(stoppingToken);
                HashSet<int> installationSiteIds = [];

                foreach (int solarPanelId in solarPanelIds)
                {
                    SolarPanel? solarPanel = await solarPanelQueryHandler.GetByIdAsync(solarPanelId, stoppingToken);
                    if (solarPanel is not null)
                    {
                        installationSiteIds.Add(solarPanel.InstallationSiteId);
                        continue;
                    }

                    ResultError error = SolarTrackerErrorCatalog.SolarPanel.NotFound(solarPanelId);
                    InfrastructureLog.AutomaticOptimizationFailed(logger, solarPanelId, error.Code, error.Message);
                }

                foreach (int installationSiteId in installationSiteIds.Order())
                {
                    Result result = await OptimizeAsync(installationSiteService, installationSiteId, stoppingToken);
                    if (result.IsSuccess)
                        continue;

                    ResultError error = result.Error!.Value;
                    InfrastructureLog.AutomaticInstallationSiteOptimizationFailed(
                        logger,
                        installationSiteId,
                        error.Code,
                        error.Message);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                InfrastructureLog.AutomaticOptimizationLoopFailed(logger, exception);
            }

            try
            {
                await Task.Delay(TimeSpan.FromMinutes(intervalMinutes), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }

    private static async ValueTask<Result> OptimizeAsync(
        IInstallationSiteService installationSiteService,
        int installationSiteId,
        CancellationToken cancellationToken)
    {
        return await installationSiteService.Optimize(installationSiteId, cancellationToken);
    }
}
