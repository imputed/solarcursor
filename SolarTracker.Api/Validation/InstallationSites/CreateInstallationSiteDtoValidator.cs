using FluentValidation;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Validation.InstallationSites;

public sealed class CreateInstallationSiteDtoValidator : AbstractValidator<CreateInstallationSiteDto>
{
    public CreateInstallationSiteDtoValidator()
    {
        RuleFor(d => d.Name).NotEmpty().MaximumLength(256);
    }
}
