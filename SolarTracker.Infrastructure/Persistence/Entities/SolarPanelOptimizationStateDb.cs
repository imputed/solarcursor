namespace SolarTracker.Infrastructure.Persistence.Entities;

public sealed class SolarPanelOptimizationStateDb
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public bool IsEnabled { get; set; }

    public SolarPanelDb SolarPanel { get; set; } = null!;
}
