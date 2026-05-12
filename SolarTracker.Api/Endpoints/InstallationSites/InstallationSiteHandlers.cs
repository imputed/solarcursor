using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Api.Infrastructure;
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

    internal static async Task<Results<Created<InstallationSiteDto>, ValidationProblem>> CreateAsync(
        CreateInstallationSiteDto dto,
        IValidator<CreateInstallationSiteDto> validator,
        IInstallationSiteService service,
        IInstallationSiteQueryHandler queryHandler,
        CancellationToken cancellationToken)
    {
        FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return validation.ToValidationProblem();

        int newId = await service.AddAsync(dto, cancellationToken);
        InstallationSite? created = await queryHandler.GetByIdAsync(newId, cancellationToken);
        if (created is null)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["_"] = ["Entity was not persisted."] });
        }

        return TypedResults.Created($"/api/installation-sites/{newId}", InstallationSiteMapping.ToDto(created));
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
