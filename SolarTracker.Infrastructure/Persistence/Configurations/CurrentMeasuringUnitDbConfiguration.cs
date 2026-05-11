using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Configurations;

internal sealed class CurrentMeasuringUnitDbConfiguration : IEntityTypeConfiguration<CurrentMeasuringUnitDb>
{
    public void Configure(EntityTypeBuilder<CurrentMeasuringUnitDb> entity)
    {
        entity.ToTable("CurrentMeasuringUnits");
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.SolarPanelId).IsUnique();
        entity.Property(e => e.Name).HasMaxLength(128);
        entity.Property(e => e.GpioPin).IsRequired();
    }
}
