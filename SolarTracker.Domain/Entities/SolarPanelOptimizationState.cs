namespace SolarTracker.Domain.Entities;

public sealed class SolarPanelOptimizationState
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public bool IsEnabled { get; set; }

    public static SolarPanelOptimizationState CreateDefault(int solarPanelId) =>
        new() { SolarPanelId = solarPanelId, IsEnabled = false };
}
