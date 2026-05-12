using FluentValidation;
using SolarTracker.Application.Dtos.InstallationSite;

namespace SolarTracker.Api.Validation.InstallationSites;

public sealed class UpdateInstallationSiteDtoValidator : AbstractValidator<UpdateInstallationSiteDto>
{
    public UpdateInstallationSiteDtoValidator()
    {
        RuleFor(d => d.Id).GreaterThan(0);
        RuleFor(d => d.Name).NotEmpty().MaximumLength(256);
        RuleFor(d => d.Latitude).InclusiveBetween(-90m, 90m);
        RuleFor(d => d.Longitude).InclusiveBetween(-180m, 180m);
    }
}
