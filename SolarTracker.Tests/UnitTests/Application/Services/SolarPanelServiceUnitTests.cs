using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Application.Dtos.SolarPanel;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Services;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class SolarPanelServiceUnitTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistMappedEntityAndReturnId_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelRepository> repository = new();
        repository.Setup(x => x.AddAsync(It.IsAny<SolarPanel>(), cancellationToken))
            .Callback<SolarPanel, CancellationToken>((entity, _) => entity.Id = 44)
            .Returns(ValueTask.CompletedTask);

        SolarPanelService service = new(repository.Object, NullLogger<SolarPanelService>.Instance);
        CreateSolarPanelDto dto = new(12, "panel-44");

        // Act
        int id = await service.AddAsync(dto, cancellationToken);

        // Assert
        Assert.Equal(44, id);
        repository.Verify(
            x => x.AddAsync(
                It.Is<SolarPanel>(entity =>
                    entity.InstallationSiteId == 12 &&
                    entity.SerialNumber == "panel-44" &&
                    entity.SolarTrackingConfiguration == null &&
                    entity.TiltMeasuringUnit == null &&
                    entity.CurrentMeasuringUnit == null &&
                    entity.LinearMotors.Count == 0),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistMappedEntity_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelRepository> repository = new();
        repository.Setup(x => x.UpdateAsync(It.IsAny<SolarPanel>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        SolarPanelService service = new(repository.Object, NullLogger<SolarPanelService>.Instance);
        UpdateSolarPanelDto dto = new(44, 12, "panel-44-updated");

        // Act
        await service.UpdateAsync(dto, cancellationToken);

        // Assert
        repository.Verify(
            x => x.UpdateAsync(
                It.Is<SolarPanel>(entity =>
                    entity.Id == 44 &&
                    entity.InstallationSiteId == 12 &&
                    entity.SerialNumber == "panel-44-updated" &&
                    entity.LinearMotors.Count == 0),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldForwardIdAndCancellationToken_WhenCalled()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarPanelRepository> repository = new();
        repository.Setup(x => x.DeleteAsync(44, cancellationToken))
            .Returns(ValueTask.CompletedTask);

        SolarPanelService service = new(repository.Object, NullLogger<SolarPanelService>.Instance);

        // Act
        await service.DeleteAsync(44, cancellationToken);

        // Assert
        repository.Verify(x => x.DeleteAsync(44, cancellationToken), Times.Once);
    }
}
