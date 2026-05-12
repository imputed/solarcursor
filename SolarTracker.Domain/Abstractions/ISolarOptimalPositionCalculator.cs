namespace SolarTracker.Domain.Abstractions;

public interface ISolarOptimalPositionCalculator
{
    double CalculateOptimalPosition(decimal latitude, decimal longitude, DateTimeOffset timestamp);
}
