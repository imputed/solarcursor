using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Endpoints.LinearMotors;

internal static class LinearMotorEndpoints
{
    internal static IEndpointRouteBuilder MapLinearMotorEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/api/linear-motors")
            .WithTags("LinearMotors");

        group.MapPost("/analyze", LinearMotorHandlers.AnalyzeAsync)
            .WithName("AnalyzeLinearMotors")
            .Produces<IReadOnlyList<LinearMotorDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:int}", LinearMotorHandlers.GetByIdAsync)
            .WithName("GetLinearMotorById")
            .Produces<LinearMotorDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", LinearMotorHandlers.CreateAsync)
            .WithName("CreateLinearMotor")
            .Produces<LinearMotorDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", LinearMotorHandlers.PutAsync)
            .WithName("UpdateLinearMotor")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", LinearMotorHandlers.DeleteAsync)
            .WithName("DeleteLinearMotor")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
