using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Configurations;

internal sealed class SolarPanelOptimizationStateDbConfiguration
    : IEntityTypeConfiguration<SolarPanelOptimizationStateDb>
{
    public void Configure(EntityTypeBuilder<SolarPanelOptimizationStateDb> entity)
    {
        entity.ToTable("SolarPanelOptimizationStates");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedOnAdd();
        entity.HasIndex(e => e.SolarPanelId).IsUnique();
        entity.Property(e => e.IsEnabled).IsRequired();

        entity.HasOne(e => e.SolarPanel)
            .WithOne()
            .HasForeignKey<SolarPanelOptimizationStateDb>(e => e.SolarPanelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
