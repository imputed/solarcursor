namespace SolarTracker.Domain.ValueObjects;

public readonly record struct TiltMeasurement(double Degrees, DateTime Timestamp);
