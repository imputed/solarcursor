using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;
using SolarTracker.Api.Infrastructure;

namespace SolarTracker.Api.Endpoints.SolarTrackingConfiguration;

internal static class SolarTrackingConfigurationHandlers
{
    internal static async Task<Results<Ok<SolarTrackingConfigurationDto>, NotFound>> GetAsync(
        int solarPanelId,
        ISolarTrackingConfigurationService service,
        CancellationToken cancellationToken)
    {
        Result<SolarTrackingConfigurationDto> result = await service.GetAsync(solarPanelId, cancellationToken);
        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.NotFound();
    }

    internal static async Task<Results<Ok<SolarTrackingConfigurationDto>, NotFound, ValidationProblem>> PutAsync(
        int solarPanelId,
        UpdateSolarTrackingConfigurationDto dto,
        IValidator<UpdateSolarTrackingConfigurationDto> validator,
        ISolarTrackingConfigurationService service,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        Result<SolarTrackingConfigurationDto> result =
            await service.UpdateAsync(solarPanelId, dto, cancellationToken);
        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.NotFound();
    }
}
