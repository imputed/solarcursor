using Microsoft.AspNetCore.Http.HttpResults;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;

namespace SolarTracker.Api.Endpoints.SolarPanelOptimizationState;

internal static class SolarPanelOptimizationStateHandlers
{
    internal static async Task<Results<Ok<SolarPanelOptimizationStateDto>, NotFound>> GetAsync(
        int solarPanelId,
        ISolarPanelOptimizationStateService service,
        CancellationToken cancellationToken)
    {
        Result<SolarPanelOptimizationStateDto> result = await service.GetAsync(solarPanelId, cancellationToken);
        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<SolarPanelOptimizationStateDto>, NotFound>> PutAsync(
        int solarPanelId,
        UpdateSolarPanelOptimizationStateDto dto,
        ISolarPanelOptimizationStateService service,
        CancellationToken cancellationToken)
    {
        Result<SolarPanelOptimizationStateDto> result =
            await service.UpdateAsync(solarPanelId, dto, cancellationToken);
        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.NotFound();
    }
}
