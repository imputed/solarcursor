using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Api.Errors;
using SolarTracker.Api.Infrastructure;
using SolarTracker.Api.Logging;
using SolarTracker.Api.Routing;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Api.Endpoints.CurrentMeasuringUnits;

internal static class CurrentMeasuringUnitHandlers
{
    internal static async Task<Results<Ok<IReadOnlyList<CurrentMeasuringUnitDto>>, ValidationProblem>> AnalyzeAsync(
        CurrentMeasuringUnitAnalyzeRequest body,
        IValidator<CurrentMeasuringUnitAnalyzeRequest> validator,
        ICurrentMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(body, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

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

    internal static async Task<Results<Created<CurrentMeasuringUnitDto>, ValidationProblem, ProblemHttpResult>> CreateAsync(
        CreateCurrentMeasuringUnitDto dto,
        IValidator<CreateCurrentMeasuringUnitDto> validator,
        ICurrentMeasuringUnitService service,
        ICurrentMeasuringUnitQueryHandler queryHandler,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        int newId = await service.AddAsync(dto, cancellationToken);
        CurrentMeasuringUnit? created = await queryHandler.GetByIdAsync(newId, cancellationToken);
        if (created is null)
        {
            ILogger logger = loggerFactory.CreateLogger(typeof(CurrentMeasuringUnitHandlers).FullName!);
            ApiLog.CreatePersistenceReadFailed(logger, ApiProblemCatalog.CurrentMeasuringUnitEntityName, newId);
            var problem = ApiProblemCatalog.CurrentMeasuringUnitPersistenceFailed(newId);
            return TypedResults.Problem(
                title: problem.Title,
                detail: problem.Detail,
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return TypedResults.Created(ApiRouteCatalog.CurrentMeasuringUnitById(newId), CurrentMeasuringUnitMapping.ToDto(created));
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
            return TypedResults.ValidationProblem(ApiProblemCatalog.RouteIdMustEqualBodyId());

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
        ICurrentMeasuringUnitService service,
        ICurrentMeasuringUnitQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        if (await queryHandler.GetByIdAsync(id, cancellationToken) is null)
            return TypedResults.NotFound();

        await service.DeleteAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
