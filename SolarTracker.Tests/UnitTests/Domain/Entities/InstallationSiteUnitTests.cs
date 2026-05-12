using Moq;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.Errors;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Tests.UnitTests.Domain.Entities;

public sealed class InstallationSiteUnitTests
{
    [Fact]
    public async Task OptimizeAsync_ShouldReturnSuccess_WhenAllSolarPanelsReachTargetPosition()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit firstUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        TiltMeasuringUnit secondUnit = new() { Id = 4, SolarPanelId = 11, GpioPin = 18 };
        InstallationSite installationSite = new()
        {
            Id = 4,
            Name = "North site",
            Latitude = 50.1m,
            Longitude = 8.6m,
            SolarPanels =
            [
                new SolarPanel
                {
                    Id = 10,
                    InstallationSiteId = 4,
                    TiltMeasuringUnit = firstUnit,
                    SolarTrackingConfiguration = new SolarTrackingConfiguration
                    {
                        SolarPanelId = 10,
                        PositionThresholdDegrees = 0.5d,
                        StepDurationMs = 0,
                        MaxAdjustmentSteps = 1,
                    },
                    LinearMotors = [new LinearMotor { Id = 30, SolarPanelId = 10, MoveUpGpioPin = 11, MoveDownGpioPin = 12 }],
                },
                new SolarPanel
                {
                    Id = 11,
                    InstallationSiteId = 4,
                    TiltMeasuringUnit = secondUnit,
                    SolarTrackingConfiguration = new SolarTrackingConfiguration
                    {
                        SolarPanelId = 11,
                        PositionThresholdDegrees = 0.5d,
                        StepDurationMs = 0,
                        MaxAdjustmentSteps = 1,
                    },
                    LinearMotors = [new LinearMotor { Id = 31, SolarPanelId = 11, MoveUpGpioPin = 13, MoveDownGpioPin = 14 }],
                },
            ],
        };
        DateTimeOffset timestamp = new(2026, 12, 21, 12, 0, 0, TimeSpan.Zero);
        Mock<ISolarOptimalPositionCalculator> calculator = new();
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        calculator.Setup(x => x.CalculateOptimalPosition(50.1m, 8.6m, timestamp))
            .Returns(35d);
        positionReader.Setup(x => x.GetCurrentPositionAsync(firstUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(35d, new DateTime(2026, 12, 21, 12, 0, 1, DateTimeKind.Utc))));
        positionReader.Setup(x => x.GetCurrentPositionAsync(secondUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(35d, new DateTime(2026, 12, 21, 12, 0, 2, DateTimeKind.Utc))));

        // Act
        InstallationSiteOptimizationResult result = await installationSite.OptimizeAsync(
            calculator.Object,
            timestamp,
            positionReader.Object,
            steeringCommandReceiver.Object,
            cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(InstallationSiteOptimizationResultStatus.Success, result.Status);
    }

    [Fact]
    public async Task OptimizeAsync_ShouldReturnValidationFailure_WhenSolarPanelCannotMove()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        InstallationSite installationSite = new()
        {
            Id = 4,
            Name = "North site",
            Latitude = 50.1m,
            Longitude = 8.6m,
            SolarPanels =
            [
                new SolarPanel
                {
                    Id = 10,
                    InstallationSiteId = 4,
                    SolarTrackingConfiguration = new SolarTrackingConfiguration
                    {
                        SolarPanelId = 10,
                        PositionThresholdDegrees = 0.5d,
                        StepDurationMs = 0,
                        MaxAdjustmentSteps = 1,
                    },
                },
            ],
        };
        DateTimeOffset timestamp = new(2026, 12, 21, 12, 0, 0, TimeSpan.Zero);
        Mock<ISolarOptimalPositionCalculator> calculator = new();
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        calculator.Setup(x => x.CalculateOptimalPosition(50.1m, 8.6m, timestamp))
            .Returns(35d);

        // Act
        InstallationSiteOptimizationResult result = await installationSite.OptimizeAsync(
            calculator.Object,
            timestamp,
            positionReader.Object,
            steeringCommandReceiver.Object,
            cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(InstallationSiteOptimizationResultStatus.SolarPanelValidationFailed, result.Status);
        Assert.Equal(10, result.FailedSolarPanelId);
        Assert.Equal(SolarPanelMovementValidationResult.TiltMeasuringUnitMissing, result.ValidationResult);
    }

