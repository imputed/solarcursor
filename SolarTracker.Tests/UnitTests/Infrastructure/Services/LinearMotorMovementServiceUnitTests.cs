using Microsoft.Extensions.Logging;
using Moq;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;
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
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ILinearMotorActuator> actuator = new();
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(4, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(null));

        LinearMotorMovementService service = new(
            linearMotorQueryHandler.Object,
            solarPanelQueryHandler.Object,
            installationSiteQueryHandler.Object,
            actuator.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());

        // Act
        var result = await service.MoveUpAsync(4, 600, cancellationToken);

        // Assert
        Assert.True(result.IsNotFound);
        Assert.Equal("linear-motor-not-found", result.Error?.Code);
        actuator.Verify(x => x.MoveUpAsync(It.IsAny<LinearMotorMovementContext>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task MoveUpAsync_ShouldReturnNotFound_WhenSolarPanelDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ILinearMotorQueryHandler> linearMotorQueryHandler = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ILinearMotorActuator> actuator = new();
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(4, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(new LinearMotor
            {
                Id = 4,
                SolarPanelId = 9,
                MoveUpGpioPin = 17,
                MoveDownGpioPin = 18,
            }));
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(9, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(null));

        LinearMotorMovementService service = new(
            linearMotorQueryHandler.Object,
            solarPanelQueryHandler.Object,
            installationSiteQueryHandler.Object,
            actuator.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());

        // Act
        var result = await service.MoveUpAsync(4, 600, cancellationToken);

        // Assert
        Assert.True(result.IsNotFound);
        Assert.Equal("solar-panel-not-found", result.Error?.Code);
        actuator.Verify(x => x.MoveUpAsync(It.IsAny<LinearMotorMovementContext>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task MoveDownAsync_ShouldReturnNotFound_WhenInstallationSiteDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ILinearMotorQueryHandler> linearMotorQueryHandler = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ILinearMotorActuator> actuator = new();
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(4, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(CreateLinearMotor()));
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(9, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(new SolarPanel { Id = 9, InstallationSiteId = 12 }));
        installationSiteQueryHandler.Setup(x => x.GetByIdAsync(12, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(null));

        LinearMotorMovementService service = new(
            linearMotorQueryHandler.Object,
            solarPanelQueryHandler.Object,
            installationSiteQueryHandler.Object,
            actuator.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());

        // Act
        var result = await service.MoveDownAsync(4, 600, cancellationToken);

        // Assert
        Assert.True(result.IsNotFound);
        Assert.Equal("installation-site-not-found", result.Error?.Code);
        actuator.Verify(x => x.MoveDownAsync(It.IsAny<LinearMotorMovementContext>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task MoveUpAsync_ShouldBuildContextAndCallActuator_WhenAllDependenciesExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ILinearMotorQueryHandler> linearMotorQueryHandler = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ILinearMotorActuator> actuator = new();
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(4, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(CreateLinearMotor()));
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(9, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(new SolarPanel { Id = 9, InstallationSiteId = 12 }));
        installationSiteQueryHandler.Setup(x => x.GetByIdAsync(12, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(new InstallationSite
            {
                Id = 12,
                Latitude = 50.123m,
                Longitude = 8.456m,
                Name = "Main site",
            }));

        LinearMotorMovementService service = new(
            linearMotorQueryHandler.Object,
            solarPanelQueryHandler.Object,
            installationSiteQueryHandler.Object,
            actuator.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());

        // Act
        var result = await service.MoveUpAsync(4, 650, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        actuator.Verify(
            x => x.MoveUpAsync(
                It.Is<LinearMotorMovementContext>(context =>
                    context.LinearMotorId == 4 &&
                    context.InstallationSiteId == 12 &&
                    context.Latitude == 50.123m &&
                    context.Longitude == 8.456m &&
                    context.MoveUpGpioPin == 17 &&
                    context.MoveDownGpioPin == 18 &&
                    context.DurationMs == 650),
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task MoveDownAsync_ShouldBuildContextAndCallActuator_WhenAllDependenciesExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ILinearMotorQueryHandler> linearMotorQueryHandler = new();
        Mock<ISolarPanelQueryHandler> solarPanelQueryHandler = new();
        Mock<IInstallationSiteQueryHandler> installationSiteQueryHandler = new();
        Mock<ILinearMotorActuator> actuator = new();
        linearMotorQueryHandler.Setup(x => x.GetByIdAsync(4, cancellationToken))
            .Returns(ValueTask.FromResult<LinearMotor?>(CreateLinearMotor()));
        solarPanelQueryHandler.Setup(x => x.GetByIdAsync(9, cancellationToken))
            .Returns(ValueTask.FromResult<SolarPanel?>(new SolarPanel { Id = 9, InstallationSiteId = 12 }));
        installationSiteQueryHandler.Setup(x => x.GetByIdAsync(12, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(new InstallationSite
            {
                Id = 12,
                Latitude = 50.123m,
                Longitude = 8.456m,
                Name = "Main site",
            }));

        LinearMotorMovementService service = new(
            linearMotorQueryHandler.Object,
            solarPanelQueryHandler.Object,
            installationSiteQueryHandler.Object,
            actuator.Object,
            Mock.Of<ILogger<LinearMotorMovementService>>());

        // Act
        var result = await service.MoveDownAsync(4, 700, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        actuator.Verify(
            x => x.MoveDownAsync(
                It.Is<LinearMotorMovementContext>(context =>
                    context.LinearMotorId == 4 &&
                    context.InstallationSiteId == 12 &&
                    context.Latitude == 50.123m &&
                    context.Longitude == 8.456m &&
                    context.MoveUpGpioPin == 17 &&
                    context.MoveDownGpioPin == 18 &&
                    context.DurationMs == 700),
                cancellationToken),
            Times.Once);
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
