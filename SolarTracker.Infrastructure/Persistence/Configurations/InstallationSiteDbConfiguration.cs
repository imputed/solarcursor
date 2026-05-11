using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Configurations;

internal sealed class InstallationSiteDbConfiguration : IEntityTypeConfiguration<InstallationSiteDb>
{
    public void Configure(EntityTypeBuilder<InstallationSiteDb> entity)
    {
        entity.ToTable("InstallationSites");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(256).IsRequired();
        entity.Property(e => e.Latitude).HasPrecision(9, 6);
        entity.Property(e => e.Longitude).HasPrecision(9, 6);

        entity.HasOne(e => e.TiltMeasuringUnit)
            .WithOne(e => e.InstallationSite)
            .HasForeignKey<TiltMeasuringUnitDb>(e => e.InstallationSiteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
