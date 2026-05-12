using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Api.Infrastructure;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Api.Endpoints.TiltMeasuringUnits;

internal static class TiltMeasuringUnitHandlers
{
    internal static async Task<Results<Ok<IReadOnlyList<TiltMeasuringUnitDto>>, ValidationProblem>> AnalyzeAsync(
        TiltMeasuringUnitAnalyzeRequest body,
        IValidator<TiltMeasuringUnitAnalyzeRequest> validator,
        ITiltMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(body, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        IReadOnlyList<TiltMeasuringUnit> entities = await queryHandler.AnalyzeAsync(body, cancellationToken);
        IReadOnlyList<TiltMeasuringUnitDto> dtos = entities.Select(TiltMeasuringUnitMapping.ToDto).ToList();
        return TypedResults.Ok(dtos);
    }

    internal static async Task<Results<Ok<TiltMeasuringUnitDto>, NotFound>> GetByIdAsync(
        int id,
        ITiltMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        TiltMeasuringUnit? entity = await queryHandler.GetByIdAsync(id, cancellationToken);
        return entity is null ? TypedResults.NotFound() : TypedResults.Ok(TiltMeasuringUnitMapping.ToDto(entity));
    }

    internal static async Task<Results<Created<TiltMeasuringUnitDto>, ValidationProblem>> CreateAsync(
        CreateTiltMeasuringUnitDto dto,
        IValidator<CreateTiltMeasuringUnitDto> validator,
        ITiltMeasuringUnitService service,
        ITiltMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        int newId = await service.AddAsync(dto, cancellationToken);
        TiltMeasuringUnit? created = await queryHandler.GetByIdAsync(newId, cancellationToken);
        if (created is null)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["_"] = ["Entity was not persisted."] });
        }

        return TypedResults.Created($"/api/tilt-measuring-units/{newId}", TiltMeasuringUnitMapping.ToDto(created));
    }

    internal static async Task<Results<NoContent, NotFound, ValidationProblem>> PutAsync(
        int id,
        UpdateTiltMeasuringUnitDto dto,
        IValidator<UpdateTiltMeasuringUnitDto> validator,
        ITiltMeasuringUnitService service,
        ITiltMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["id"] = ["Route id must equal body Id."] });
        }

        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        if (await queryHandler.GetByIdAsync(id, cancellationToken) is null)
            return TypedResults.NotFound();

        await service.UpdateAsync(dto, cancellationToken);
        return TypedResults.NoContent();
    }

    internal static async Task<Results<NoContent, NotFound>> DeleteAsync(
        int id,
        ITiltMeasuringUnitService service,
        ITiltMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        if (await queryHandler.GetByIdAsync(id, cancellationToken) is null)
            return TypedResults.NotFound();
        await service.DeleteAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
