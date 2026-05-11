namespace SolarTracker.Domain.Entities;

public sealed class SolarOptimizationScheduleConfiguration
{
    public const int SingletonId = 1;
    public const int DefaultIntervalMinutes = 10;

    public int Id { get; set; } = SingletonId;

    public int IntervalMinutes { get; set; } = DefaultIntervalMinutes;

    public static SolarOptimizationScheduleConfiguration CreateDefault() => new();
}
