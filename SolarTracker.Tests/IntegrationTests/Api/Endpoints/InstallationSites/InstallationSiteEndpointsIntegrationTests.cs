using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using SolarTracker.Application.Dtos;
using SolarTracker.Infrastructure.Persistence.Entities;
using SolarTracker.Tests.IntegrationTests.Common;

namespace SolarTracker.Tests.IntegrationTests.Api.Endpoints.InstallationSites;

public sealed class InstallationSiteEndpointsIntegrationTests(SqliteApiIntegrationTestFixture fixture)
    : IClassFixture<SqliteApiIntegrationTestFixture>
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnInstallationSite_WhenSiteExists()
    {
        // Arrange
        await fixture.ResetDatabaseAsync();
        int siteId = await fixture.ExecuteDbContextAsync(async dbContext =>
        {
            InstallationSiteDb row = new() { Name = "Main site", Latitude = 51.0m, Longitude = 7.4m };
            await dbContext.InstallationSites.AddAsync(row);
            await dbContext.SaveChangesAsync();
            return row.Id;
        });

        // Act
        HttpResponseMessage response = await fixture.Client.GetAsync($"/api/installation-sites/{siteId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        InstallationSiteDto? payload = await response.Content.ReadFromJsonAsync<InstallationSiteDto>();

        Assert.NotNull(payload);
        Assert.Equal(siteId, payload.Id);
        Assert.Equal("Main site", payload.Name);
        Assert.Equal(51.0m, payload.Latitude);
        Assert.Equal(7.4m, payload.Longitude);
        Assert.Empty(payload.SolarPanels);
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistInstallationSiteAndReturnCreated_WhenRequestIsValid()
    {
        // Arrange
        await fixture.ResetDatabaseAsync();
        CreateInstallationSiteDto request = new("Created site", 49.5m, 8.1m);

        // Act
        HttpResponseMessage response = await fixture.Client.PostAsJsonAsync("/api/installation-sites", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        InstallationSiteDto? payload = await response.Content.ReadFromJsonAsync<InstallationSiteDto>();

        Assert.NotNull(payload);
        Assert.True(payload.Id > 0);
        Assert.Equal("Created site", payload.Name);
        Assert.Equal(49.5m, payload.Latitude);
        Assert.Equal(8.1m, payload.Longitude);
        Assert.Equal($"/api/installation-sites/{payload.Id}", response.Headers.Location?.OriginalString);

        InstallationSiteDb persisted = await fixture.ExecuteDbContextAsync(async dbContext =>
            await dbContext.InstallationSites.SingleAsync(site => site.Id == payload.Id));

        Assert.Equal("Created site", persisted.Name);
        Assert.Equal(49.5m, persisted.Latitude);
        Assert.Equal(8.1m, persisted.Longitude);
    }

    [Fact]
    public async Task PutAsync_ShouldUpdateInstallationSite_WhenRequestIsValid()
    {
        // Arrange
        await fixture.ResetDatabaseAsync();
        int siteId = await fixture.ExecuteDbContextAsync(async dbContext =>
        {
            InstallationSiteDb row = new() { Name = "Before update", Latitude = 45.0m, Longitude = 10.0m };
            await dbContext.InstallationSites.AddAsync(row);
            await dbContext.SaveChangesAsync();
            return row.Id;
        });
        UpdateInstallationSiteDto request = new(siteId, "After update", 46.5m, 11.5m);

        // Act
        HttpResponseMessage response = await fixture.Client.PutAsJsonAsync($"/api/installation-sites/{siteId}", request);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        InstallationSiteDb updated = await fixture.ExecuteDbContextAsync(async dbContext =>
            await dbContext.InstallationSites.SingleAsync(site => site.Id == siteId));

        Assert.Equal("After update", updated.Name);
        Assert.Equal(46.5m, updated.Latitude);
        Assert.Equal(11.5m, updated.Longitude);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveInstallationSite_WhenSiteExists()
    {
        // Arrange
        await fixture.ResetDatabaseAsync();
        int siteId = await fixture.ExecuteDbContextAsync(async dbContext =>
        {
            InstallationSiteDb row = new() { Name = "Delete me", Latitude = 47.0m, Longitude = 12.0m };
            await dbContext.InstallationSites.AddAsync(row);
            await dbContext.SaveChangesAsync();
            return row.Id;
        });

        // Act
        HttpResponseMessage response = await fixture.Client.DeleteAsync($"/api/installation-sites/{siteId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        bool stillExists = await fixture.ExecuteDbContextAsync(async dbContext =>
            await dbContext.InstallationSites.AnyAsync(site => site.Id == siteId));

        Assert.False(stillExists);
    }
}
