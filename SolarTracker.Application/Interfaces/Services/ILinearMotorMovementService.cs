using SolarTracker.Application.Dtos;
using SolarTracker.Application.Results;

namespace SolarTracker.Application.Interfaces.Services;

public interface ILinearMotorMovementService
{
    ValueTask<Result> MoveUpAsync(int linearMotorId, LinearMotorMoveRequest request, CancellationToken cancellationToken);

    ValueTask<Result> MoveDownAsync(int linearMotorId, LinearMotorMoveRequest request, CancellationToken cancellationToken);
}
