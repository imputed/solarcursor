using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Tests.UnitTests.Domain.Entities;

public sealed class SolarPanelMovementValidationUnitTests
{
    [Fact]
    public void ValidateSolarPanelForMovement_ShouldReturnTiltMeasuringUnitMissing_WhenTiltMeasuringUnitDoesNotExist()
    {
        // Arrange
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = null,
            LinearMotors = [new LinearMotor { Id = 20, SolarPanelId = 10, MoveUpGpioPin = 11, MoveDownGpioPin = 12 }],
        };

        // Act
        SolarPanelMovementValidationResult result = solarPanel.ValidateSolarPanelForMovement();

        // Assert
        Assert.Equal(SolarPanelMovementValidationResult.TiltMeasuringUnitMissing, result);
    }

    [Fact]
    public void ValidateSolarPanelForMovement_ShouldReturnLinearMotorsMissing_WhenLinearMotorsDoNotExist()
    {
        // Arrange
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = new TiltMeasuringUnit { Id = 3, SolarPanelId = 10, GpioPin = 17 },
            LinearMotors = [],
        };

        // Act
        SolarPanelMovementValidationResult result = solarPanel.ValidateSolarPanelForMovement();

        // Assert
        Assert.Equal(SolarPanelMovementValidationResult.LinearMotorsMissing, result);
    }

    [Fact]
    public void ValidateSolarPanelForMovement_ShouldReturnValid_WhenTiltMeasuringUnitAndLinearMotorsExist()
    {
        // Arrange
        SolarPanel solarPanel = new()
        {
            Id = 10,
            InstallationSiteId = 4,
            TiltMeasuringUnit = new TiltMeasuringUnit { Id = 3, SolarPanelId = 10, GpioPin = 17 },
            LinearMotors = [new LinearMotor { Id = 20, SolarPanelId = 10, MoveUpGpioPin = 11, MoveDownGpioPin = 12 }],
        };

        // Act
        SolarPanelMovementValidationResult result = solarPanel.ValidateSolarPanelForMovement();

        // Assert
        Assert.Equal(SolarPanelMovementValidationResult.Valid, result);
    }
}
