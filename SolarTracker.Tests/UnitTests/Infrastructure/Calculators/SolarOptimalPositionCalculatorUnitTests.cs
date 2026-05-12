using SolarTracker.Infrastructure.Calculators;

namespace SolarTracker.Tests.UnitTests.Infrastructure.Calculators;

public sealed class SolarOptimalPositionCalculatorUnitTests
{
    [Fact]
    public void CalculateOptimalPosition_ShouldReturnValueWithinValidTiltRange_WhenCalled()
    {
        // Arrange
        SolarOptimalPositionCalculator calculator = new();

        // Act
        double result = calculator.CalculateOptimalPosition(50.1m, 8.6m, new DateTimeOffset(2026, 6, 21, 12, 0, 0, TimeSpan.Zero));

        // Assert
        Assert.InRange(result, 0d, 90d);
    }
}
