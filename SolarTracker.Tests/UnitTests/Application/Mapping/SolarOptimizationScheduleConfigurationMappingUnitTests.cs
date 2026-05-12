using SolarTracker.Application.Dtos;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Mapping;

public sealed class SolarOptimizationScheduleConfigurationMappingUnitTests
{
    [Fact]
    public void ToDto_ShouldMapIntervalMinutes_WhenEntityIsValid()
    {
        // Arrange
        SolarOptimizationScheduleConfiguration entity = new() { IntervalMinutes = 15 };

        // Act
        SolarOptimizationScheduleConfigurationDto dto = SolarOptimizationScheduleConfigurationMapping.ToDto(entity);

        // Assert
        Assert.Equal(new SolarOptimizationScheduleConfigurationDto(15), dto);
    }

    [Fact]
    public void ToDomain_ShouldUseSingletonIdAndMapUpdateDto_WhenCalled()
    {
        // Arrange
        UpdateSolarOptimizationScheduleConfigurationDto dto = new(20);

        // Act
        SolarOptimizationScheduleConfiguration entity = SolarOptimizationScheduleConfigurationMapping.ToDomain(dto);

        // Assert
        Assert.Equal(SolarOptimizationScheduleConfiguration.SingletonId, entity.Id);
        Assert.Equal(20, entity.IntervalMinutes);
    }
}
