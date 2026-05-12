using SolarTracker.Application.Dtos;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Mapping;

public sealed class SolarPanelMappingUnitTests
{
    [Fact]
    public void ToDto_ShouldMapNestedUnitsAndLinearMotors_WhenEntityContainsChildren()
    {
        // Arrange
        SolarPanel entity = new()
        {
            Id = 4,
            InstallationSiteId = 2,
            SerialNumber = "panel-4",
            TiltMeasuringUnit = new TiltMeasuringUnit
            {
                Id = 6,
                SolarPanelId = 4,
                Name = "Tilt sensor",
                GpioPin = 13,
            },
            CurrentMeasuringUnit = new CurrentMeasuringUnit
            {
                Id = 7,
                SolarPanelId = 4,
                Name = "Current sensor",
                GpioPin = 19,
            },
            LinearMotors =
            [
                new LinearMotor
                {
                    Id = 8,
                    SolarPanelId = 4,
                    Name = "Motor A",
                    MoveUpGpioPin = 20,
                    MoveDownGpioPin = 21,
                },
            ],
        };

        // Act
        SolarPanelDto dto = SolarPanelMapping.ToDto(entity);

        // Assert
        Assert.Equal(4, dto.Id);
        Assert.Equal(2, dto.InstallationSiteId);
        Assert.Equal("panel-4", dto.SerialNumber);
        Assert.Equal(new TiltMeasuringUnitDto(6, 4, "Tilt sensor", 13), dto.TiltMeasuringUnit);
        Assert.Equal(new CurrentMeasuringUnitDto(7, 4, "Current sensor", 19), dto.CurrentMeasuringUnit);
        Assert.Single(dto.LinearMotors);
        Assert.Equal(new LinearMotorDto(8, 4, "Motor A", 20, 21), dto.LinearMotors[0]);
    }

    [Fact]
    public void ToDto_ShouldKeepOptionalUnitsNull_WhenEntityDoesNotContainThem()
    {
        // Arrange
        SolarPanel entity = new()
        {
            Id = 5,
            InstallationSiteId = 3,
            SerialNumber = "panel-5",
            TiltMeasuringUnit = null,
            CurrentMeasuringUnit = null,
            LinearMotors = [],
        };

        // Act
        SolarPanelDto dto = SolarPanelMapping.ToDto(entity);

        // Assert
        Assert.Null(dto.TiltMeasuringUnit);
        Assert.Null(dto.CurrentMeasuringUnit);
        Assert.Empty(dto.LinearMotors);
    }

    [Fact]
    public void ToDomain_ShouldMapCreateDtoAndInitializeChildReferences_WhenCalled()
    {
        // Arrange
        CreateSolarPanelDto dto = new(12, "new-panel");

        // Act
        SolarPanel entity = SolarPanelMapping.ToDomain(dto);

        // Assert
        Assert.Equal(12, entity.InstallationSiteId);
        Assert.Equal("new-panel", entity.SerialNumber);
        Assert.Null(entity.SolarTrackingConfiguration);
        Assert.Null(entity.TiltMeasuringUnit);
        Assert.Null(entity.CurrentMeasuringUnit);
        Assert.Empty(entity.LinearMotors);
    }

    [Fact]
    public void ToDomain_ShouldMapUpdateDtoAndInitializeChildReferences_WhenCalled()
    {
        // Arrange
        UpdateSolarPanelDto dto = new(14, 12, "updated-panel");

        // Act
        SolarPanel entity = SolarPanelMapping.ToDomain(dto);

        // Assert
        Assert.Equal(14, entity.Id);
        Assert.Equal(12, entity.InstallationSiteId);
        Assert.Equal("updated-panel", entity.SerialNumber);
        Assert.Null(entity.SolarTrackingConfiguration);
        Assert.Null(entity.TiltMeasuringUnit);
        Assert.Null(entity.CurrentMeasuringUnit);
        Assert.Empty(entity.LinearMotors);
    }
}
