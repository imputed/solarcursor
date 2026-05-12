using Moq;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.Errors;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Tests.UnitTests.Domain.Entities;

public sealed class InstallationSiteUnitTests
{
    [Fact]
    public async Task GetPosition_ShouldReturnAverageTiltMeasurement_WhenAllSolarPanelsHaveTiltMeasuringUnits()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        TiltMeasuringUnit firstUnit = new() { Id = 3, SolarPanelId = 10, GpioPin = 17 };
        TiltMeasuringUnit secondUnit = new() { Id = 4, SolarPanelId = 11, GpioPin = 18 };
        InstallationSite installationSite = new()
        {
            Id = 4,
            Name = "North site",
            Latitude = 50.1m,
            Longitude = 8.6m,
            SolarPanels =
            [
                new SolarPanel { Id = 10, InstallationSiteId = 4, TiltMeasuringUnit = firstUnit },
                new SolarPanel { Id = 11, InstallationSiteId = 4, TiltMeasuringUnit = secondUnit },
            ],
        };
        DateTime firstTimestamp = new(2026, 12, 21, 12, 0, 0, DateTimeKind.Utc);
        DateTime secondTimestamp = new(2026, 12, 21, 12, 0, 2, DateTimeKind.Utc);
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();
        positionReader.Setup(x => x.GetCurrentPositionAsync(firstUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(10d, firstTimestamp)));
        positionReader.Setup(x => x.GetCurrentPositionAsync(secondUnit, cancellationToken))
            .Returns(ValueTask.FromResult(new TiltMeasurement(20d, secondTimestamp)));

        // Act
        TiltMeasurement result = await installationSite.GetPosition(positionReader.Object, cancellationToken);

        // Assert
        Assert.Equal(15d, result.Degrees);
        Assert.Equal(secondTimestamp, result.Timestamp);
        positionReader.Verify(x => x.GetCurrentPositionAsync(firstUnit, cancellationToken), Times.Once);
        positionReader.Verify(x => x.GetCurrentPositionAsync(secondUnit, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetPosition_ShouldThrow_WhenSolarPanelDoesNotHaveTiltMeasuringUnit()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        InstallationSite installationSite = new()
        {
            Id = 4,
            Name = "North site",
            Latitude = 50.1m,
            Longitude = 8.6m,
            SolarPanels =
            [
                new SolarPanel { Id = 10, InstallationSiteId = 4, TiltMeasuringUnit = null },
            ],
        };
        Mock<ITiltMeasuringUnitPositionReader> positionReader = new();

        // Act
        InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => installationSite.GetPosition(positionReader.Object, cancellationToken).AsTask());

        // Assert
        Assert.Equal(
            DomainTextCatalog.SolarPanel.PositionRequiresTiltMeasuringUnit(),
            exception.Message);
    }

    [Fact]
    public void GetOptimalPosition_ShouldDelegateCoordinatesAndTimestampToCalculator_WhenInvoked()
    {
        // Arrange
        InstallationSite installationSite = new()
        {
            Id = 4,
            Name = "North site",
            Latitude = 50.1m,
            Longitude = 8.6m,
        };
        DateTimeOffset timestamp = new(2026, 12, 21, 12, 0, 0, TimeSpan.Zero);
        Mock<ISolarOptimalPositionCalculator> calculator = new();
        calculator.Setup(x => x.CalculateOptimalPosition(50.1m, 8.6m, timestamp))
            .Returns(37.5d);

        // Act
        double result = installationSite.GetOptimalPosition(calculator.Object, timestamp);

        // Assert
        Assert.Equal(37.5d, result);
    }
}
