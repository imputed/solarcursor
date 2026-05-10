using Microsoft.EntityFrameworkCore;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence;

public sealed class SolarTrackerDbContext(DbContextOptions<SolarTrackerDbContext> options) : DbContext(options)
{
    public DbSet<InstallationSiteDb> InstallationSites => Set<InstallationSiteDb>();

    public DbSet<SolarPanelDb> SolarPanels => Set<SolarPanelDb>();

    public DbSet<LinearMotorDb> LinearMotors => Set<LinearMotorDb>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SolarTrackerDbContext).Assembly);
}
