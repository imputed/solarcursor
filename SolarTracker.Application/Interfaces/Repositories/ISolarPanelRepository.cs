using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface ISolarPanelRepository
{
    ValueTask AddAsync(SolarPanel entity, CancellationToken cancellationToken);

    ValueTask UpdateAsync(SolarPanel entity, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
