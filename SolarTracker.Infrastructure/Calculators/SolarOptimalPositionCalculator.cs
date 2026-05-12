using Innovative.Geometry;
using Innovative.SolarCalculator;
using SolarTracker.Domain.Abstractions;

namespace SolarTracker.Infrastructure.Calculators;

public sealed class SolarOptimalPositionCalculator : ISolarOptimalPositionCalculator
{
    public double CalculateOptimalPosition(decimal latitude, decimal longitude, DateTimeOffset timestamp)
    {
        SolarTimes solarTimes = new(timestamp, new Angle(latitude), new Angle(longitude));
        return Math.Clamp((double)solarTimes.SolarZenith, 0d, 90d);
    }
}
