using FluentValidation;
using SolarTracker.Application.Dtos.InstallationSite;

namespace SolarTracker.Api.Validation.InstallationSites;

public sealed class CreateInstallationSiteDtoValidator : AbstractValidator<CreateInstallationSiteDto>
{
    public CreateInstallationSiteDtoValidator()
    {
        RuleFor(d => d.Name).NotEmpty().MaximumLength(256);
        RuleFor(d => d.Latitude).InclusiveBetween(-90m, 90m);
        RuleFor(d => d.Longitude).InclusiveBetween(-180m, 180m);
    }
}
