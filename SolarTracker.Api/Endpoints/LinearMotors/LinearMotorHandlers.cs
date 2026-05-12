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

namespace SolarTracker.Api.Endpoints.LinearMotors;

internal static class LinearMotorHandlers
{
    internal static async Task<Results<Ok<IReadOnlyList<LinearMotorDto>>, ValidationProblem>> AnalyzeAsync(
        LinearMotorAnalyzeRequest body,
        IValidator<LinearMotorAnalyzeRequest> validator,
        ILinearMotorQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(body, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        IReadOnlyList<LinearMotor> entities = await queryHandler.AnalyzeAsync(body, cancellationToken);
        IReadOnlyList<LinearMotorDto> dtos = entities.Select(LinearMotorMapping.ToDto).ToList();
        return TypedResults.Ok(dtos);
    }

    internal static async Task<Results<Ok<LinearMotorDto>, NotFound>> GetByIdAsync(
        int id,
        ILinearMotorQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        LinearMotor? entity = await queryHandler.GetByIdAsync(id, cancellationToken);
        return entity is null ? TypedResults.NotFound() : TypedResults.Ok(LinearMotorMapping.ToDto(entity));
    }

    internal static async Task<Results<Created<LinearMotorDto>, ValidationProblem, ProblemHttpResult>> CreateAsync(
        CreateLinearMotorDto dto,
        IValidator<CreateLinearMotorDto> validator,
        ILinearMotorService service,
        ILinearMotorQueryHandler queryHandler,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        int newId = await service.AddAsync(dto, cancellationToken);
        LinearMotor? created = await queryHandler.GetByIdAsync(newId, cancellationToken);
        if (created is null)
        {
            ILogger logger = loggerFactory.CreateLogger(typeof(LinearMotorHandlers).FullName!);
            ApiLog.CreatePersistenceReadFailed(logger, ApiProblemCatalog.LinearMotorEntityName, newId);
            var problem = ApiProblemCatalog.LinearMotorPersistenceFailed(newId);
            return TypedResults.Problem(
                title: problem.Title,
                detail: problem.Detail,
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return TypedResults.Created(ApiRouteCatalog.LinearMotorById(newId), LinearMotorMapping.ToDto(created));
    }

    internal static async Task<Results<NoContent, NotFound, ValidationProblem>> PutAsync(
        int id,
        UpdateLinearMotorDto dto,
        IValidator<UpdateLinearMotorDto> validator,
        ILinearMotorService service,
        ILinearMotorQueryHandler queryHandler,
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
        ILinearMotorService service,
        ILinearMotorQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        if (await queryHandler.GetByIdAsync(id, cancellationToken) is null)
            return TypedResults.NotFound();

        await service.DeleteAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
