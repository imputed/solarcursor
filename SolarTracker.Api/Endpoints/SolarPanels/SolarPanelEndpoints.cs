using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Endpoints.SolarPanels;

internal static class SolarPanelEndpoints
{
    internal static IEndpointRouteBuilder MapSolarPanelEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/solar-panels")
            .WithTags("SolarPanels");

        group.MapGet("/", SolarPanelHandlers.ListAsync)
            .WithName("ListSolarPanels")
            .Produces<IReadOnlyList<SolarPanelDto>>(StatusCodes.Status200OK);

        group.MapPost("/analyze", SolarPanelHandlers.AnalyzeAsync)
            .WithName("AnalyzeSolarPanels")
            .Produces<IReadOnlyList<SolarPanelDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:int}", SolarPanelHandlers.GetByIdAsync)
            .WithName("GetSolarPanelById")
            .Produces<SolarPanelDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", SolarPanelHandlers.CreateAsync)
            .WithName("CreateSolarPanel")
            .Produces<SolarPanelDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", SolarPanelHandlers.PutAsync)
            .WithName("UpdateSolarPanel")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", SolarPanelHandlers.DeleteAsync)
            .WithName("DeleteSolarPanel")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
