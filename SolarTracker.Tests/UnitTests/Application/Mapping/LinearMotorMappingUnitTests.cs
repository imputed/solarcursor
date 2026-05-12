using SolarTracker.Application.Dtos;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Mapping;

public sealed class LinearMotorMappingUnitTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties_WhenEntityIsValid()
    {
        // Arrange
        LinearMotor entity = new()
        {
            Id = 8,
            SolarPanelId = 5,
            Name = "Motor A",
            MoveUpGpioPin = 17,
            MoveDownGpioPin = 18,
        };

        // Act
        LinearMotorDto dto = LinearMotorMapping.ToDto(entity);

        // Assert
        Assert.Equal(new LinearMotorDto(8, 5, "Motor A", 17, 18), dto);
    }

    [Fact]
    public void ToDomain_ShouldMapCreateDto_WhenCalled()
    {
        // Arrange
        CreateLinearMotorDto dto = new(5, "Motor A", 17, 18);

        // Act
        LinearMotor entity = LinearMotorMapping.ToDomain(dto);

        // Assert
        Assert.Equal(5, entity.SolarPanelId);
        Assert.Equal("Motor A", entity.Name);
        Assert.Equal(17, entity.MoveUpGpioPin);
        Assert.Equal(18, entity.MoveDownGpioPin);
    }

    [Fact]
    public void ToDomain_ShouldMapUpdateDto_WhenCalled()
    {
        // Arrange
        UpdateLinearMotorDto dto = new(8, 5, "Motor B", 19, 20);

        // Act
        LinearMotor entity = LinearMotorMapping.ToDomain(dto);

        // Assert
        Assert.Equal(8, entity.Id);
        Assert.Equal(5, entity.SolarPanelId);
        Assert.Equal("Motor B", entity.Name);
        Assert.Equal(19, entity.MoveUpGpioPin);
        Assert.Equal(20, entity.MoveDownGpioPin);
    }
}
