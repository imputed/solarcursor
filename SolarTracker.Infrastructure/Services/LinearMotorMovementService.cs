using Microsoft.Extensions.Logging;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Logging;

namespace SolarTracker.Infrastructure.Services;

public sealed class LinearMotorMovementService(
    ILinearMotorQueryHandler linearMotorQueryHandler,
    ISteeringCommandReceiver steeringCommandReceiver,
    ILogger<LinearMotorMovementService> logger)
{
    public async ValueTask<Result> MoveUpAsync(
        int linearMotorId,
        int durationMs,
        CancellationToken cancellationToken)
    {
        Result<LinearMotor> linearMotorResult = await GetLinearMotorAsync(linearMotorId, cancellationToken);
        if (!linearMotorResult.IsSuccess)
        {
            ResultError error = linearMotorResult.Error!.Value;
            return Result.NotFound(error.Code, error.Message);
        }

        await ExecuteMovementAsync(
            linearMotorResult.Value,
            static (linearMotor, receiver, token) => linearMotor.MoveUpAsync(receiver, token),
            durationMs,
            cancellationToken);
        return Result.Success();
    }

    public async ValueTask<Result> MoveDownAsync(
        int linearMotorId,
        int durationMs,
        CancellationToken cancellationToken)
    {
        Result<LinearMotor> linearMotorResult = await GetLinearMotorAsync(linearMotorId, cancellationToken);
        if (!linearMotorResult.IsSuccess)
        {
            ResultError error = linearMotorResult.Error!.Value;
            return Result.NotFound(error.Code, error.Message);
        }

        await ExecuteMovementAsync(
            linearMotorResult.Value,
            static (linearMotor, receiver, token) => linearMotor.MoveDownAsync(receiver, token),
            durationMs,
            cancellationToken);
        return Result.Success();
    }

    private async Task ExecuteMovementAsync(
        LinearMotor linearMotor,
        Func<LinearMotor, ISteeringCommandReceiver, CancellationToken, Task> startMovement,
        int durationMs,
        CancellationToken cancellationToken)
    {
        await startMovement(linearMotor, steeringCommandReceiver, cancellationToken);

        try
        {
            await Task.Delay(durationMs, cancellationToken);
        }
        finally
        {
            await linearMotor.StopAsync(steeringCommandReceiver, CancellationToken.None);
        }
    }

    private async ValueTask<Result<LinearMotor>> GetLinearMotorAsync(
        int linearMotorId,
        CancellationToken cancellationToken)
    {
        var linearMotor = await linearMotorQueryHandler.GetByIdAsync(linearMotorId, cancellationToken);
        if (linearMotor is null)
        {
            InfrastructureLog.LinearMotorNotFound(logger, linearMotorId);
            return Result<LinearMotor>.NotFound(SolarTrackerErrorCatalog.LinearMotor.NotFound(linearMotorId));
        }

        return Result<LinearMotor>.Success(linearMotor);
    }
}
