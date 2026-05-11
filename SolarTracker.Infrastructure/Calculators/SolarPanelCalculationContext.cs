using SolarTracker.Domain.Entities;

namespace SolarTracker.Infrastructure.Calculators;

internal sealed record SolarPanelCalculationContext(
    SolarPanel SolarPanel,
    InstallationSite InstallationSite,
    SolarTrackingConfiguration Configuration);
