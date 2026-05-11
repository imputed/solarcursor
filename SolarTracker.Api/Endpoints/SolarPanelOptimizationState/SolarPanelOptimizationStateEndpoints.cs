using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Endpoints.SolarPanelOptimizationState;

internal static class SolarPanelOptimizationStateEndpoints
{
    internal static IEndpointRouteBuilder MapSolarPanelOptimizationStateEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/solar-panels/{solarPanelId:int}/optimization-state")
            .WithTags("SolarPanelOptimizationState");

        group.MapGet("/", SolarPanelOptimizationStateHandlers.GetAsync)
            .WithName("GetSolarPanelOptimizationState")
            .Produces<SolarPanelOptimizationStateDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/", SolarPanelOptimizationStateHandlers.PutAsync)
            .WithName("UpdateSolarPanelOptimizationState")
            .Produces<SolarPanelOptimizationStateDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
