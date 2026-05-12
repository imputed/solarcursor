using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Application.Dtos.SolarPanelOptimizationState;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Services;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class SolarPanelOptimizationStateServiceUnitTests
{
    [Fact]
    public async Task GetAsync_ShouldReturnNotFound_WhenSolarPanelDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelOptimizationStateRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(9, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(null));

        SolarPanelOptimizationStateService service =
            new(repository.Object, solarPanelQueryHandler.Object, NullLogger<SolarPanelOptimizationStateService>.Instance);

        // Act
        var result = await service.GetAsync(9, cancellationToken);

        // Assert
        Assert.True(result.IsNotFound);
        Assert.Equal("solar-panel-not-found", result.Error?.Code);
        repository.Verify(x => x.GetBySolarPanelIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnMappedState_WhenSolarPanelExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelOptimizationStateRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(9, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(new SolarPanel { Id = 9 }));
        repository.Setup(x => x.GetBySolarPanelIdAsync(9, cancellationToken))
            .Returns(ValueTask.FromResult(new SolarPanelOptimizationState
            {
                SolarPanelId = 9,
                IsEnabled = true,
            }));

        SolarPanelOptimizationStateService service =
            new(repository.Object, solarPanelQueryHandler.Object, NullLogger<SolarPanelOptimizationStateService>.Instance);

        // Act
        var result = await service.GetAsync(9, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new SolarPanelOptimizationStateDto(9, true), result.Value);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenSolarPanelDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelOptimizationStateRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(9, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(null));

        SolarPanelOptimizationStateService service =
            new(repository.Object, solarPanelQueryHandler.Object, NullLogger<SolarPanelOptimizationStateService>.Instance);

        // Act
        var result = await service.UpdateAsync(9, new UpdateSolarPanelOptimizationStateDto(true), cancellationToken);

        // Assert
        Assert.True(result.IsNotFound);
        repository.Verify(x => x.UpsertAsync(It.IsAny<SolarPanelOptimizationState>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistMappedState_WhenSolarPanelExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelOptimizationStateRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(9, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(new SolarPanel { Id = 9 }));
        repository.Setup(x => x.UpsertAsync(
                It.Is<SolarPanelOptimizationState>(entity =>
                    entity.SolarPanelId == 9 &&
                    entity.IsEnabled),
                cancellationToken))
            .Returns(ValueTask.FromResult(new SolarPanelOptimizationState
            {
                SolarPanelId = 9,
                IsEnabled = true,
            }));

        SolarPanelOptimizationStateService service =
            new(repository.Object, solarPanelQueryHandler.Object, NullLogger<SolarPanelOptimizationStateService>.Instance);

        // Act
        var result = await service.UpdateAsync(9, new UpdateSolarPanelOptimizationStateDto(true), cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new SolarPanelOptimizationStateDto(9, true), result.Value);
    }
}
