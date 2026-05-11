using Moq;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.Unit.Services;

public sealed class SolarPanelOptimizationStateServiceTests
{
    [Fact]
    public async Task GetAsync_returns_not_found_when_solar_panel_does_not_exist()
    {
        Mock<ISolarPanelOptimizationStateRepository> repository = new();
        Mock<ISolarPanelQueryHandler> queryHandler = new();
        queryHandler.Setup(x => x.GetByIdAsync(9, CancellationToken.None))
            .Returns(ValueTask.FromResult<SolarPanel?>(null));

        SolarPanelOptimizationStateService service = new(repository.Object, queryHandler.Object);

        var result = await service.GetAsync(9, CancellationToken.None);

        Assert.True(result.IsNotFound);
        repository.Verify(x => x.GetBySolarPanelIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_persists_optimization_state_when_solar_panel_exists()
    {
        Mock<ISolarPanelOptimizationStateRepository> repository = new();
        Mock<ISolarPanelQueryHandler> queryHandler = new();
        queryHandler.Setup(x => x.GetByIdAsync(9, CancellationToken.None))
            .Returns(ValueTask.FromResult<SolarPanel?>(new SolarPanel { Id = 9 }));
        repository.Setup(x => x.UpsertAsync(
                It.Is<SolarPanelOptimizationState>(s => s.SolarPanelId == 9 && s.IsEnabled),
                CancellationToken.None))
            .Returns(ValueTask.FromResult(new SolarPanelOptimizationState
            {
                Id = 3,
                SolarPanelId = 9,
                IsEnabled = true,
            }));

        SolarPanelOptimizationStateService service = new(repository.Object, queryHandler.Object);

        var result = await service.UpdateAsync(9, new UpdateSolarPanelOptimizationStateDto(true), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(new SolarPanelOptimizationStateDto(9, true), result.Value);
    }
}
