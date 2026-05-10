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
    }
}
