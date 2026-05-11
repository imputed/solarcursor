using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Configurations;

internal sealed class SolarOptimizationScheduleConfigurationDbConfiguration
    : IEntityTypeConfiguration<SolarOptimizationScheduleConfigurationDb>
{
    public void Configure(EntityTypeBuilder<SolarOptimizationScheduleConfigurationDb> entity)
    {
        entity.ToTable("SolarOptimizationScheduleConfigurations");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.IntervalMinutes).IsRequired();
    }
}
