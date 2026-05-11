using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Api.Infrastructure;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Api.Endpoints.CurrentMeasuringUnits;

internal static class CurrentMeasuringUnitHandlers
{
    internal static async Task<Ok<IReadOnlyList<CurrentMeasuringUnitDto>>> GetCollectionAsync(
        ICurrentMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<CurrentMeasuringUnit> entities = await queryHandler.AnalyzeAsync(CreateDefaultAnalyzeRequest(), cancellationToken);
        IReadOnlyList<CurrentMeasuringUnitDto> dtos = entities.Select(CurrentMeasuringUnitMapping.ToDto).ToList();
        return TypedResults.Ok(dtos);
    }

    internal static async Task<Results<Ok<IReadOnlyList<CurrentMeasuringUnitDto>>, ValidationProblem>> AnalyzeAsync(
        CurrentMeasuringUnitAnalyzeRequest body,
        IValidator<CurrentMeasuringUnitAnalyzeRequest> validator,
        ICurrentMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(body, cancellationToken);
        if (!validation.IsValid)
        {
            return validation.ToValidationProblem();
        }

        IReadOnlyList<CurrentMeasuringUnit> entities = await queryHandler.AnalyzeAsync(body, cancellationToken);
        IReadOnlyList<CurrentMeasuringUnitDto> dtos = entities.Select(CurrentMeasuringUnitMapping.ToDto).ToList();
        return TypedResults.Ok(dtos);
    }

    internal static async Task<Results<Ok<CurrentMeasuringUnitDto>, NotFound>> GetByIdAsync(
        int id,
        ICurrentMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        CurrentMeasuringUnit? entity = await queryHandler.GetByIdAsync(id, cancellationToken);
        return entity is null ? TypedResults.NotFound() : TypedResults.Ok(CurrentMeasuringUnitMapping.ToDto(entity));
    }

    internal static async Task<Results<Created<CurrentMeasuringUnitDto>, ValidationProblem>> CreateAsync(
        CreateCurrentMeasuringUnitDto dto,
        IValidator<CreateCurrentMeasuringUnitDto> validator,
        ICurrentMeasuringUnitService service,
        ICurrentMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
        {
            return validation.ToValidationProblem();
        }

        int newId = await service.AddAsync(dto, cancellationToken);
        CurrentMeasuringUnit? created = await queryHandler.GetByIdAsync(newId, cancellationToken);
        if (created is null)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["_"] = ["Entity was not persisted."] });
        }

        return TypedResults.Created($"/api/current-measuring-units/{newId}", CurrentMeasuringUnitMapping.ToDto(created));
    }

    internal static async Task<Results<NoContent, NotFound, ValidationProblem>> PutAsync(
        int id,
        UpdateCurrentMeasuringUnitDto dto,
        IValidator<UpdateCurrentMeasuringUnitDto> validator,
        ICurrentMeasuringUnitService service,
        ICurrentMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["id"] = ["Route id must equal body Id."] });
        }

        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
        {
            return validation.ToValidationProblem();
        }

        if (await queryHandler.GetByIdAsync(id, cancellationToken) is null)
        {
            return TypedResults.NotFound();
        }

        await service.UpdateAsync(dto, cancellationToken);
        return TypedResults.NoContent();
    }

    internal static async Task<Results<NoContent, NotFound>> DeleteAsync(
        int id,
        ICurrentMeasuringUnitService service,
        ICurrentMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        if (await queryHandler.GetByIdAsync(id, cancellationToken) is null)
        {
            return TypedResults.NotFound();
        }

        await service.DeleteAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }

    private static CurrentMeasuringUnitAnalyzeRequest CreateDefaultAnalyzeRequest() =>
        new(Filter: null, SortBy: null);
}
