using FluentValidation;
using SolarTracker.Application.Dtos.SolarPanel;

namespace SolarTracker.Api.Validation.SolarPanels;

public sealed class CreateSolarPanelDtoValidator : AbstractValidator<CreateSolarPanelDto>
{
    public CreateSolarPanelDtoValidator()
    {
        RuleFor(d => d.InstallationSiteId).GreaterThan(0);
        RuleFor(d => d.SerialNumber).MaximumLength(128);
    }
}
