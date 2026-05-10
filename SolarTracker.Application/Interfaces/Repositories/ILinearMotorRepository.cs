using SolarTracker.Application.Analysis;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface ILinearMotorRepository
{
    ValueTask<LinearMotor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<LinearMotor>> AnalyzeAsync(
        LinearMotorAnalyzeRequest request,
        CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<LinearMotor>> ListAsync(CancellationToken cancellationToken = default);

    ValueTask AddAsync(LinearMotor entity, CancellationToken cancellationToken = default);

    ValueTask UpdateAsync(LinearMotor entity, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(LinearMotor entity, CancellationToken cancellationToken = default);
}
