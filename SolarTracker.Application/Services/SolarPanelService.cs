using Microsoft.Extensions.Logging;
using SolarTracker.Application.Dtos.SolarPanel;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Logging;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Services;

public sealed class SolarPanelService(
    ISolarPanelRepository repository,
    ILogger<SolarPanelService> logger) : ISolarPanelService
{
    public async ValueTask<int> AddAsync(CreateSolarPanelDto dto, CancellationToken cancellationToken)
    {
        SolarPanel entity = SolarPanelMapping.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        ApplicationLog.CreatedSolarPanel(logger, entity.Id, entity.InstallationSiteId);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateSolarPanelDto dto, CancellationToken cancellationToken)
    {
        SolarPanel entity = SolarPanelMapping.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
        ApplicationLog.UpdatedSolarPanel(logger, entity.Id);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
        ApplicationLog.DeletedSolarPanel(logger, id);
    }
}
