using SolarTracker.Application.Analysis;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Application.Interfaces.Services;

public interface ILinearMotorService
{
    ValueTask<LinearMotorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<LinearMotorDto>> AnalyzeAsync(
        LinearMotorAnalyzeRequest request,
        CancellationToken cancellationToken = default);

    ValueTask<IReadOnlyList<LinearMotorDto>> ListAsync(CancellationToken cancellationToken = default);

    ValueTask<int> AddAsync(CreateLinearMotorDto dto, CancellationToken cancellationToken = default);

    ValueTask UpdateAsync(UpdateLinearMotorDto dto, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default);
}
