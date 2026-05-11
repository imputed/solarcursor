using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Endpoints.SolarTrackingConfiguration;

internal static class SolarTrackingConfigurationEndpoints
{
    internal static IEndpointRouteBuilder MapSolarTrackingConfigurationEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/solar-panels/{solarPanelId:int}/tracking-configuration")
            .WithTags("SolarTrackingConfiguration");

        group.MapGet("/", SolarTrackingConfigurationHandlers.GetAsync)
            .WithName("GetSolarTrackingConfiguration")
            .Produces<SolarTrackingConfigurationDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/", SolarTrackingConfigurationHandlers.PutAsync)
            .WithName("UpdateSolarTrackingConfiguration")
            .Produces<SolarTrackingConfigurationDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        return app;
    }
}
