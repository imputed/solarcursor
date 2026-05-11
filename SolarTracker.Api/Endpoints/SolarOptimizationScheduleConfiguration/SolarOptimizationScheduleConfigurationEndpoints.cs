using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Endpoints.SolarOptimizationScheduleConfiguration;

internal static class SolarOptimizationScheduleConfigurationEndpoints
{
    internal static IEndpointRouteBuilder MapSolarOptimizationScheduleConfigurationEndpoints(
        this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/solar-optimization-schedule")
            .WithTags("SolarOptimizationScheduleConfiguration");

        group.MapGet("/", SolarOptimizationScheduleConfigurationHandlers.GetAsync)
            .WithName("GetSolarOptimizationScheduleConfiguration")
            .Produces<SolarOptimizationScheduleConfigurationDto>(StatusCodes.Status200OK);

        group.MapPut("/", SolarOptimizationScheduleConfigurationHandlers.PutAsync)
            .WithName("UpdateSolarOptimizationScheduleConfiguration")
            .Produces<SolarOptimizationScheduleConfigurationDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        return app;
    }
}
