using SolarTracker.Application.Dtos.CurrentMeasuringUnit;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Mapping;

public sealed class CurrentMeasuringUnitMappingUnitTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties_WhenEntityIsValid()
    {
        // Arrange
        CurrentMeasuringUnit entity = new() { Id = 4, SolarPanelId = 6, Name = "Current sensor", GpioPin = 21 };

        // Act
        CurrentMeasuringUnitDto dto = CurrentMeasuringUnitMapping.ToDto(entity);

        // Assert
        Assert.Equal(new CurrentMeasuringUnitDto(4, 6, "Current sensor", 21), dto);
    }

    [Fact]
    public void ToDomain_ShouldMapCreateDto_WhenCalled()
    {
        // Arrange
        CreateCurrentMeasuringUnitDto dto = new(6, "Current sensor", 21);

        // Act
        CurrentMeasuringUnit entity = CurrentMeasuringUnitMapping.ToDomain(dto);

        // Assert
        Assert.Equal(6, entity.SolarPanelId);
        Assert.Equal("Current sensor", entity.Name);
        Assert.Equal(21, entity.GpioPin);
    }

    [Fact]
    public void ToDomain_ShouldMapUpdateDto_WhenCalled()
    {
        // Arrange
        UpdateCurrentMeasuringUnitDto dto = new(4, 6, "Updated sensor", 22);

        // Act
        CurrentMeasuringUnit entity = CurrentMeasuringUnitMapping.ToDomain(dto);

        // Assert
        Assert.Equal(4, entity.Id);
        Assert.Equal(6, entity.SolarPanelId);
        Assert.Equal("Updated sensor", entity.Name);
        Assert.Equal(22, entity.GpioPin);
    }
}
