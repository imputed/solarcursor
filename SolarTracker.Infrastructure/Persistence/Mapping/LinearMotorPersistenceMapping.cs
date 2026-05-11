using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Mapping;

internal static class LinearMotorPersistenceMapping
{
    public static LinearMotor ToDomain(LinearMotorDb db) =>
        new()
        {
            Id = db.Id,
            SolarPanelId = db.SolarPanelId,
            Name = db.Name,
            MoveUpGpioPin = db.MoveUpGpioPin,
            MoveDownGpioPin = db.MoveDownGpioPin,
        };

    public static LinearMotorDb ToDb(LinearMotor domain) =>
        new()
        {
            Id = domain.Id,
            SolarPanelId = domain.SolarPanelId,
            Name = domain.Name,
            MoveUpGpioPin = domain.MoveUpGpioPin,
            MoveDownGpioPin = domain.MoveDownGpioPin,
        };

    public static void CopyScalars(LinearMotorDb target, LinearMotor source)
    {
        target.SolarPanelId = source.SolarPanelId;
        target.Name = source.Name;
        target.MoveUpGpioPin = source.MoveUpGpioPin;
        target.MoveDownGpioPin = source.MoveDownGpioPin;
    }
}
