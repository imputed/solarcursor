using SolarTracker.Application.Dtos.SolarPanelOptimizationState;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Mapping;

public sealed class SolarPanelOptimizationStateMappingUnitTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties_WhenEntityIsValid()
    {
        // Arrange
        SolarPanelOptimizationState entity = new() { SolarPanelId = 9, IsEnabled = true };

        // Act
        SolarPanelOptimizationStateDto dto = SolarPanelOptimizationStateMapping.ToDto(entity);

        // Assert
        Assert.Equal(new SolarPanelOptimizationStateDto(9, true), dto);
    }

    [Fact]
    public void ToDomain_ShouldMapSolarPanelIdAndUpdateDto_WhenCalled()
    {
        // Arrange
        UpdateSolarPanelOptimizationStateDto dto = new(true);

        // Act
        SolarPanelOptimizationState entity = SolarPanelOptimizationStateMapping.ToDomain(9, dto);

        // Assert
        Assert.Equal(9, entity.SolarPanelId);
        Assert.True(entity.IsEnabled);
    }
}
