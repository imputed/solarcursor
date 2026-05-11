using Microsoft.EntityFrameworkCore;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence;

public sealed class SolarTrackerDbContext(DbContextOptions<SolarTrackerDbContext> options) : DbContext(options)
{
    public DbSet<InstallationSiteDb> InstallationSites => Set<InstallationSiteDb>();

    public DbSet<SolarPanelDb> SolarPanels => Set<SolarPanelDb>();

    public DbSet<LinearMotorDb> LinearMotors => Set<LinearMotorDb>();

    public DbSet<TiltMeasuringUnitDb> TiltMeasuringUnits => Set<TiltMeasuringUnitDb>();

    public DbSet<CurrentMeasuringUnitDb> CurrentMeasuringUnits => Set<CurrentMeasuringUnitDb>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SolarTrackerDbContext).Assembly);
}
