using Microsoft.Extensions.Logging;
using Moq;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Services;

namespace SolarTracker.Tests.UnitTests.Infrastructure.Services;

public sealed class LinearMotorMovementServiceUnitTests
{
    [Fact]
    public async Task MoveUpAsync_ShouldReturnNotFound_WhenLinearMotorDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ILinearMotorQueryHandler> linearMotorQueryHandler = new();
        Mock<ISteeringCommandReceiver> receiver = new();
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(4, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(null));

        LinearMotorMovementService service = new(
            linearMotorQueryHandler.Object,
            receiver.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());

        // Act
        var result = await service.MoveUpAsync(4, 600, cancellationToken);

        // Assert
        Assert.True(result.IsNotFound);
        Assert.Equal("linear-motor-not-found", result.Error?.Code);
        receiver.Verify(x => x.MoveUpAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        receiver.Verify(x => x.StopAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task MoveUpAsync_ShouldSendMoveAndStopCommands_WhenLinearMotorExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ILinearMotorQueryHandler> linearMotorQueryHandler = new();
        Mock<ISteeringCommandReceiver> receiver = new();
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(4, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(CreateLinearMotor()));

        LinearMotorMovementService service = new(
            linearMotorQueryHandler.Object,
            receiver.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());

        // Act
        var result = await service.MoveUpAsync(4, 600, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        receiver.Verify(x => x.MoveUpAsync(17, 18, cancellationToken), Times.Once);
        receiver.Verify(x => x.StopAsync(17, 18, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MoveDownAsync_ShouldSendMoveAndStopCommands_WhenLinearMotorExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ILinearMotorQueryHandler> linearMotorQueryHandler = new();
        Mock<ISteeringCommandReceiver> receiver = new();
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(4, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(CreateLinearMotor()));

        LinearMotorMovementService service = new(
            linearMotorQueryHandler.Object,
            receiver.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());

        // Act
        var result = await service.MoveDownAsync(4, 600, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        receiver.Verify(x => x.MoveDownAsync(17, 18, cancellationToken), Times.Once);
        receiver.Verify(x => x.StopAsync(17, 18, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MoveUpAsync_ShouldStopEvenWhenDelayIsCanceled_WhenCancellationOccursDuringMovement()
    {
        // Arrange
        using CancellationTokenSource cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        Mock<ILinearMotorQueryHandler> linearMotorQueryHandler = new();
        Mock<ISteeringCommandReceiver> receiver = new();
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(4, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(CreateLinearMotor()));
        receiver.Setup(x => x.MoveUpAsync(17, 18, cancellationToken))
            .Callback(() => cancellationTokenSource.Cancel())
            .Returns(ValueTask.CompletedTask);

        LinearMotorMovementService service = new(
            linearMotorQueryHandler.Object,
            receiver.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => service.MoveUpAsync(4, 650, cancellationToken).AsTask());

        // Assert
        receiver.Verify(x => x.MoveUpAsync(17, 18, cancellationToken), Times.Once);
        receiver.Verify(x => x.StopAsync(17, 18, It.IsAny<CancellationToken>()), Times.Once);
    }

    private static LinearMotor CreateLinearMotor() =>
        new()
        {
            Id = 4,
            SolarPanelId = 9,
            MoveUpGpioPin = 17,
            MoveDownGpioPin = 18,
        };
}
