using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Results;
using SolarTracker.Api.Infrastructure;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Api.Endpoints.LinearMotors;

internal static class LinearMotorHandlers
{
    internal static async Task<Ok<IReadOnlyList<LinearMotorDto>>> GetCollectionAsync(
        ILinearMotorQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<LinearMotor> entities = await queryHandler.AnalyzeAsync(CreateDefaultAnalyzeRequest(), cancellationToken);
        IReadOnlyList<LinearMotorDto> dtos = entities.Select(LinearMotorMapping.ToDto).ToList();
        return TypedResults.Ok(dtos);
    }

    internal static async Task<Results<Ok<IReadOnlyList<LinearMotorDto>>, ValidationProblem>> AnalyzeAsync(
        LinearMotorAnalyzeRequest body,
        IValidator<LinearMotorAnalyzeRequest> validator,
        ILinearMotorQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(body, cancellationToken);
        if (!validation.IsValid)
        {
            return validation.ToValidationProblem();
        }

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

    internal static async Task<Results<Created<LinearMotorDto>, ValidationProblem>> CreateAsync(
        CreateLinearMotorDto dto,
        IValidator<CreateLinearMotorDto> validator,
        ILinearMotorService service,
        ILinearMotorQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
        {
            return validation.ToValidationProblem();
        }

        int newId = await service.AddAsync(dto, cancellationToken);
        LinearMotor? created = await queryHandler.GetByIdAsync(newId, cancellationToken);
        if (created is null)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["_"] = ["Entity was not persisted."] });
        }

        return TypedResults.Created($"/api/linear-motors/{newId}", LinearMotorMapping.ToDto(created));
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
        ILinearMotorService service,
        ILinearMotorQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        if (await queryHandler.GetByIdAsync(id, cancellationToken) is null)
        {
            return TypedResults.NotFound();
        }

        await service.DeleteAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }

    internal static async Task<Results<NoContent, NotFound, ValidationProblem>> MoveUpAsync(
        int id,
        LinearMotorMoveRequest request,
        IValidator<LinearMotorMoveRequest> validator,
        ILinearMotorMovementService service,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        Result result = await service.MoveUpAsync(id, request, cancellationToken);
        return result.IsSuccess ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    internal static async Task<Results<NoContent, NotFound, ValidationProblem>> MoveDownAsync(
        int id,
        LinearMotorMoveRequest request,
        IValidator<LinearMotorMoveRequest> validator,
        ILinearMotorMovementService service,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        Result result = await service.MoveDownAsync(id, request, cancellationToken);
        return result.IsSuccess ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static LinearMotorAnalyzeRequest CreateDefaultAnalyzeRequest() =>
        new(Filter: null, SortBy: null);
}
