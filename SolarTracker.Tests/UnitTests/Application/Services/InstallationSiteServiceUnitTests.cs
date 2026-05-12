using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Interfaces.Calculators;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;
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
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ISolarPanelCalculator> solarPanelCalculator = new();
        repository.Setup(x => x.AddAsync(It.IsAny<InstallationSite>(), cancellationToken))
            .Callback<InstallationSite, CancellationToken>((entity, _) => entity.Id = 42)
            .Returns(ValueTask.CompletedTask);

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            solarPanelCalculator.Object,
            NullLogger<InstallationSiteService>.Instance);
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
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ISolarPanelCalculator> solarPanelCalculator = new();
        repository.Setup(x => x.UpdateAsync(It.IsAny<InstallationSite>(), cancellationToken))
            .Returns(ValueTask.CompletedTask);

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            solarPanelCalculator.Object,
            NullLogger<InstallationSiteService>.Instance);
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
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ISolarPanelCalculator> solarPanelCalculator = new();
        repository.Setup(x => x.DeleteAsync(9, cancellationToken))
            .Returns(ValueTask.CompletedTask);

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            solarPanelCalculator.Object,
            NullLogger<InstallationSiteService>.Instance);

        // Act
        await service.DeleteAsync(9, cancellationToken);

        // Assert
        repository.Verify(x => x.DeleteAsync(9, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task MoveToOptimumAsync_ShouldReturnNotFound_WhenInstallationSiteDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ISolarPanelCalculator> solarPanelCalculator = new();
        queryHandler.Setup(x => x.GetByIdAsync(5, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(null));

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            solarPanelCalculator.Object,
            NullLogger<InstallationSiteService>.Instance);

        // Act
        Result<IReadOnlyList<SolarPanelCurrentPositionDto>> result =
            await service.MoveToOptimumAsync(5, cancellationToken);

        // Assert
        Assert.True(result.IsNotFound);
        Assert.Equal(SolarTrackerErrorCatalog.InstallationSite.NotFound(5), result.Error);
        solarPanelCalculator.Verify(
            x => x.MoveToOptimumAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task MoveToOptimumAsync_ShouldMoveAllSolarPanels_WhenInstallationSiteExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteRepository> repository = new();
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ISolarPanelCalculator> solarPanelCalculator = new();
        InstallationSite installationSite = new()
        {
            Id = 5,
            Name = "Primary site",
            Latitude = 50.1m,
            Longitude = 8.6m,
            SolarPanels =
            [
                new SolarPanel { Id = 12, InstallationSiteId = 5, SerialNumber = "panel-12" },
                new SolarPanel { Id = 14, InstallationSiteId = 5, SerialNumber = "panel-14" },
            ],
        };
        Result<SolarPanelCurrentPositionDto> firstResult =
            Result<SolarPanelCurrentPositionDto>.Success(new SolarPanelCurrentPositionDto(12, 35.0d, 34.2d));
        Result<SolarPanelCurrentPositionDto> secondResult =
            Result<SolarPanelCurrentPositionDto>.Success(new SolarPanelCurrentPositionDto(14, 35.0d, 35.0d));
        queryHandler.Setup(x => x.GetByIdAsync(5, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(installationSite));
        solarPanelCalculator.Setup(x => x.MoveToOptimumAsync(12, cancellationToken))
            .Returns(ValueTask.FromResult(firstResult));
        solarPanelCalculator.Setup(x => x.MoveToOptimumAsync(14, cancellationToken))
            .Returns(ValueTask.FromResult(secondResult));

        InstallationSiteService service = new(
            repository.Object,
            queryHandler.Object,
            solarPanelCalculator.Object,
            NullLogger<InstallationSiteService>.Instance);

        // Act
        Result<IReadOnlyList<SolarPanelCurrentPositionDto>> result =
            await service.MoveToOptimumAsync(5, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal([firstResult.Value, secondResult.Value], result.Value);
        solarPanelCalculator.Verify(x => x.MoveToOptimumAsync(12, cancellationToken), Times.Once);
        solarPanelCalculator.Verify(x => x.MoveToOptimumAsync(14, cancellationToken), Times.Once);
    }
}
