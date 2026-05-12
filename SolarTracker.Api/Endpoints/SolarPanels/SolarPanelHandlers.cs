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

namespace SolarTracker.Api.Endpoints.SolarPanels;

internal static class SolarPanelHandlers
{
    internal static async Task<Results<Ok<IReadOnlyList<SolarPanelDto>>, ValidationProblem>> AnalyzeAsync(
        SolarPanelAnalyzeRequest body,
        IValidator<SolarPanelAnalyzeRequest> validator,
        ISolarPanelQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(body, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        IReadOnlyList<SolarPanel> entities = await queryHandler.AnalyzeAsync(body, cancellationToken);
        IReadOnlyList<SolarPanelDto> dtos = entities.Select(SolarPanelMapping.ToDto).ToList();
        return TypedResults.Ok(dtos);
    }

    internal static async Task<Results<Ok<SolarPanelDto>, NotFound>> GetByIdAsync(
        int id,
        ISolarPanelQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        SolarPanel? entity = await queryHandler.GetByIdAsync(id, cancellationToken);
        return entity is null ? TypedResults.NotFound() : TypedResults.Ok(SolarPanelMapping.ToDto(entity));
    }

    internal static async Task<Results<Ok<SolarPanelCurrentPositionDto>, NotFound>> GetCurrentPositionAsync(
        int id,
        ISolarPanelService service,
        CancellationToken cancellationToken)
    {
        Result<SolarPanelCurrentPositionDto> result = await service.GetCurrentPositionAsync(id, cancellationToken);
        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.NotFound();
    }

    internal static async Task<Results<Created<SolarPanelDto>, ValidationProblem, ProblemHttpResult>> CreateAsync(
        CreateSolarPanelDto dto,
        IValidator<CreateSolarPanelDto> validator,
        ISolarPanelService service,
        ISolarPanelQueryHandler queryHandler,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        int newId = await service.AddAsync(dto, cancellationToken);
        SolarPanel? created = await queryHandler.GetByIdAsync(newId, cancellationToken);
        if (created is null)
        {
            ILogger logger = loggerFactory.CreateLogger(typeof(SolarPanelHandlers).FullName!);
            ApiLog.CreatePersistenceReadFailed(logger, ApiProblemCatalog.SolarPanelEntityName, newId);
            var problem = ApiProblemCatalog.SolarPanelPersistenceFailed(newId);
            return TypedResults.Problem(
                title: problem.Title,
                detail: problem.Detail,
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return TypedResults.Created(ApiRouteCatalog.SolarPanelById(newId), SolarPanelMapping.ToDto(created));
    }

    internal static async Task<Results<NoContent, NotFound, ValidationProblem>> PutAsync(
        int id,
        UpdateSolarPanelDto dto,
        IValidator<UpdateSolarPanelDto> validator,
        ISolarPanelService service,
        ISolarPanelQueryHandler queryHandler,
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
        ISolarPanelService service,
        ISolarPanelQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        if (await queryHandler.GetByIdAsync(id, cancellationToken) is null)
            return TypedResults.NotFound();

        await service.DeleteAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
