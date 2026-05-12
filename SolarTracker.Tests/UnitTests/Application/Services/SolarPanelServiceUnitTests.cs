using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Calculators;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class SolarPanelServiceUnitTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistMappedEntityAndReturnId_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelRepository> repository = new();
        Mock<ISolarPanelCalculator> solarPanelCalculator = new();
        repository.Setup(x => x.AddAsync(It.IsAny<SolarPanel>(), cancellationToken))
            .Callback<SolarPanel, CancellationToken>((entity, _) => entity.Id = 44)
            .Returns(ValueTask.CompletedTask);

        SolarPanelService service =
            new(repository.Object, solarPanelCalculator.Object, NullLogger<SolarPanelService>.Instance);
        CreateSolarPanelDto dto = new(12, "panel-44");

        // Act
        int id = await service.AddAsync(dto, cancellationToken);

        // Assert
        Assert.Equal(44, id);
        repository.Verify(
            x => x.AddAsync(
                It.Is<SolarPanel>(entity =>
                    entity.InstallationSiteId == 12 &&
                    entity.SerialNumber == "panel-44" &&
                    entity.SolarTrackingConfiguration == null &&
                    entity.TiltMeasuringUnit == null &&
                    entity.CurrentMeasuringUnit == null &&
                    entity.LinearMotors.Count == 0),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistMappedEntity_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelRepository> repository = new();
        Mock<ISolarPanelCalculator> solarPanelCalculator = new();
        repository.Setup(x => x.UpdateAsync(It.IsAny<SolarPanel>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        SolarPanelService service =
            new(repository.Object, solarPanelCalculator.Object, NullLogger<SolarPanelService>.Instance);
        UpdateSolarPanelDto dto = new(44, 12, "panel-44-updated");

        // Act
        await service.UpdateAsync(dto, cancellationToken);

        // Assert
        repository.Verify(
            x => x.UpdateAsync(
                It.Is<SolarPanel>(entity =>
                    entity.Id == 44 &&
                    entity.InstallationSiteId == 12 &&
                    entity.SerialNumber == "panel-44-updated" &&
                    entity.LinearMotors.Count == 0),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task GetCurrentPositionAsync_ShouldDelegateToCalculator_WhenCalled()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Result<SolarPanelCurrentPositionDto> expected =
            Result<SolarPanelCurrentPositionDto>.Success(new SolarPanelCurrentPositionDto(12, 35.0d, 28.5d));
        Mock<ISolarPanelRepository> repository = new();
        Mock<ISolarPanelCalculator> solarPanelCalculator = new();
        solarPanelCalculator.Setup(x => x.GetCurrentPositionAsync(12, cancellationToken))
            .Returns(ValueTask.FromResult(expected));

        SolarPanelService service =
            new(repository.Object, solarPanelCalculator.Object, NullLogger<SolarPanelService>.Instance);

        // Act
        Result<SolarPanelCurrentPositionDto> result = await service.GetCurrentPositionAsync(12, cancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task MoveToOptimumAsync_ShouldDelegateToCalculator_WhenCalled()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Result<SolarPanelCurrentPositionDto> expected =
            Result<SolarPanelCurrentPositionDto>.Success(new SolarPanelCurrentPositionDto(12, 35.0d, 35.0d));
        Mock<ISolarPanelRepository> repository = new();
        Mock<ISolarPanelCalculator> solarPanelCalculator = new();
        solarPanelCalculator.Setup(x => x.MoveToOptimumAsync(12, cancellationToken))
            .Returns(ValueTask.FromResult(expected));

        SolarPanelService service =
            new(repository.Object, solarPanelCalculator.Object, NullLogger<SolarPanelService>.Instance);

        // Act
        Result<SolarPanelCurrentPositionDto> result = await service.MoveToOptimumAsync(12, cancellationToken);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldForwardIdAndCancellationToken_WhenCalled()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelRepository> repository = new();
        Mock<ISolarPanelCalculator> solarPanelCalculator = new();
        repository.Setup(x => x.DeleteAsync(44, cancellationToken))
            .Returns(ValueTask.CompletedTask);

        SolarPanelService service =
            new(repository.Object, solarPanelCalculator.Object, NullLogger<SolarPanelService>.Instance);

        // Act
        await service.DeleteAsync(44, cancellationToken);

        // Assert
        repository.Verify(x => x.DeleteAsync(44, cancellationToken), Times.Once);
    }
}
