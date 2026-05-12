using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Endpoints.InstallationSites;

internal static class InstallationSiteEndpoints
{
    internal static IEndpointRouteBuilder MapInstallationSiteEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/installation-sites")
            .WithTags("InstallationSites");

        group.MapPost("/analyze", InstallationSiteHandlers.AnalyzeAsync)
            .WithName("AnalyzeInstallationSites")
            .Produces<IReadOnlyList<InstallationSiteDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:int}", InstallationSiteHandlers.GetByIdAsync)
            .WithName("GetInstallationSiteById")
            .Produces<InstallationSiteDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", InstallationSiteHandlers.CreateAsync)
            .WithName("CreateInstallationSite")
            .Produces<InstallationSiteDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", InstallationSiteHandlers.PutAsync)
            .WithName("UpdateInstallationSite")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", InstallationSiteHandlers.DeleteAsync)
            .WithName("DeleteInstallationSite")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
