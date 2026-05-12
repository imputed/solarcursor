using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Services;

public sealed class SolarOptimizationScheduleConfigurationServiceUnitTests
{
    [Fact]
    public async Task GetAsync_ShouldReturnMappedDto_WhenConfigurationExists()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarOptimizationScheduleConfigurationRepository> repository = new();
        repository.Setup(x => x.GetAsync(cancellationToken))
            .Returns(ValueTask.FromResult(new SolarOptimizationScheduleConfiguration
            {
                IntervalMinutes = 15,
            }));

        SolarOptimizationScheduleConfigurationService service =
            new(repository.Object, NullLogger<SolarOptimizationScheduleConfigurationService>.Instance);

        // Act
        SolarOptimizationScheduleConfigurationDto result = await service.GetAsync(cancellationToken);

        // Assert
        Assert.Equal(new SolarOptimizationScheduleConfigurationDto(15), result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistMappedEntityAndReturnMappedDto_WhenDtoIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<ISolarOptimizationScheduleConfigurationRepository> repository = new();
        repository.Setup(x => x.UpsertAsync(
                It.Is<SolarOptimizationScheduleConfiguration>(entity =>
                    entity.Id == SolarOptimizationScheduleConfiguration.SingletonId &&
                    entity.IntervalMinutes == 20),
                cancellationToken))
            .Returns(ValueTask.FromResult(new SolarOptimizationScheduleConfiguration
            {
                IntervalMinutes = 20,
            }));

        SolarOptimizationScheduleConfigurationService service =
            new(repository.Object, NullLogger<SolarOptimizationScheduleConfigurationService>.Instance);
        UpdateSolarOptimizationScheduleConfigurationDto dto = new(20);

        // Act
        SolarOptimizationScheduleConfigurationDto result = await service.UpdateAsync(dto, cancellationToken);

        // Assert
        Assert.Equal(new SolarOptimizationScheduleConfigurationDto(20), result);
    }
}
