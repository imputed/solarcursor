using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Configurations;

internal sealed class SolarPanelDbConfiguration : IEntityTypeConfiguration<SolarPanelDb>
{
    public void Configure(EntityTypeBuilder<SolarPanelDb> entity)
    {
        entity.ToTable("SolarPanels");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.InstallationSiteId);
        entity.Property(e => e.SerialNumber).HasMaxLength(128);

        entity.HasOne(e => e.InstallationSite)
            .WithMany(e => e.SolarPanels)
            .HasForeignKey(e => e.InstallationSiteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
