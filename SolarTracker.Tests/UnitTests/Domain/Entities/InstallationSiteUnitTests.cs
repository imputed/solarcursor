using Moq;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Domain.Entities;

public sealed class InstallationSiteUnitTests
{
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
