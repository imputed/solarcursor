using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Configurations;

internal sealed class LinearMotorDbConfiguration : IEntityTypeConfiguration<LinearMotorDb>
{
    public void Configure(EntityTypeBuilder<LinearMotorDb> entity)
    {
        entity.ToTable("LinearMotors");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.InstallationSiteId);
        entity.Property(e => e.Name).HasMaxLength(128);

        entity.HasOne(e => e.InstallationSite)
            .WithMany(e => e.LinearMotors)
            .HasForeignKey(e => e.InstallationSiteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
