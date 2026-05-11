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
        entity.HasIndex(e => e.SolarPanelId);
        entity.Property(e => e.Name).HasMaxLength(128);
        entity.Property(e => e.MoveUpGpioPin).IsRequired();
        entity.Property(e => e.MoveDownGpioPin).IsRequired();

        entity.HasOne(e => e.SolarPanel)
            .WithMany(e => e.LinearMotors)
            .HasForeignKey(e => e.SolarPanelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
