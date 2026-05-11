using SolarTracker.Application.Dtos;

namespace SolarTracker.Application.Interfaces.Services;

public interface ITiltMeasuringUnitService
{
    ValueTask<int> AddAsync(CreateTiltMeasuringUnitDto dto, CancellationToken cancellationToken);

    ValueTask UpdateAsync(UpdateTiltMeasuringUnitDto dto, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
