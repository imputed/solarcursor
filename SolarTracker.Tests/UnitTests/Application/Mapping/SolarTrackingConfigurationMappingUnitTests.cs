using SolarTracker.Application.Dtos;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Mapping;

public sealed class SolarTrackingConfigurationMappingUnitTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties_WhenEntityIsValid()
    {
        // Arrange
        SolarTrackingConfiguration entity = new()
        {
            SolarPanelId = 7,
            PositionThresholdDegrees = 2.5d,
            StepDurationMs = 750,
            MaxAdjustmentSteps = 30,
        };

        // Act
        SolarTrackingConfigurationDto dto = SolarTrackingConfigurationMapping.ToDto(entity);

        // Assert
        Assert.Equal(new SolarTrackingConfigurationDto(7, 2.5d, 750, 30), dto);
    }

    [Fact]
    public void ToDomain_ShouldMapSolarPanelIdAndUpdateDto_WhenCalled()
    {
        // Arrange
        UpdateSolarTrackingConfigurationDto dto = new(3.5d, 900, 45);

        // Act
        SolarTrackingConfiguration entity = SolarTrackingConfigurationMapping.ToDomain(9, dto);

        // Assert
        Assert.Equal(9, entity.SolarPanelId);
        Assert.Equal(3.5d, entity.PositionThresholdDegrees);
        Assert.Equal(900, entity.StepDurationMs);
        Assert.Equal(45, entity.MaxAdjustmentSteps);
    }
}
