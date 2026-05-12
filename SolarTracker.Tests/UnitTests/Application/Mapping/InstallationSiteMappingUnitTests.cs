using SolarTracker.Application.Dtos;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Application.Mapping;

public sealed class InstallationSiteMappingUnitTests
{
    [Fact]
    public void ToDto_ShouldMapAllScalarPropertiesAndSolarPanels_WhenEntityContainsChildren()
    {
        // Arrange
        InstallationSite entity = new()
        {
            Id = 5,
            Name = "Main site",
            Latitude = 50.123m,
            Longitude = 8.456m,
            SolarPanels =
            [
                new SolarPanel
                {
                    Id = 9,
                    InstallationSiteId = 5,
                    SerialNumber = "panel-9",
                    LinearMotors =
                    [
                        new LinearMotor
                        {
                            Id = 11,
                            SolarPanelId = 9,
                            Name = "Motor A",
                            MoveUpGpioPin = 17,
                            MoveDownGpioPin = 18,
                        },
                    ],
                },
            ],
        };

        // Act
        InstallationSiteDto dto = InstallationSiteMapping.ToDto(entity);

        // Assert
        Assert.Equal(5, dto.Id);
        Assert.Equal("Main site", dto.Name);
        Assert.Equal(50.123m, dto.Latitude);
        Assert.Equal(8.456m, dto.Longitude);
        Assert.Single(dto.SolarPanels);
        Assert.Equal(9, dto.SolarPanels[0].Id);
        Assert.Equal("panel-9", dto.SolarPanels[0].SerialNumber);
        Assert.Single(dto.SolarPanels[0].LinearMotors);
        Assert.Equal(11, dto.SolarPanels[0].LinearMotors[0].Id);
    }

    [Fact]
    public void ToDomain_ShouldMapCreateDtoAndInitializeSolarPanels_WhenCalled()
    {
        // Arrange
        CreateInstallationSiteDto dto = new("North site", 48.1m, 11.6m);

        // Act
        InstallationSite entity = InstallationSiteMapping.ToDomain(dto);

        // Assert
        Assert.Equal("North site", entity.Name);
        Assert.Equal(48.1m, entity.Latitude);
        Assert.Equal(11.6m, entity.Longitude);
        Assert.Empty(entity.SolarPanels);
    }

    [Fact]
    public void ToDomain_ShouldMapUpdateDtoAndInitializeSolarPanels_WhenCalled()
    {
        // Arrange
        UpdateInstallationSiteDto dto = new(7, "Updated site", 49.2m, 9.4m);

        // Act
        InstallationSite entity = InstallationSiteMapping.ToDomain(dto);

        // Assert
        Assert.Equal(7, entity.Id);
        Assert.Equal("Updated site", entity.Name);
        Assert.Equal(49.2m, entity.Latitude);
        Assert.Equal(9.4m, entity.Longitude);
        Assert.Empty(entity.SolarPanels);
    }
}
