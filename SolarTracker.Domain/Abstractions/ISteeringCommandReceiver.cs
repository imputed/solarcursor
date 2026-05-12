namespace SolarTracker.Domain.Abstractions;

public interface ISteeringCommandReceiver
{
    ValueTask MoveUpAsync(int moveUpPin, int moveDownPin, CancellationToken cancellationToken);

    ValueTask MoveDownAsync(int moveUpPin, int moveDownPin, CancellationToken cancellationToken);

    ValueTask StopAsync(int moveUpPin, int moveDownPin, CancellationToken cancellationToken);
}
