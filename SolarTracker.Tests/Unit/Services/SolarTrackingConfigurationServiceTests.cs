using Moq;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.Unit.Services;

public sealed class SolarTrackingConfigurationServiceTests
{
    [Fact]
    public async Task GetAsync_returns_not_found_when_solar_panel_does_not_exist()
    {
        Mock<ISolarTrackingConfigurationRepository> repository = new();
        Mock<ISolarPanelQueryHandler> queryHandler = new();
        queryHandler.Setup(x => x.GetByIdAsync(7, CancellationToken.None))
            .Returns(ValueTask.FromResult<SolarPanel?>(null));

        SolarTrackingConfigurationService service = new(repository.Object, queryHandler.Object);

        var result = await service.GetAsync(7, CancellationToken.None);

        Assert.True(result.IsNotFound);
        repository.Verify(x => x.GetBySolarPanelIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_persists_configuration_when_solar_panel_exists()
    {
        Mock<ISolarTrackingConfigurationRepository> repository = new();
        Mock<ISolarPanelQueryHandler> queryHandler = new();
        queryHandler.Setup(x => x.GetByIdAsync(7, CancellationToken.None))
            .Returns(ValueTask.FromResult<SolarPanel?>(new SolarPanel { Id = 7 }));
        repository.Setup(x => x.UpsertAsync(
                It.Is<SolarTrackingConfiguration>(c =>
                    c.SolarPanelId == 7 &&
                    c.PositionThresholdDegrees == 2.5d &&
                    c.StepDurationMs == 700 &&
                    c.MaxAdjustmentSteps == 30),
                CancellationToken.None))
            .Returns(ValueTask.FromResult(new SolarTrackingConfiguration
            {
                Id = 1,
                SolarPanelId = 7,
                PositionThresholdDegrees = 2.5d,
                StepDurationMs = 700,
                MaxAdjustmentSteps = 30,
            }));

        SolarTrackingConfigurationService service = new(repository.Object, queryHandler.Object);
        UpdateSolarTrackingConfigurationDto dto = new(2.5d, 700, 30);

        var result = await service.UpdateAsync(7, dto, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(new SolarTrackingConfigurationDto(7, 2.5d, 700, 30), result.Value);
    }
}
