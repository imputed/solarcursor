using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarTracker.IoC;

namespace SolarTracker.Tests.UnitTests.IoC;

public sealed class DependencyInjectionUnitTests
{
    [Fact]
    public void AddSolarInfrastructure_ShouldThrow_WhenConnectionStringIsMissing()
    {
        // Arrange
        ServiceCollection services = new();
        IConfiguration configuration = new ConfigurationBuilder().Build();

        // Act
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => services.AddSolarInfrastructure(configuration));

        // Assert
        Assert.Contains("Connection string 'SolarTracker' must be configured", exception.Message);
    }
}
