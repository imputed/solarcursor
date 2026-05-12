using SolarTracker.Application.Dtos.TiltMeasuringUnit;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Mapping;

public sealed class TiltMeasuringUnitMappingUnitTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties_WhenEntityIsValid()
    {
        // Arrange
        TiltMeasuringUnit entity = new() { Id = 4, SolarPanelId = 6, Name = "Tilt sensor", GpioPin = 12 };

        // Act
        TiltMeasuringUnitDto dto = TiltMeasuringUnitMapping.ToDto(entity);

        // Assert
        Assert.Equal(new TiltMeasuringUnitDto(4, 6, "Tilt sensor", 12), dto);
    }

    [Fact]
    public void ToDomain_ShouldMapCreateDto_WhenCalled()
    {
        // Arrange
        CreateTiltMeasuringUnitDto dto = new(6, "Tilt sensor", 12);

        // Act
        TiltMeasuringUnit entity = TiltMeasuringUnitMapping.ToDomain(dto);

        // Assert
        Assert.Equal(6, entity.SolarPanelId);
        Assert.Equal("Tilt sensor", entity.Name);
        Assert.Equal(12, entity.GpioPin);
    }

    [Fact]
    public void ToDomain_ShouldMapUpdateDto_WhenCalled()
    {
        // Arrange
        UpdateTiltMeasuringUnitDto dto = new(4, 6, "Updated tilt sensor", 13);

        // Act
        TiltMeasuringUnit entity = TiltMeasuringUnitMapping.ToDomain(dto);

        // Assert
        Assert.Equal(4, entity.Id);
        Assert.Equal(6, entity.SolarPanelId);
        Assert.Equal("Updated tilt sensor", entity.Name);
        Assert.Equal(13, entity.GpioPin);
    }
}
