using FluentValidation;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Validation.InstallationSites;

public sealed class UpdateInstallationSiteDtoValidator : AbstractValidator<UpdateInstallationSiteDto>
{
    public UpdateInstallationSiteDtoValidator()
    {
        RuleFor(d => d.Id).GreaterThan(0);
        RuleFor(d => d.Name).NotEmpty().MaximumLength(256);
    }
}
