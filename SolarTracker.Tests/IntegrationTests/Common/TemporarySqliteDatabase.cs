namespace SolarTracker.Tests.IntegrationTests.Common;

public sealed class TemporarySqliteDatabase : IAsyncDisposable
{
    private readonly string databasePath;

    public TemporarySqliteDatabase()
    {
        databasePath = Path.Combine(Path.GetTempPath(), $"solartracker-tests-{Guid.NewGuid():N}.db");
    }

    public string ConnectionString => $"Data Source={databasePath};Cache=Shared";

    public ValueTask DisposeAsync()
    {
        if (File.Exists(databasePath))
            File.Delete(databasePath);

        return ValueTask.CompletedTask;
    }
}
