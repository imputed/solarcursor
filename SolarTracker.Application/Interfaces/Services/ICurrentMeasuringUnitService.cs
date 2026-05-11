using SolarTracker.Application.Dtos;

namespace SolarTracker.Application.Interfaces.Services;

public interface ICurrentMeasuringUnitService
{
    ValueTask<int> AddAsync(CreateCurrentMeasuringUnitDto dto, CancellationToken cancellationToken);

    ValueTask UpdateAsync(UpdateCurrentMeasuringUnitDto dto, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
