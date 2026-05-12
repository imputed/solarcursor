using SolarTracker.Application.Analysis;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.QueryHandlers;

public interface ILinearMotorQueryHandler
{
    ValueTask<LinearMotor?> GetByIdAsync(int id, CancellationToken cancellationToken);

    ValueTask<IReadOnlyList<LinearMotor>> AnalyzeAsync(LinearMotorAnalyzeRequest request, CancellationToken cancellationToken);
}
