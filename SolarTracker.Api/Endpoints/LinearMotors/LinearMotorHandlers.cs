using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Api.Infrastructure;

namespace SolarTracker.Api.Endpoints.LinearMotors;

internal static class LinearMotorHandlers
{
    internal static async Task<Ok<IReadOnlyList<LinearMotorDto>>> ListAsync(
        ILinearMotorService service,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<LinearMotorDto> list = await service.ListAsync(cancellationToken);
        return TypedResults.Ok(list);
    }

    internal static async Task<Results<Ok<IReadOnlyList<LinearMotorDto>>, ValidationProblem>> AnalyzeAsync(
        LinearMotorAnalyzeRequest body,
        IValidator<LinearMotorAnalyzeRequest> validator,
        ILinearMotorService service,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(body, cancellationToken);
        if (!validation.IsValid)
        {
            return validation.ToValidationProblem();
        }

        IReadOnlyList<LinearMotorDto> result = await service.AnalyzeAsync(body, cancellationToken);
        return TypedResults.Ok(result);
    }

    internal static async Task<Results<Ok<LinearMotorDto>, NotFound>> GetByIdAsync(
        int id,
        ILinearMotorService service,
        CancellationToken cancellationToken)
    {
        LinearMotorDto? dto = await service.GetByIdAsync(id, cancellationToken);
        return dto is null ? TypedResults.NotFound() : TypedResults.Ok(dto);
    }

    internal static async Task<Results<Created<LinearMotorDto>, ValidationProblem>> CreateAsync(
        CreateLinearMotorDto dto,
        IValidator<CreateLinearMotorDto> validator,
        ILinearMotorService service,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
        {
            return validation.ToValidationProblem();
        }

        int newId = await service.AddAsync(dto, cancellationToken);
        LinearMotorDto? created = await service.GetByIdAsync(newId, cancellationToken);
        if (created is null)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["_"] = ["Entity was not persisted."] });
        }

        return TypedResults.Created($"/api/linear-motors/{newId}", created);
    }

    internal static async Task<Results<NoContent, NotFound, ValidationProblem>> PutAsync(
        int id,
        UpdateLinearMotorDto dto,
        IValidator<UpdateLinearMotorDto> validator,
        ILinearMotorService service,
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

        if (await service.GetByIdAsync(id, cancellationToken) is null)
        {
            return TypedResults.NotFound();
        }

        await service.UpdateAsync(dto, cancellationToken);
        return TypedResults.NoContent();
    }

    internal static async Task<Results<NoContent, NotFound>> DeleteAsync(
        int id,
        ILinearMotorService service,
        CancellationToken cancellationToken)
    {
        if (await service.GetByIdAsync(id, cancellationToken) is null)
        {
            return TypedResults.NotFound();
        }

        await service.DeleteAsync(id, cancellationToken);
        return TypedResults.NoContent();
    }
}
