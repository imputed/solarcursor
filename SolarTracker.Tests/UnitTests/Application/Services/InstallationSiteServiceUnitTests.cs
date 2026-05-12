using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Application.Dtos.InstallationSite;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Results;
using SolarTracker.Application.Services;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class InstallationSiteServiceUnitTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistMappedEntityAndReturnId_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        Mock<ISolarOptimalPositionCalculator> solarOptimalPositionCalculator = new();
        repository.Setup(x => x.AddAsync(It.IsAny<InstallationSite>(), cancellationToken))
            .Callback<InstallationSite, CancellationToken>((entity, _) => entity.Id = 42)
            .Returns(ValueTask.CompletedTask);

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            tiltMeasuringUnitPositionReader.Object,
            steeringCommandReceiver.Object,
            solarOptimalPositionCalculator.Object,
            TimeProvider.System,
            NullLogger<InstallationSiteService>.Instance);
        CreateInstallationSiteDto dto = new("Primary site", 50.1m, 8.6m);

        // Act
        int id = await service.AddAsync(dto, cancellationToken);

        // Assert
        Assert.Equal(42, id);
        repository.Verify(
            x => x.AddAsync(
                It.Is<InstallationSite>(entity =>
                    entity.Name == "Primary site" &&
                    entity.Latitude == 50.1m &&
                    entity.Longitude == 8.6m &&
                    entity.SolarPanels.Count == 0),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistMappedEntity_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        Mock<ISolarOptimalPositionCalculator> solarOptimalPositionCalculator = new();
        repository.Setup(x => x.UpdateAsync(It.IsAny<InstallationSite>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            tiltMeasuringUnitPositionReader.Object,
            steeringCommandReceiver.Object,
            solarOptimalPositionCalculator.Object,
            TimeProvider.System,
            NullLogger<InstallationSiteService>.Instance);
        UpdateInstallationSiteDto dto = new(7, "Updated site", 49.2m, 9.4m);

        // Act
        await service.UpdateAsync(dto, cancellationToken);

        // Assert
        repository.Verify(
            x => x.UpdateAsync(
                It.Is<InstallationSite>(entity =>
                    entity.Id == 7 &&
                    entity.Name == "Updated site" &&
                    entity.Latitude == 49.2m &&
                    entity.Longitude == 9.4m &&
                    entity.SolarPanels.Count == 0),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldForwardIdAndCancellationToken_WhenCalled()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        Mock<ISolarOptimalPositionCalculator> solarOptimalPositionCalculator = new();
        repository.Setup(x => x.DeleteAsync(9, cancellationToken))
            .Returns(ValueTask.CompletedTask);

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            tiltMeasuringUnitPositionReader.Object,
            steeringCommandReceiver.Object,
            solarOptimalPositionCalculator.Object,
            TimeProvider.System,
            NullLogger<InstallationSiteService>.Instance);

        // Act
        await service.DeleteAsync(9, cancellationToken);

        // Assert
        repository.Verify(x => x.DeleteAsync(9, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task MoveToOptimumAsync_ShouldReturnNotFound_WhenInstallationSiteDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        Mock<ISolarOptimalPositionCalculator> solarOptimalPositionCalculator = new();
        queryHandler.Setup(x => x.GetByIdAsync(5, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(null));

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            tiltMeasuringUnitPositionReader.Object,
            steeringCommandReceiver.Object,
            solarOptimalPositionCalculator.Object,
            TimeProvider.System,
            NullLogger<InstallationSiteService>.Instance);

        // Act
        Result result = await service.Optimize(5, cancellationToken);

        // Assert
        Assert.True(result.IsNotFound);
        Assert.Equal(SolarTrackerErrorCatalog.InstallationSite.NotFound(5), result.Error);
    }

    [Fact]
    public async Task MoveToOptimumAsync_ShouldMoveAllSolarPanels_WhenInstallationSiteExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        Mock<ISolarOptimalPositionCalculator> solarOptimalPositionCalculator = new();
        TiltMeasuringUnit firstTiltMeasuringUnit = new() { Id = 21, SolarPanelId = 12, GpioPin = 17 };
        TiltMeasuringUnit secondTiltMeasuringUnit = new() { Id = 22, SolarPanelId = 14, GpioPin = 18 };
        InstallationSite installationSite = new()
        {
            Id = 5,
            Name = "Primary site",
            Latitude = 50.1m,
            Longitude = 8.6m,
            SolarPanels =
            [
                new SolarPanel
                {
                    Id = 12,
                    InstallationSiteId = 5,
                    SerialNumber = "panel-12",
                    SolarTrackingConfiguration = new SolarTrackingConfiguration
                    {
                        SolarPanelId = 12,
                        PositionThresholdDegrees = 0.5d,
                        StepDurationMs = 0,
                        MaxAdjustmentSteps = 1,
                    },
                    TiltMeasuringUnit = firstTiltMeasuringUnit,
                    LinearMotors = [new LinearMotor { Id = 31, SolarPanelId = 12, MoveUpGpioPin = 10, MoveDownGpioPin = 11 }],
                },
                new SolarPanel
                {
                    Id = 14,
                    InstallationSiteId = 5,
                    SerialNumber = "panel-14",
                    SolarTrackingConfiguration = new SolarTrackingConfiguration
                    {
                        SolarPanelId = 14,
                        PositionThresholdDegrees = 0.5d,
                        StepDurationMs = 0,
                        MaxAdjustmentSteps = 1,
                    },
                    TiltMeasuringUnit = secondTiltMeasuringUnit,
                    LinearMotors = [new LinearMotor { Id = 32, SolarPanelId = 14, MoveUpGpioPin = 12, MoveDownGpioPin = 13 }],
                },
            ],
        };
        DateTimeOffset utcNow = new(2026, 12, 21, 12, 0, 0, TimeSpan.Zero);
        queryHandler.Setup(x => x.GetByIdAsync(5, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(installationSite));
        solarOptimalPositionCalculator.Setup(x => x.CalculateOptimalPosition(50.1m, 8.6m, utcNow))
            .Returns(35.0d);
        tiltMeasuringUnitPositionReader.Setup(x => x.GetCurrentPositionAsync(firstTiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(35.0d, new DateTime(2026, 12, 21, 12, 0, 1, DateTimeKind.Utc))));
        tiltMeasuringUnitPositionReader.Setup(x => x.GetCurrentPositionAsync(secondTiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(35.0d, new DateTime(2026, 12, 21, 12, 0, 2, DateTimeKind.Utc))));

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            tiltMeasuringUnitPositionReader.Object,
            steeringCommandReceiver.Object,
            solarOptimalPositionCalculator.Object,
            new FixedTimeProvider(utcNow),
            NullLogger<InstallationSiteService>.Instance);

        // Act
        Result result = await service.Optimize(5, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task MoveToOptimumAsync_ShouldReturnFailure_WhenSolarPanelMovementFails()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        Mock<ISolarOptimalPositionCalculator> solarOptimalPositionCalculator = new();
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 21, SolarPanelId = 12, GpioPin = 17 };
        InstallationSite installationSite = new()
        {
            Id = 5,
            Name = "Primary site",
            Latitude = 50.1m,
            Longitude = 8.6m,
            SolarPanels =
            [
                new SolarPanel
                {
                    Id = 12,
                    InstallationSiteId = 5,
                    SerialNumber = "panel-12",
                    SolarTrackingConfiguration = new SolarTrackingConfiguration
                    {
                        SolarPanelId = 12,
                        PositionThresholdDegrees = 0.5d,
                        StepDurationMs = 0,
                        MaxAdjustmentSteps = 1,
                    },
                    TiltMeasuringUnit = tiltMeasuringUnit,
                    LinearMotors = [new LinearMotor { Id = 31, SolarPanelId = 12, MoveUpGpioPin = 10, MoveDownGpioPin = 11 }],
                },
            ],
        };
        DateTimeOffset utcNow = new(2026, 12, 21, 12, 0, 0, TimeSpan.Zero);
        queryHandler.Setup(x => x.GetByIdAsync(5, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(installationSite));
        solarOptimalPositionCalculator.Setup(x => x.CalculateOptimalPosition(50.1m, 8.6m, utcNow))
            .Returns(35.0d);
        tiltMeasuringUnitPositionReader.Setup(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(0d, new DateTime(2026, 12, 21, 12, 0, 1, DateTimeKind.Utc))));
        steeringCommandReceiver.Setup(x => x.MoveUpAsync(10, 11, cancellationToken))
            .Returns(ValueTask.FromException(new InvalidOperationException("motor blocked")));

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            tiltMeasuringUnitPositionReader.Object,
            steeringCommandReceiver.Object,
            solarOptimalPositionCalculator.Object,
            new FixedTimeProvider(utcNow),
            NullLogger<InstallationSiteService>.Instance);

        // Act
        Result result = await service.Optimize(5, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("solar-panel-movement-failed", result.Error?.Code);
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }
}
