using Moq;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class InstallationSiteServiceUnitTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistMappedEntityAndReturnId_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        repository.Setup(x => x.AddAsync(It.IsAny<InstallationSite>(), cancellationToken))
            .Callback<InstallationSite, CancellationToken>((entity, _) => entity.Id = 42)
            .Returns(ValueTask.CompletedTask);

        InstallationSiteService service = new(repository.Object);
        CreateInstallationSiteDto dto = new("Primary site", 50.1m, 8.6m);

        // Act
        int id = await service.AddAsync(dto, cancellationToken);

        // Assert
        Assert.Equal(42, id);
        repository.Verify(
            x => x.AddAsync(
                It.Is<InstallationSite>(entity =>
                    entity.Name == "Primary site" &&
                    entity.Latitude == 50.1m &&
                    entity.Longitude == 8.6m &&
                    entity.SolarPanels.Count == 0),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistMappedEntity_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        repository.Setup(x => x.UpdateAsync(It.IsAny<InstallationSite>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        InstallationSiteService service = new(repository.Object);
        UpdateInstallationSiteDto dto = new(7, "Updated site", 49.2m, 9.4m);

        // Act
        await service.UpdateAsync(dto, cancellationToken);

        // Assert
        repository.Verify(
            x => x.UpdateAsync(
                It.Is<InstallationSite>(entity =>
                    entity.Id == 7 &&
                    entity.Name == "Updated site" &&
                    entity.Latitude == 49.2m &&
                    entity.Longitude == 9.4m &&
                    entity.SolarPanels.Count == 0),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldForwardIdAndCancellationToken_WhenCalled()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        repository.Setup(x => x.DeleteAsync(9, cancellationToken))
            .Returns(ValueTask.CompletedTask);

        InstallationSiteService service = new(repository.Object);

        // Act
        await service.DeleteAsync(9, cancellationToken);

        // Assert
        repository.Verify(x => x.DeleteAsync(9, cancellationToken), Times.Once);
    }
}
