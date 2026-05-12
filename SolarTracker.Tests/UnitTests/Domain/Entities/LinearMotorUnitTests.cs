using Moq;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Domain.Entities;

public sealed class LinearMotorUnitTests
{
    [Fact]
    public async Task MoveUpAsync_ShouldPassPinsToReceiver_WhenInvoked()
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
        Mock<ISteeringCommandReceiver> receiver = new();

        // Act
        await linearMotor.MoveUpAsync(receiver.Object, cancellationToken);

        // Assert
        receiver.Verify(x => x.MoveUpAsync(17, 18, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task MoveDownAsync_ShouldPassPinsToReceiver_WhenInvoked()
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
        Mock<ISteeringCommandReceiver> receiver = new();

        // Act
        await linearMotor.MoveDownAsync(receiver.Object, cancellationToken);

        // Assert
        receiver.Verify(x => x.MoveDownAsync(17, 18, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task StopAsync_ShouldPassPinsToReceiver_WhenInvoked()
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
        Mock<ISteeringCommandReceiver> receiver = new();

        // Act
        await linearMotor.StopAsync(receiver.Object, cancellationToken);

        // Assert
        receiver.Verify(x => x.StopAsync(17, 18, cancellationToken), Times.Once);
    }
}
