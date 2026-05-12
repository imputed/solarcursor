using Moq;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.Errors;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Tests.UnitTests.Domain.Entities;

public sealed class SolarPanelUnitTests
{
    [Fact]
    public async Task GetPosition_ShouldReturnTiltMeasurement_WhenTiltMeasuringUnitExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit tiltMeasuringUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = tiltMeasuringUnit,
        };
        TiltMeasurement expected = new(12.5d, new DateTime(2026, 12, 21, 12, 0, 0, DateTimeKind.Utc));
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        positionReader.Setup(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken))
            .Returns(ValueTask.FromResult(expected));

        // Act
        TiltMeasurement result = await solarPanel.GetPosition(positionReader.Object, cancellationToken);

        // Assert
        Assert.Equal(expected, result);
        positionReader.Verify(x => x.GetCurrentPositionAsync(tiltMeasuringUnit, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetPosition_ShouldThrow_WhenTiltMeasuringUnitDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = null,
        };
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();

        // Act
        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => solarPanel.GetPosition(positionReader.Object, cancellationToken).AsTask());

        // Assert
        Assert.Equal(DomainTextCatalog.SolarPanel.PositionRequiresTiltMeasuringUnit(), exception.Message);
    }
}
