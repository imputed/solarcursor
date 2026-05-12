using Moq;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class SolarTrackingConfigurationServiceUnitTests
{
    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenSolarPanelDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarTrackingConfigurationRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(7, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(null));

        SolarTrackingConfigurationService service = new(repository.Object, solarPanelQueryHandler.Object);

        // Act
        var result = await service.GetAsync(7, cancellationToken);

        // Assert
        Assert.True(result.IsNotFound);
        Assert.Equal("solar-panel-not-found", result.Error?.Code);
        repository.Verify(x => x.GetBySolarPanelIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnMappedConfiguration_WhenSolarPanelExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarTrackingConfigurationRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(7, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(new SolarPanel { Id = 7 }));
        repository.Setup(x => x.GetBySolarPanelIdAsync(7, cancellationToken))
            .Returns(ValueTask.FromResult(new SolarTrackingConfiguration
            {
                SolarPanelId = 7,
                PositionThresholdDegrees = 2.5d,
                StepDurationMs = 700,
                MaxAdjustmentSteps = 30,
            }));

        SolarTrackingConfigurationService service = new(repository.Object, solarPanelQueryHandler.Object);

        // Act
        var result = await service.GetAsync(7, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new SolarTrackingConfigurationDto(7, 2.5d, 700, 30), result.Value);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenSolarPanelDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarTrackingConfigurationRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(7, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(null));

        SolarTrackingConfigurationService service = new(repository.Object, solarPanelQueryHandler.Object);

        // Act
        var result = await service.UpdateAsync(7, new UpdateSolarTrackingConfigurationDto(2.5d, 700, 30), cancellationToken);

        // Assert
        Assert.True(result.IsNotFound);
        repository.Verify(x => x.UpsertAsync(It.IsAny<SolarTrackingConfiguration>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistMappedConfiguration_WhenSolarPanelExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarTrackingConfigurationRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(7, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(new SolarPanel { Id = 7 }));
        repository.Setup(x => x.UpsertAsync(
                It.Is<SolarTrackingConfiguration>(entity =>
                    entity.SolarPanelId == 7 &&
                    entity.PositionThresholdDegrees == 2.5d &&
                    entity.StepDurationMs == 700 &&
                    entity.MaxAdjustmentSteps == 30),
                cancellationToken))
            .Returns(ValueTask.FromResult(new SolarTrackingConfiguration
            {
                SolarPanelId = 7,
                PositionThresholdDegrees = 2.5d,
                StepDurationMs = 700,
                MaxAdjustmentSteps = 30,
            }));

        SolarTrackingConfigurationService service = new(repository.Object, solarPanelQueryHandler.Object);
        UpdateSolarTrackingConfigurationDto dto = new(2.5d, 700, 30);

        // Act
        var result = await service.UpdateAsync(7, dto, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new SolarTrackingConfigurationDto(7, 2.5d, 700, 30), result.Value);
    }
}
