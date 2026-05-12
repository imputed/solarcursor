using FluentValidation;
using SolarTracker.Api.Validation.Analyze;
using SolarTracker.Application.Analysis.CurrentMeasuringUnit;
using SolarTracker.Application.Analysis.InstallationSite;
using SolarTracker.Application.Analysis.LinearMotor;
using SolarTracker.Application.Analysis.SolarPanel;
using SolarTracker.Application.Analysis.TiltMeasuringUnit;

namespace SolarTracker.Tests.UnitTests.Api.Validation.Analyze;

public sealed class AnalyzeRequestValidatorsUnitTests
{
    [Fact]
    public async Task InstallationSiteAnalyzeRequestValidator_ShouldRejectTakeAbove100()
    {
        await AssertTakeAbove100IsInvalidAsync(
            new InstallationSiteAnalyzeRequestValidator(),
            new InstallationSiteAnalyzeRequest(null, null, Take: 101));
    }

    [Fact]
    public async Task SolarPanelAnalyzeRequestValidator_ShouldRejectTakeAbove100()
    {
        await AssertTakeAbove100IsInvalidAsync(
            new SolarPanelAnalyzeRequestValidator(),
            new SolarPanelAnalyzeRequest(null, null, Take: 101));
    }

    [Fact]
    public async Task CurrentMeasuringUnitAnalyzeRequestValidator_ShouldRejectTakeAbove100()
    {
        await AssertTakeAbove100IsInvalidAsync(
            new CurrentMeasuringUnitAnalyzeRequestValidator(),
            new CurrentMeasuringUnitAnalyzeRequest(null, null, Take: 101));
    }

    [Fact]
    public async Task TiltMeasuringUnitAnalyzeRequestValidator_ShouldRejectTakeAbove100()
    {
        await AssertTakeAbove100IsInvalidAsync(
            new TiltMeasuringUnitAnalyzeRequestValidator(),
            new TiltMeasuringUnitAnalyzeRequest(null, null, Take: 101));
    }

    [Fact]
    public async Task LinearMotorAnalyzeRequestValidator_ShouldRejectTakeAbove100()
    {
        await AssertTakeAbove100IsInvalidAsync(
            new LinearMotorAnalyzeRequestValidator(),
            new LinearMotorAnalyzeRequest(null, null, Take: 101));
    }

    private static async Task AssertTakeAbove100IsInvalidAsync<T>(
        IValidator<T> validator,
        T request) where T : notnull
    {
        var result = await validator.ValidateAsync(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Take");
    }
}
