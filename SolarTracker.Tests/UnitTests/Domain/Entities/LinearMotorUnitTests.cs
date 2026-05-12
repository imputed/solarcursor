using Moq;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Tests.UnitTests.Domain.Entities;

public sealed class LinearMotorUnitTests
{
    [Fact]
    public async Task MoveUpAsync_ShouldBuildMovementContextAndCallActuator_WhenInvoked()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        LinearMotor linearMotor = new()
        {
            Id = 7,
            SolarPanelId = 20,
            MoveUpGpioPin = 17,
            MoveDownGpioPin = 18,
        };
        Mock<ILinearMotorActuator> actuator = new();

        // Act
        await linearMotor.MoveUpAsync(actuator.Object, 3, 50.1m, 8.6m, 900, cancellationToken);

        // Assert
        actuator.Verify(
            x => x.MoveUpAsync(
                It.Is<LinearMotorMovementContext>(context =>
                    context.LinearMotorId == 7 &&
                    context.InstallationSiteId == 3 &&
                    context.Latitude == 50.1m &&
                    context.Longitude == 8.6m &&
                    context.MoveUpGpioPin == 17 &&
                    context.MoveDownGpioPin == 18 &&
                    context.DurationMs == 900),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task MoveDownAsync_ShouldBuildMovementContextAndCallActuator_WhenInvoked()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        LinearMotor linearMotor = new()
        {
            Id = 7,
            SolarPanelId = 20,
            MoveUpGpioPin = 17,
            MoveDownGpioPin = 18,
        };
        Mock<ILinearMotorActuator> actuator = new();

        // Act
        await linearMotor.MoveDownAsync(actuator.Object, 3, 50.1m, 8.6m, 900, cancellationToken);

        // Assert
        actuator.Verify(
            x => x.MoveDownAsync(
                It.Is<LinearMotorMovementContext>(context =>
                    context.LinearMotorId == 7 &&
                    context.InstallationSiteId == 3 &&
                    context.Latitude == 50.1m &&
                    context.Longitude == 8.6m &&
                    context.MoveUpGpioPin == 17 &&
                    context.MoveDownGpioPin == 18 &&
                    context.DurationMs == 900),
                cancellationToken),
            Times.Once);
    }
}
