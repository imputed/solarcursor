using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Api.Infrastructure;

namespace SolarTracker.Api.Endpoints.SolarOptimizationScheduleConfiguration;

internal static class SolarOptimizationScheduleConfigurationHandlers
{
    internal static async Task<Ok<SolarOptimizationScheduleConfigurationDto>> GetAsync(
        ISolarOptimizationScheduleConfigurationService service,
        CancellationToken cancellationToken) =>
        TypedResults.Ok(await service.GetAsync(cancellationToken));

    internal static async Task<Results<Ok<SolarOptimizationScheduleConfigurationDto>, ValidationProblem>> PutAsync(
        UpdateSolarOptimizationScheduleConfigurationDto dto,
        IValidator<UpdateSolarOptimizationScheduleConfigurationDto> validator,
        ISolarOptimizationScheduleConfigurationService service,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        return TypedResults.Ok(await service.UpdateAsync(dto, cancellationToken));
    }
}