    [Fact]
    public async Task OptimizeAsync_ShouldReturnMovementFailure_WhenSolarPanelMovementFails()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        InstallationSite installationSite = new()
        {
            Id = 4,
            Name = "North site",
            Latitude = 50.1m,
            Longitude = 8.6m,
            SolarPanels =
            [
                new SolarPanel
                {
                    Id = 10,
                    InstallationSiteId = 4,
                    TiltMeasuringUnit = tiltMeasuringUnit,
                    SolarTrackingConfiguration = new SolarTrackingConfiguration
                    {
                        SolarPanelId = 10,
                        PositionThresholdDegrees = 0.5d,
                        StepDurationMs = 0,
                        MaxAdjustmentSteps = 1,
                    },
                    LinearMotors = [new LinearMotor { Id = 30, SolarPanelId = 10, MoveUpGpioPin = 11, MoveDownGpioPin = 12 }],
                },
            ],
        };
        DateTimeOffset timestamp = new(2026, 12, 21, 12, 0, 0, TimeSpan.Zero);
        Mock<ISolarOptimalPositionCalculator> calculator = new();
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        calculator.Setup(x => x.CalculateOptimalPosition(50.1m, 8.6m, timestamp))
            .Returns(35d);
        positionReader.Setup(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(0d, new DateTime(2026, 12, 21, 12, 0, 1, DateTimeKind.Utc))));
        steeringCommandReceiver.Setup(x => x.MoveUpAsync(11, 12, cancellationToken))
            .Returns(ValueTask.FromException(new InvalidOperationException("motor blocked")));

        // Act
        InstallationSiteOptimizationResult result = await installationSite.OptimizeAsync(
            calculator.Object,
            timestamp,
            positionReader.Object,
            steeringCommandReceiver.Object,
            cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(InstallationSiteOptimizationResultStatus.SolarPanelMovementFailed, result.Status);
        Assert.Equal(10, result.FailedSolarPanelId);
        Assert.Equal(SolarPanelMoveResultStatus.MovementFailed, result.MoveResult?.Status);
    }

    [Fact]
    public async Task GetPosition_ShouldReturnAverageTiltMeasurement_WhenAllSolarPanelsHaveTiltMeasuringUnits()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit firstUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        TiltMeasuringUnit secondUnit = new() { Id = 4, SolarPanelId = 11, GpioPin = 18 };
        InstallationSite installationSite = new()
        {
            Id = 4,
            Name = "North site",
            Latitude = 50.1m,
            Longitude = 8.6m,
            SolarPanels =
            [
                new SolarPanel { Id = 10, InstallationSiteId = 4, TiltMeasuringUnit = firstUnit },
                new SolarPanel { Id = 11, InstallationSiteId = 4, TiltMeasuringUnit = secondUnit },
            ],
        };
        DateTime firstTimestamp = new(2026, 12, 21, 12, 0, 0, DateTimeKind.Utc);
        DateTime secondTimestamp = new(2026, 12, 21, 12, 0, 2, DateTimeKind.Utc);
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        positionReader.Setup(x => x.GetCurrentPositionAsync(firstUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(10d, firstTimestamp)));
        positionReader.Setup(x => x.GetCurrentPositionAsync(secondUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(20d, secondTimestamp)));

        // Act
        TiltMeasurement result = await installationSite.GetPosition(positionReader.Object, cancellationToken);

        // Assert
        Assert.Equal(15d, result.Degrees);
        Assert.Equal(secondTimestamp, result.Timestamp);
        positionReader.Verify(x => x.GetCurrentPositionAsync(firstUnit, cancellationToken), Times.Once);
        positionReader.Verify(x => x.GetCurrentPositionAsync(secondUnit, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetPosition_ShouldThrow_WhenSolarPanelDoesNotHaveTiltMeasuringUnit()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        InstallationSite installationSite = new()
        {
            Id = 4,
            Name = "North site",
            Latitude = 50.1m,
            Longitude = 8.6m,
            SolarPanels =
            [
                new SolarPanel { Id = 10, InstallationSiteId = 4, TiltMeasuringUnit = null },
            ],
        };
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();

        // Act
        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => installationSite.GetPosition(positionReader.Object, cancellationToken).AsTask());

        // Assert
        Assert.Equal(
            DomainTextCatalog.SolarPanel.PositionRequiresTiltMeasuringUnit(),
            exception.Message);
    }

    [Fact]
    public void GetOptimalPosition_ShouldDelegateCoordinatesAndTimestampToCalculator_WhenInvoked()
    {
        // Arrange
        InstallationSite installationSite = new()
        {
            Id = 4,
            Name = "North site",
            Latitude = 50.1m,
            Longitude = 8.6m,
        };
        DateTimeOffset timestamp = new(2026, 12, 21, 12, 0, 0, TimeSpan.Zero);
        Mock<ISolarOptimalPositionCalculator> calculator = new();
        calculator.Setup(x => x.CalculateOptimalPosition(50.1m, 8.6m, timestamp))
            .Returns(37.5d);

        // Act
        double result = installationSite.GetOptimalPosition(calculator.Object, timestamp);

        // Assert
        Assert.Equal(37.5d, result);
    }
}
