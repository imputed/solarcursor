using SolarTracker.Domain.Abstractions;

namespace SolarTracker.Domain.Entities;

public sealed class InstallationSite
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public ICollection<SolarPanel> SolarPanels { get; set; } = [];

    public double GetOptimalPosition(ISolarOptimalPositionCalculator calculator, DateTimeOffset timestamp)
    {
        ArgumentNullException.ThrowIfNull(calculator);
        return calculator.CalculateOptimalPosition(Latitude, Longitude, timestamp);
    }
}
