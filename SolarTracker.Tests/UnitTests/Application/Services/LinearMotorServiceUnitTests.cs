using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class LinearMotorServiceUnitTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistMappedEntityAndReturnId_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ILinearMotorRepository> repository = new();
        repository.Setup(x => x.AddAsync(It.IsAny<LinearMotor>(), cancellationToken))
            .Callback<LinearMotor, CancellationToken>((entity, _) => entity.Id = 23)
            .Returns(ValueTask.CompletedTask);

        LinearMotorService service = new(repository.Object, NullLogger<LinearMotorService>.Instance);
        CreateLinearMotorDto dto = new(8, "Motor A", 17, 18);

        // Act
        int id = await service.AddAsync(dto, cancellationToken);

        // Assert
        Assert.Equal(23, id);
        repository.Verify(
            x => x.AddAsync(
                It.Is<LinearMotor>(entity =>
                    entity.SolarPanelId == 8 &&
                    entity.Name == "Motor A" &&
                    entity.MoveUpGpioPin == 17 &&
                    entity.MoveDownGpioPin == 18),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistMappedEntity_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ILinearMotorRepository> repository = new();
        repository.Setup(x => x.UpdateAsync(It.IsAny<LinearMotor>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        LinearMotorService service = new(repository.Object, NullLogger<LinearMotorService>.Instance);
        UpdateLinearMotorDto dto = new(23, 8, "Motor B", 19, 20);

        // Act
        await service.UpdateAsync(dto, cancellationToken);

        // Assert
        repository.Verify(
            x => x.UpdateAsync(
                It.Is<LinearMotor>(entity =>
                    entity.Id == 23 &&
                    entity.SolarPanelId == 8 &&
                    entity.Name == "Motor B" &&
                    entity.MoveUpGpioPin == 19 &&
                    entity.MoveDownGpioPin == 20),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldForwardIdAndCancellationToken_WhenCalled()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ILinearMotorRepository> repository = new();
        repository.Setup(x => x.DeleteAsync(23, cancellationToken))
            .Returns(ValueTask.CompletedTask);

        LinearMotorService service = new(repository.Object, NullLogger<LinearMotorService>.Instance);

        // Act
        await service.DeleteAsync(23, cancellationToken);

        // Assert
        repository.Verify(x => x.DeleteAsync(23, cancellationToken), Times.Once);
    }
}
