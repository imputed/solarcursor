using Microsoft.Extensions.Logging;
using Moq;
using SolarTracker.Application.Interfaces.Hardware;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;
using SolarTracker.Infrastructure.Calculators;
using SolarTracker.Infrastructure.Services;

namespace SolarTracker.Tests.UnitTests.Infrastructure.Calculators;

public sealed class SolarPanelCalculatorUnitTests
{
    [Fact]
    public async Task MoveToOptimumAsync_ShouldRecoverMovedMotorsAndReturnFailure_WhenLaterMotorFails()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        LinearMotor motor1 = new() { Id = 1, SolarPanelId = 10, MoveUpGpioPin = 17, MoveDownGpioPin = 18 };
        LinearMotor motor2 = new() { Id = 2, SolarPanelId = 10, MoveUpGpioPin = 19, MoveDownGpioPin = 20 };
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 20,
            TiltMeasuringUnit = new TiltMeasuringUnit { Id = 30, SolarPanelId = 10, GpioPin = 12 },
            LinearMotors = [motor1, motor2],
        };
        InstallationSite installationSite = new() { Id = 20, Name = "Main site", Latitude = 50.1m, Longitude = 8.6m };
        SolarTrackingConfiguration configuration = new()
        {
            SolarPanelId = 10,
            PositionThresholdDegrees = 0.5d,
            StepDurationMs = 100,
            MaxAdjustmentSteps = 1,
        };

        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ILinearMotorQueryHandler> linearMotorQueryHandler = new();
        Mock<ISolarTrackingConfigurationRepository> configurationRepository = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ILinearMotorActuator> actuator = new();
        Mock<ILogger<SolarPanelCalculator>> logger = new();

        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(10, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(solarPanel));
        installationSiteQueryHandler.Setup(x => x.GetByIdAsync(20, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(installationSite));
        configurationRepository.Setup(x => x.GetBySolarPanelIdAsync(10, cancellationToken))
            .Returns(ValueTask.FromResult(configuration));
        tiltMeasuringUnitPositionReader.Setup(x => x.GetCurrentPositionAsync(It.IsAny<TiltMeasuringUnit>(), cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(0d, DateTime.UtcNow)));
        linearMotorQueryHandler.SetupSequence(x => x.GetByIdAsync(1, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(motor1))
            .Returns(ValueTask.FromResult<LinearMotor?>(motor1));
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(2, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(null));
        actuator.Setup(x => x.MoveUpAsync(It.IsAny<LinearMotorMovementContext>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);
        actuator.Setup(x => x.MoveDownAsync(It.IsAny<LinearMotorMovementContext>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        LinearMotorMovementService movementService = new(
            linearMotorQueryHandler.Object,
            solarPanelQueryHandler.Object,
            installationSiteQueryHandler.Object,
            actuator.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());
        SolarPanelCalculator calculator = new(
            solarPanelQueryHandler.Object,
            installationSiteQueryHandler.Object,
            configurationRepository.Object,
            movementService,
            tiltMeasuringUnitPositionReader.Object,
            new FixedTimeProvider(new DateTimeOffset(2026, 12, 21, 0, 0, 0, TimeSpan.Zero)),
            logger.Object);

        // Act
        var result = await calculator.MoveToOptimumAsync(10, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.False(result.IsNotFound);
        Assert.Equal("solar-panel-movement-step-reverted", result.Error?.Code);
        actuator.Verify(
            x => x.MoveUpAsync(It.Is<LinearMotorMovementContext>(context => context.LinearMotorId == 1), cancellationToken),
            Times.Once);
        actuator.Verify(
            x => x.MoveDownAsync(It.Is<LinearMotorMovementContext>(context => context.LinearMotorId == 1), cancellationToken),
            Times.Once);
        actuator.Verify(
            x => x.MoveUpAsync(It.Is<LinearMotorMovementContext>(context => context.LinearMotorId == 2), cancellationToken),
            Times.Never);
    }

    [Fact]
    public async Task MoveToOptimumAsync_ShouldReturnRecoveryFailure_WhenRecoveryCannotBeCompleted()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        LinearMotor motor1 = new() { Id = 1, SolarPanelId = 10, MoveUpGpioPin = 17, MoveDownGpioPin = 18 };
        LinearMotor motor2 = new() { Id = 2, SolarPanelId = 10, MoveUpGpioPin = 19, MoveDownGpioPin = 20 };
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 20,
            TiltMeasuringUnit = new TiltMeasuringUnit { Id = 30, SolarPanelId = 10, GpioPin = 12 },
            LinearMotors = [motor1, motor2],
        };
        InstallationSite installationSite = new() { Id = 20, Name = "Main site", Latitude = 50.1m, Longitude = 8.6m };
        SolarTrackingConfiguration configuration = new()
        {
            SolarPanelId = 10,
            PositionThresholdDegrees = 0.5d,
            StepDurationMs = 100,
            MaxAdjustmentSteps = 1,
        };

        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ILinearMotorQueryHandler> linearMotorQueryHandler = new();
        Mock<ISolarTrackingConfigurationRepository> configurationRepository = new();
        Mock<ITiltMeasuringUnitPositionReader> tiltMeasuringUnitPositionReader = new();
        Mock<ILinearMotorActuator> actuator = new();
        Mock<ILogger<SolarPanelCalculator>> logger = new();

        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(10, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(solarPanel));
        installationSiteQueryHandler.Setup(x => x.GetByIdAsync(20, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(installationSite));
        configurationRepository.Setup(x => x.GetBySolarPanelIdAsync(10, cancellationToken))
            .Returns(ValueTask.FromResult(configuration));
        tiltMeasuringUnitPositionReader.Setup(x => x.GetCurrentPositionAsync(It.IsAny<TiltMeasuringUnit>(), cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(0d, DateTime.UtcNow)));
        linearMotorQueryHandler.SetupSequence(x => x.GetByIdAsync(1, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(motor1))
            .Returns(ValueTask.FromResult<LinearMotor?>(null));
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(2, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(null));
        actuator.Setup(x => x.MoveUpAsync(It.IsAny<LinearMotorMovementContext>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        LinearMotorMovementService movementService = new(
            linearMotorQueryHandler.Object,
            solarPanelQueryHandler.Object,
            installationSiteQueryHandler.Object,
            actuator.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());
        SolarPanelCalculator calculator = new(
            solarPanelQueryHandler.Object,
            installationSiteQueryHandler.Object,
            configurationRepository.Object,
            movementService,
            tiltMeasuringUnitPositionReader.Object,
            new FixedTimeProvider(new DateTimeOffset(2026, 12, 21, 0, 0, 0, TimeSpan.Zero)),
            logger.Object);

        // Act
        var result = await calculator.MoveToOptimumAsync(10, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("solar-panel-movement-recovery-failed", result.Error?.Code);
        actuator.Verify(
            x => x.MoveUpAsync(It.Is<LinearMotorMovementContext>(context => context.LinearMotorId == 1), cancellationToken),
            Times.Once);
        actuator.Verify(
            x => x.MoveDownAsync(It.IsAny<LinearMotorMovementContext>(), cancellationToken),
            Times.Never);
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }
}
