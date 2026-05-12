using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class SolarPanelServiceUnitTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistMappedEntityAndReturnId_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ISolarOptimalPositionCalculator> solarOptimalPositionCalculator = new();
        repository.Setup(x => x.AddAsync(It.IsAny<SolarPanel>(), cancellationToken))
            .Callback<SolarPanel, CancellationToken>((entity, _) => entity.Id = 44)
            .Returns(ValueTask.CompletedTask);

        SolarPanelService service =
            new(
                repository.Object,
                solarPanelQueryHandler.Object,
                installationSiteQueryHandler.Object,
                tiltMeasuringUnitPositionReader.Object,
                solarOptimalPositionCalculator.Object,
                TimeProvider.System,
                NullLogger<SolarPanelService>.Instance);
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
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ISolarOptimalPositionCalculator> solarOptimalPositionCalculator = new();
        repository.Setup(x => x.UpdateAsync(It.IsAny<SolarPanel>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        SolarPanelService service =
            new(
                repository.Object,
                solarPanelQueryHandler.Object,
                installationSiteQueryHandler.Object,
                tiltMeasuringUnitPositionReader.Object,
                solarOptimalPositionCalculator.Object,
                TimeProvider.System,
                NullLogger<SolarPanelService>.Instance);
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
    public async Task GetCurrentPositionAsync_ShouldReturnCurrentAndOptimalPosition_WhenSolarPanelExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ISolarOptimalPositionCalculator> solarOptimalPositionCalculator = new();
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 6, SolarPanelId = 12, GpioPin = 17 };
        SolarPanel solarPanel = new()
        {
            Id = 12,
            InstallationSiteId = 4,
            SerialNumber = "panel-12",
            TiltMeasuringUnit = tiltMeasuringUnit,
        };
        InstallationSite installationSite = new()
        {
            Id = 4,
            Name = "North site",
            Latitude = 50.1m,
            Longitude = 8.6m,
        };
        DateTime timestamp = new(2026, 12, 21, 12, 0, 0, DateTimeKind.Utc);
        DateTimeOffset utcNow = new(2026, 12, 21, 12, 5, 0, TimeSpan.Zero);
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(12, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(solarPanel));
        installationSiteQueryHandler.Setup(x => x.GetByIdAsync(4, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(installationSite));
        tiltMeasuringUnitPositionReader.Setup(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(28.5d, timestamp)));
        solarOptimalPositionCalculator.Setup(x => x.CalculateOptimalPosition(50.1m, 8.6m, utcNow))
            .Returns(35.0d);

        SolarPanelService service =
            new(
                repository.Object,
                solarPanelQueryHandler.Object,
                installationSiteQueryHandler.Object,
                tiltMeasuringUnitPositionReader.Object,
                solarOptimalPositionCalculator.Object,
                new FixedTimeProvider(utcNow),
                NullLogger<SolarPanelService>.Instance);

        // Act
        Result<SolarPanelCurrentPositionDto> result = await service.GetCurrentPositionAsync(12, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new SolarPanelCurrentPositionDto(12, 35.0d, 28.5d), result.Value);
    }

    [Fact]
    public async Task DeleteAsync_ShouldForwardIdAndCancellationToken_WhenCalled()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelRepository> repository = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ISolarOptimalPositionCalculator> solarOptimalPositionCalculator = new();
        repository.Setup(x => x.DeleteAsync(44, cancellationToken))
            .Returns(ValueTask.CompletedTask);

        SolarPanelService service =
            new(
                repository.Object,
                solarPanelQueryHandler.Object,
                installationSiteQueryHandler.Object,
                tiltMeasuringUnitPositionReader.Object,
                solarOptimalPositionCalculator.Object,
                TimeProvider.System,
                NullLogger<SolarPanelService>.Instance);

        // Act
        await service.DeleteAsync(44, cancellationToken);

        // Assert
        repository.Verify(x => x.DeleteAsync(44, cancellationToken), Times.Once);
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }
}
