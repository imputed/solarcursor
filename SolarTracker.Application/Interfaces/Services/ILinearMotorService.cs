using SolarTracker.Application.Dtos;

namespace SolarTracker.Application.Interfaces.Services;

public interface ILinearMotorService
{
    ValueTask<int> AddAsync(CreateLinearMotorDto dto, CancellationToken cancellationToken);

    ValueTask UpdateAsync(UpdateLinearMotorDto dto, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
