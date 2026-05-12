using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Application.Dtos.TiltMeasuringUnit;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Services;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class TiltMeasuringUnitServiceUnitTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistMappedEntityAndReturnId_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ITiltMeasuringUnitRepository> repository = new();
        repository.Setup(x => x.AddAsync(It.IsAny<TiltMeasuringUnit>(), cancellationToken))
            .Callback<TiltMeasuringUnit, CancellationToken>((entity, _) => entity.Id = 31)
            .Returns(ValueTask.CompletedTask);

        TiltMeasuringUnitService service = new(repository.Object, NullLogger<TiltMeasuringUnitService>.Instance);
        CreateTiltMeasuringUnitDto dto = new(8, "Tilt sensor", 12);

        // Act
        int id = await service.AddAsync(dto, cancellationToken);

        // Assert
        Assert.Equal(31, id);
        repository.Verify(
            x => x.AddAsync(
                It.Is<TiltMeasuringUnit>(entity =>
                    entity.SolarPanelId == 8 &&
                    entity.Name == "Tilt sensor" &&
                    entity.GpioPin == 12),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistMappedEntity_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ITiltMeasuringUnitRepository> repository = new();
        repository.Setup(x => x.UpdateAsync(It.IsAny<TiltMeasuringUnit>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        TiltMeasuringUnitService service = new(repository.Object, NullLogger<TiltMeasuringUnitService>.Instance);
        UpdateTiltMeasuringUnitDto dto = new(31, 8, "Updated tilt sensor", 13);

        // Act
        await service.UpdateAsync(dto, cancellationToken);

        // Assert
        repository.Verify(
            x => x.UpdateAsync(
                It.Is<TiltMeasuringUnit>(entity =>
                    entity.Id == 31 &&
                    entity.SolarPanelId == 8 &&
                    entity.Name == "Updated tilt sensor" &&
                    entity.GpioPin == 13),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldForwardIdAndCancellationToken_WhenCalled()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ITiltMeasuringUnitRepository> repository = new();
        repository.Setup(x => x.DeleteAsync(31, cancellationToken))
            .Returns(ValueTask.CompletedTask);

        TiltMeasuringUnitService service = new(repository.Object, NullLogger<TiltMeasuringUnitService>.Instance);

        // Act
        await service.DeleteAsync(31, cancellationToken);

        // Assert
        repository.Verify(x => x.DeleteAsync(31, cancellationToken), Times.Once);
    }
}
