using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Calculators;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class SolarPanelService(
    ISolarPanelRepository repository,
    ISolarmoduleCalculator solarmoduleCalculator) : ISolarPanelService
{
    public async ValueTask<int> AddAsync(CreateSolarPanelDto dto, CancellationToken cancellationToken)
    {
        SolarPanel entity = SolarPanelMapping.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateSolarPanelDto dto, CancellationToken cancellationToken)
    {
        SolarPanel entity = SolarPanelMapping.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
    }

    public ValueTask<Result<SolarPanelCurrentPositionDto>> GetCurrentPositionAsync(
        int id,
        CancellationToken cancellationToken) =>
        solarmoduleCalculator.GetCurrentPositionAsync(id, cancellationToken);

    public ValueTask<Result<SolarPanelCurrentPositionDto>> MoveToOptimumAsync(
        int id,
        CancellationToken cancellationToken) =>
        solarmoduleCalculator.MoveToOptimumAsync(id, cancellationToken);

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
        => await repository.DeleteAsync(id, cancellationToken);
}
