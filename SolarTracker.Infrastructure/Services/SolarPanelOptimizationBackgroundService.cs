using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SolarTracker.Application.Dtos;
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
                var solarPanelService = scope.ServiceProvider.GetRequiredService<ISolarPanelService>();

                SolarOptimizationScheduleConfiguration schedule =
                    await scheduleRepository.GetAsync(stoppingToken);
                intervalMinutes = schedule.IntervalMinutes;

                IReadOnlyList<int> solarPanelIds =
                    await stateRepository.ListEnabledSolarPanelIdsAsync(stoppingToken);

                foreach (int solarPanelId in solarPanelIds)
                {
                    Result result = await OptimizeAsync(solarPanelService, solarPanelId, stoppingToken);
                    if (result.IsSuccess)
                        continue;

                    ResultError error = result.Error!.Value;
                    InfrastructureLog.AutomaticOptimizationFailed(logger, solarPanelId, error.Code, error.Message);
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
        ISolarPanelService solarPanelService,
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        Result<SolarPanelCurrentPositionDto> result =
            await solarPanelService.MoveToOptimumAsync(solarPanelId, cancellationToken);
        return result.IsSuccess
            ? Result.Success()
            : result.IsNotFound
                ? Result.NotFound(result.Error!.Value.Code, result.Error.Value.Message)
                : Result.Failure(result.Error!.Value.Code, result.Error.Value.Message);
    }
}
