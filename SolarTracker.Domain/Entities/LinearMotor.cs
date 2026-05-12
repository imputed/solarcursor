namespace SolarTracker.Domain.Entities;

public sealed class LinearMotor
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public string? Name { get; set; }

    public int MoveUpGpioPin { get; set; }

    public int MoveDownGpioPin { get; set; }

    public ValueTask MoveUpAsync(
        Func<LinearMotor, int, CancellationToken, ValueTask> action,
        int durationMs,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(action);
        return action(this, durationMs, cancellationToken);
    }

    public ValueTask MoveDownAsync(
        Func<LinearMotor, int, CancellationToken, ValueTask> action,
        int durationMs,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(action);
        return action(this, durationMs, cancellationToken);
    }
}
