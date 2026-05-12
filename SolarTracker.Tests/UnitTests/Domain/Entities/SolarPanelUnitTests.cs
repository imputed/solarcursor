using Moq;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.Errors;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Tests.UnitTests.Domain.Entities;

public sealed class SolarPanelUnitTests
{
    [Fact]
    public async Task GetPosition_ShouldReturnTiltMeasurement_WhenTiltMeasuringUnitExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = tiltMeasuringUnit,
        };
        TiltMeasurement expected = new(12.5d, new DateTime(2026, 12, 21, 12, 0, 0, DateTimeKind.Utc));
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        positionReader.Setup(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(expected));

        // Act
        TiltMeasurement result = await solarPanel.GetPosition(positionReader.Object, cancellationToken);

        // Assert
        Assert.Equal(expected, result);
        positionReader.Verify(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetPosition_ShouldThrow_WhenTiltMeasuringUnitDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = null,
        };
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();

        // Act
        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => solarPanel.GetPosition(positionReader.Object, cancellationToken).AsTask());

        // Assert
        Assert.Equal(DomainTextCatalog.SolarPanel.PositionRequiresTiltMeasuringUnit(), exception.Message);
    }

    [Fact]
    public async Task MoveToTargetPositionAsync_ShouldReturnSuccessWithoutMoving_WhenCurrentPositionIsWithinThreshold()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = tiltMeasuringUnit,
            LinearMotors = [new LinearMotor { Id = 20, SolarPanelId = 10, MoveUpGpioPin = 11, MoveDownGpioPin = 12 }],
        };
        SolarTrackingConfiguration configuration = new()
        {
            SolarPanelId = 10,
            PositionThresholdDegrees = 0.5d,
            StepDurationMs = 0,
            MaxAdjustmentSteps = 1,
        };
        TiltMeasurement expected = new(10.2d, new DateTime(2026, 12, 21, 12, 0, 0, DateTimeKind.Utc));
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        positionReader.Setup(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(expected));

        // Act
        SolarPanelMoveResult result = await solarPanel.MoveToTargetPositionAsync(
            10.0d,
            configuration,
            positionReader.Object,
            steeringCommandReceiver.Object,
            cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Measurement);
        steeringCommandReceiver.Verify(x => x.MoveUpAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        steeringCommandReceiver.Verify(x => x.MoveDownAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task MoveToTargetPositionAsync_ShouldReturnThresholdNotMet_WhenMaxAdjustmentStepsAreReached()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = tiltMeasuringUnit,
            LinearMotors = [new LinearMotor { Id = 20, SolarPanelId = 10, MoveUpGpioPin = 11, MoveDownGpioPin = 12 }],
        };
        SolarTrackingConfiguration configuration = new()
        {
            SolarPanelId = 10,
            PositionThresholdDegrees = 0.5d,
            StepDurationMs = 0,
            MaxAdjustmentSteps = 1,
        };
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        positionReader.SetupSequence(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(0d, new DateTime(2026, 12, 21, 12, 0, 0, DateTimeKind.Utc))))
            .Returns(ValueTask.FromResult(new TiltMeasurement(10d, new DateTime(2026, 12, 21, 12, 0, 1, DateTimeKind.Utc))));
        steeringCommandReceiver.Setup(x => x.MoveUpAsync(11, 12, cancellationToken))
            .Returns(ValueTask.CompletedTask);
        steeringCommandReceiver.Setup(x => x.StopAsync(11, 12, It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        // Act
        SolarPanelMoveResult result = await solarPanel.MoveToTargetPositionAsync(
            45d,
            configuration,
            positionReader.Object,
            steeringCommandReceiver.Object,
            cancellationToken);

        // Assert
        Assert.Equal(SolarPanelMoveResultStatus.ThresholdNotMet, result.Status);
        Assert.Equal(10d, result.Measurement?.Degrees);
    }

    [Fact]
    public async Task MoveToTargetPositionAsync_ShouldReturnMovementFailed_WhenFirstMotorFails()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = tiltMeasuringUnit,
            LinearMotors = [new LinearMotor { Id = 20, SolarPanelId = 10, MoveUpGpioPin = 11, MoveDownGpioPin = 12 }],
        };
        SolarTrackingConfiguration configuration = new()
        {
            SolarPanelId = 10,
            PositionThresholdDegrees = 0.5d,
            StepDurationMs = 0,
            MaxAdjustmentSteps = 1,
        };
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        positionReader.Setup(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(0d, new DateTime(2026, 12, 21, 12, 0, 0, DateTimeKind.Utc))));
        steeringCommandReceiver.Setup(x => x.MoveUpAsync(11, 12, cancellationToken))
            .Returns(ValueTask.FromException(new InvalidOperationException("motor blocked")));

        // Act
        SolarPanelMoveResult result = await solarPanel.MoveToTargetPositionAsync(
            45d,
            configuration,
            positionReader.Object,
            steeringCommandReceiver.Object,
            cancellationToken);

        // Assert
        Assert.Equal(SolarPanelMoveResultStatus.MovementFailed, result.Status);
        Assert.Equal(20, result.FailedLinearMotorId);
        Assert.Equal("motor blocked", result.FailureMessage);
    }

    [Fact]
    public async Task MoveToTargetPositionAsync_ShouldReturnMovementStepReverted_WhenLaterMotorFails()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = tiltMeasuringUnit,
            LinearMotors =
            [
                new LinearMotor { Id = 20, SolarPanelId = 10, MoveUpGpioPin = 11, MoveDownGpioPin = 12 },
                new LinearMotor { Id = 21, SolarPanelId = 10, MoveUpGpioPin = 13, MoveDownGpioPin = 14 },
            ],
        };
        SolarTrackingConfiguration configuration = new()
        {
            SolarPanelId = 10,
            PositionThresholdDegrees = 0.5d,
            StepDurationMs = 0,
            MaxAdjustmentSteps = 1,
        };
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        positionReader.Setup(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(0d, new DateTime(2026, 12, 21, 12, 0, 0, DateTimeKind.Utc))));
        steeringCommandReceiver.Setup(x => x.MoveUpAsync(11, 12, cancellationToken))
            .Returns(ValueTask.CompletedTask);
        steeringCommandReceiver.Setup(x => x.MoveUpAsync(13, 14, cancellationToken))
            .Returns(ValueTask.FromException(new InvalidOperationException("motor 21 failed")));
        steeringCommandReceiver.Setup(x => x.MoveDownAsync(11, 12, cancellationToken))
            .Returns(ValueTask.CompletedTask);
        steeringCommandReceiver.Setup(x => x.StopAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        // Act
        SolarPanelMoveResult result = await solarPanel.MoveToTargetPositionAsync(
            45d,
            configuration,
            positionReader.Object,
            steeringCommandReceiver.Object,
            cancellationToken);

        // Assert
        Assert.Equal(SolarPanelMoveResultStatus.MovementStepReverted, result.Status);
        Assert.Equal(21, result.FailedLinearMotorId);
        Assert.Equal("motor 21 failed", result.FailureMessage);
        steeringCommandReceiver.Verify(x => x.MoveDownAsync(11, 12, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task MoveToTargetPositionAsync_ShouldReturnMovementRecoveryFailed_WhenRecoveryFails()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = tiltMeasuringUnit,
            LinearMotors =
            [
                new LinearMotor { Id = 20, SolarPanelId = 10, MoveUpGpioPin = 11, MoveDownGpioPin = 12 },
                new LinearMotor { Id = 21, SolarPanelId = 10, MoveUpGpioPin = 13, MoveDownGpioPin = 14 },
            ],
        };
        SolarTrackingConfiguration configuration = new()
        {
            SolarPanelId = 10,
            PositionThresholdDegrees = 0.5d,
            StepDurationMs = 0,
            MaxAdjustmentSteps = 1,
        };
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();
        positionReader.Setup(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(0d, new DateTime(2026, 12, 21, 12, 0, 0, DateTimeKind.Utc))));
        steeringCommandReceiver.Setup(x => x.MoveUpAsync(11, 12, cancellationToken))
            .Returns(ValueTask.CompletedTask);
        steeringCommandReceiver.Setup(x => x.MoveUpAsync(13, 14, cancellationToken))
            .Returns(ValueTask.FromException(new InvalidOperationException("motor 21 failed")));
        steeringCommandReceiver.Setup(x => x.MoveDownAsync(11, 12, cancellationToken))
            .Returns(ValueTask.FromException(new InvalidOperationException("recovery failed")));
        steeringCommandReceiver.Setup(x => x.StopAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        // Act
        SolarPanelMoveResult result = await solarPanel.MoveToTargetPositionAsync(
            45d,
            configuration,
            positionReader.Object,
            steeringCommandReceiver.Object,
            cancellationToken);

        // Assert
        Assert.Equal(SolarPanelMoveResultStatus.MovementRecoveryFailed, result.Status);
        Assert.Equal(21, result.FailedLinearMotorId);
        Assert.Equal("motor 21 failed", result.FailureMessage);
        Assert.Equal("recovery failed", result.RecoveryFailureMessage);
    }

    [Fact]
    public async Task MoveToTargetPositionAsync_ShouldThrow_WhenLinearMotorsDoNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = tiltMeasuringUnit,
            LinearMotors = [],
        };
        SolarTrackingConfiguration configuration = new() { SolarPanelId = 10 };
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        Mock<ISteeringCommandReceiver> steeringCommandReceiver = new();

        // Act
        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => solarPanel.MoveToTargetPositionAsync(
                45d,
                configuration,
                positionReader.Object,
                steeringCommandReceiver.Object,
                cancellationToken).AsTask());

        // Assert
        Assert.Equal(DomainTextCatalog.SolarPanel.MovementRequiresLinearMotors(), exception.Message);
    }
}
