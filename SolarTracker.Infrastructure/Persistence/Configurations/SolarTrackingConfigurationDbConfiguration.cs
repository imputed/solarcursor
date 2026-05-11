using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Configurations;

internal sealed class SolarTrackingConfigurationDbConfiguration
    : IEntityTypeConfiguration<SolarTrackingConfigurationDb>
{
    public void Configure(EntityTypeBuilder<SolarTrackingConfigurationDb> entity)
    {
        entity.ToTable("SolarTrackingConfigurations");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedOnAdd();
        entity.HasIndex(e => e.SolarPanelId).IsUnique();
        entity.Property(e => e.PositionThresholdDegrees).IsRequired();
        entity.Property(e => e.StepDurationMs).IsRequired();
        entity.Property(e => e.MaxAdjustmentSteps).IsRequired();

        entity.HasOne(e => e.SolarPanel)
            .WithOne(e => e.SolarTrackingConfiguration)
            .HasForeignKey<SolarTrackingConfigurationDb>(e => e.SolarPanelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
