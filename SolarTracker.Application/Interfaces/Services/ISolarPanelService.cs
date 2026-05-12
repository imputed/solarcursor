using SolarTracker.Application.Dtos.SolarPanel;

namespace SolarTracker.Application.Interfaces.Services;

public interface ISolarPanelService
{
    ValueTask<int> AddAsync(CreateSolarPanelDto dto, CancellationToken cancellationToken);

    ValueTask UpdateAsync(UpdateSolarPanelDto dto, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
