using FluentValidation;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Validation.SolarPanels;

public sealed class UpdateSolarPanelDtoValidator : AbstractValidator<UpdateSolarPanelDto>
{
    public UpdateSolarPanelDtoValidator()
    {
        RuleFor(d => d.Id).GreaterThan(0);
        RuleFor(d => d.InstallationSiteId).GreaterThan(0);
        RuleFor(d => d.SerialNumber).MaximumLength(128);
    }
}
