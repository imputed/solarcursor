using SolarTracker.Application.Dtos.CurrentMeasuringUnit;

namespace SolarTracker.Api.Endpoints.CurrentMeasuringUnits;

internal static class CurrentMeasuringUnitEndpoints
{
    internal static IEndpointRouteBuilder MapCurrentMeasuringUnitEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/current-measuring-units")
            .WithTags("CurrentMeasuringUnits");

        group.MapPost("/analyze", CurrentMeasuringUnitHandlers.AnalyzeAsync)
            .WithName("AnalyzeCurrentMeasuringUnits")
            .Produces<IReadOnlyList<CurrentMeasuringUnitDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:int}", CurrentMeasuringUnitHandlers.GetByIdAsync)
            .WithName("GetCurrentMeasuringUnitById")
            .Produces<CurrentMeasuringUnitDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CurrentMeasuringUnitHandlers.CreateAsync)
            .WithName("CreateCurrentMeasuringUnit")
            .Produces<CurrentMeasuringUnitDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", CurrentMeasuringUnitHandlers.PutAsync)
            .WithName("UpdateCurrentMeasuringUnit")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", CurrentMeasuringUnitHandlers.DeleteAsync)
            .WithName("DeleteCurrentMeasuringUnit")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
