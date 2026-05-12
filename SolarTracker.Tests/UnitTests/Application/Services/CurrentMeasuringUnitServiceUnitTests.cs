using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Application.Dtos.CurrentMeasuringUnit;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Services;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class CurrentMeasuringUnitServiceUnitTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistMappedEntityAndReturnId_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ICurrentMeasuringUnitRepository> repository = new();
        repository.Setup(x => x.AddAsync(It.IsAny<CurrentMeasuringUnit>(), cancellationToken))
            .Callback<CurrentMeasuringUnit, CancellationToken>((entity, _) => entity.Id = 17)
            .Returns(ValueTask.CompletedTask);

        CurrentMeasuringUnitService service = new(repository.Object, NullLogger<CurrentMeasuringUnitService>.Instance);
        CreateCurrentMeasuringUnitDto dto = new(5, "Current sensor", 21);

        // Act
        int id = await service.AddAsync(dto, cancellationToken);

        // Assert
        Assert.Equal(17, id);
        repository.Verify(
            x => x.AddAsync(
                It.Is<CurrentMeasuringUnit>(entity =>
                    entity.SolarPanelId == 5 &&
                    entity.Name == "Current sensor" &&
                    entity.GpioPin == 21),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistMappedEntity_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ICurrentMeasuringUnitRepository> repository = new();
        repository.Setup(x => x.UpdateAsync(It.IsAny<CurrentMeasuringUnit>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        CurrentMeasuringUnitService service = new(repository.Object, NullLogger<CurrentMeasuringUnitService>.Instance);
        UpdateCurrentMeasuringUnitDto dto = new(13, 5, "Updated sensor", 22);

        // Act
        await service.UpdateAsync(dto, cancellationToken);

        // Assert
        repository.Verify(
            x => x.UpdateAsync(
                It.Is<CurrentMeasuringUnit>(entity =>
                    entity.Id == 13 &&
                    entity.SolarPanelId == 5 &&
                    entity.Name == "Updated sensor" &&
                    entity.GpioPin == 22),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldForwardIdAndCancellationToken_WhenCalled()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ICurrentMeasuringUnitRepository> repository = new();
        repository.Setup(x => x.DeleteAsync(13, cancellationToken))
            .Returns(ValueTask.CompletedTask);

        CurrentMeasuringUnitService service = new(repository.Object, NullLogger<CurrentMeasuringUnitService>.Instance);

        // Act
        await service.DeleteAsync(13, cancellationToken);

        // Assert
        repository.Verify(x => x.DeleteAsync(13, cancellationToken), Times.Once);
    }
}
