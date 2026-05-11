using SolarTracker.Application.Dtos;

namespace SolarTracker.Application.Interfaces.Services;

public interface ILinearMotorMovementService
{
    ValueTask<bool> MoveUpAsync(int linearMotorId, LinearMotorMoveRequest request, CancellationToken cancellationToken);

    ValueTask<bool> MoveDownAsync(int linearMotorId, LinearMotorMoveRequest request, CancellationToken cancellationToken);
}
