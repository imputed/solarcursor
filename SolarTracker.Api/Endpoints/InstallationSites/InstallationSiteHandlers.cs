using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Results;
using SolarTracker.Api.Errors;
using SolarTracker.Api.Infrastructure;
using SolarTracker.Api.Logging;
using SolarTracker.Api.Routing;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Api.Endpoints.InstallationSites;

internal static class InstallationSiteHandlers
{
    internal static async Task<Results<Ok<IReadOnlyList<InstallationSiteDto>>, ValidationProblem>> AnalyzeAsync(
        InstallationSiteAnalyzeRequest body,
        IValidator<InstallationSiteAnalyzeRequest> validator,
        IInstallationSiteQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(body, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        IReadOnlyList<InstallationSite> entities = await queryHandler.AnalyzeAsync(body, cancellationToken);
        IReadOnlyList<InstallationSiteDto> dtos = entities.Select(InstallationSiteMapping.ToDto).ToList();
        return TypedResults.Ok(dtos);
    }

    internal static async Task<Results<Ok<InstallationSiteDto>, NotFound>> GetByIdAsync(
        int id,
        IInstallationSiteQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        InstallationSite? entity = await queryHandler.GetByIdAsync(id, cancellationToken);
        return entity is null ? TypedResults.NotFound() : TypedResults.Ok(InstallationSiteMapping.ToDto(entity));
    }

    internal static async Task<Results<Ok<IReadOnlyList<SolarPanelCurrentPositionDto>>, NotFound, ProblemHttpResult>> MoveToOptimumAsync(
        int id,
        IInstallationSiteService service,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<SolarPanelCurrentPositionDto>> result = await service.MoveToOptimumAsync(id, cancellationToken);
        if (result.IsSuccess)
            return TypedResults.Ok(result.Value);

        if (result.IsNotFound)
            return TypedResults.NotFound();

        ResultError error = result.Error!.Value;
        ILogger logger = loggerFactory.CreateLogger(typeof(InstallationSiteHandlers).FullName!);
        ApiLog.InstallationSiteMoveToOptimumConflict(logger, id, error.Code, error.Message);
        var problem = ApiProblemCatalog.InstallationSiteMovementFailed(error.Message);
        return TypedResults.Problem(
            title: problem.Title,
            detail: problem.Detail,
            statusCode: StatusCodes.Status409Conflict);
    }

    internal static async Task<Results<Created<InstallationSiteDto>, ValidationProblem, ProblemHttpResult>> CreateAsync(
        CreateInstallationSiteDto dto,
        IValidator<CreateInstallationSiteDto> validator,
        IInstallationSiteService service,
        IInstallationSiteQueryHandler queryHandler,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        int newId = await service.AddAsync(dto, cancellationToken);
        InstallationSite? created = await queryHandler.GetByIdAsync(newId, cancellationToken);
        if (created is null)
        {
            ILogger logger = loggerFactory.CreateLogger(typeof(InstallationSiteHandlers).FullName!);
            ApiLog.CreatePersistenceReadFailed(logger, ApiProblemCatalog.InstallationSiteEntityName, newId);
            var problem = ApiProblemCatalog.InstallationSitePersistenceFailed(newId);
            return TypedResults.Problem(
                title: problem.Title,
                detail: problem.Detail,
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return TypedResults.Created(ApiRouteCatalog.InstallationSiteById(newId), InstallationSiteMapping.ToDto(created));
    }

    internal static async Task<Results<NoContent, NotFound, ValidationProblem>> PutAsync(
        int id,
        UpdateInstallationSiteDto dto,
        IValidator<UpdateInstallationSiteDto> validator,
        IInstallationSiteService service,
        IInstallationSiteQueryHandler queryHandler,
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
        IInstallationSiteService service,
        IInstallationSiteQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        if (await queryHandler.GetByIdAsync(id, cancellationToken) is null)
            return TypedResults.NotFound();

        await service.DeleteAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
