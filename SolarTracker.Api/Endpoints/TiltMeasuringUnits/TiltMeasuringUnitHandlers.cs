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

    internal static async Task<Results<Created<TiltMeasuringUnitDto>, ValidationProblem, ProblemHttpResult>> CreateAsync(
        CreateTiltMeasuringUnitDto dto,
        IValidator<CreateTiltMeasuringUnitDto> validator,
        ITiltMeasuringUnitService service,
        ITiltMeasuringUnitQueryHandler queryHandler,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        int newId = await service.AddAsync(dto, cancellationToken);
        TiltMeasuringUnit? created = await queryHandler.GetByIdAsync(newId, cancellationToken);
        if (created is null)
        {
            ILogger logger = loggerFactory.CreateLogger(typeof(TiltMeasuringUnitHandlers).FullName!);
            ApiLog.CreatePersistenceReadFailed(logger, ApiProblemCatalog.TiltMeasuringUnitEntityName, newId);
            var problem = ApiProblemCatalog.TiltMeasuringUnitPersistenceFailed(newId);
            return TypedResults.Problem(
                title: problem.Title,
                detail: problem.Detail,
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return TypedResults.Created(ApiRouteCatalog.TiltMeasuringUnitById(newId), TiltMeasuringUnitMapping.ToDto(created));
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
