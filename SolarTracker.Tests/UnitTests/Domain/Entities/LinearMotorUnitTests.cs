using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Domain.Entities;

public sealed class LinearMotorUnitTests
{
    [Fact]
    public async Task MoveUpAsync_ShouldPassTheLinearMotorAndDurationToTheAction_WhenInvoked()
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
        LinearMotor? passedMotor = null;
        int passedDurationMs = 0;
        CancellationToken passedCancellationToken = default;

        ValueTask Action(LinearMotor motor, int durationMs, CancellationToken token)
        {
            passedMotor = motor;
            passedDurationMs = durationMs;
            passedCancellationToken = token;
            return ValueTask.CompletedTask;
        }

        // Act
        await linearMotor.MoveUpAsync(Action, 900, cancellationToken);

        // Assert
        Assert.Same(linearMotor, passedMotor);
        Assert.Equal(900, passedDurationMs);
        Assert.Equal(cancellationToken, passedCancellationToken);
    }

    [Fact]
    public async Task MoveDownAsync_ShouldPassTheLinearMotorAndDurationToTheAction_WhenInvoked()
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
        LinearMotor? passedMotor = null;
        int passedDurationMs = 0;
        CancellationToken passedCancellationToken = default;

        ValueTask Action(LinearMotor motor, int durationMs, CancellationToken token)
        {
            passedMotor = motor;
            passedDurationMs = durationMs;
            passedCancellationToken = token;
            return ValueTask.CompletedTask;
        }

        // Act
        await linearMotor.MoveDownAsync(Action, 900, cancellationToken);

        // Assert
        Assert.Same(linearMotor, passedMotor);
        Assert.Equal(900, passedDurationMs);
        Assert.Equal(cancellationToken, passedCancellationToken);
    }
}
