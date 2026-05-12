using SolarTracker.Application.Dtos.TiltMeasuringUnit;

namespace SolarTracker.Api.Endpoints.TiltMeasuringUnits;

internal static class TiltMeasuringUnitEndpoints
{
    internal static IEndpointRouteBuilder MapTiltMeasuringUnitEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/tilt-measuring-units")
            .WithTags("TiltMeasuringUnits");

        group.MapPost("/analyze", TiltMeasuringUnitHandlers.AnalyzeAsync)
            .WithName("AnalyzeTiltMeasuringUnits")
            .Produces<IReadOnlyList<TiltMeasuringUnitDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:int}", TiltMeasuringUnitHandlers.GetByIdAsync)
            .WithName("GetTiltMeasuringUnitById")
            .Produces<TiltMeasuringUnitDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", TiltMeasuringUnitHandlers.CreateAsync)
            .WithName("CreateTiltMeasuringUnit")
            .Produces<TiltMeasuringUnitDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", TiltMeasuringUnitHandlers.PutAsync)
            .WithName("UpdateTiltMeasuringUnit")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", TiltMeasuringUnitHandlers.DeleteAsync)
            .WithName("DeleteTiltMeasuringUnit")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
