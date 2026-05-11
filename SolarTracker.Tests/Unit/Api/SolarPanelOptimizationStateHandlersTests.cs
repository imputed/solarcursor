using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SolarTracker.Api.Endpoints.SolarPanelOptimizationState;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;

namespace SolarTracker.Tests.Unit.Api;

public sealed class SolarPanelOptimizationStateHandlersTests
{
    [Fact]
    public async Task GetAsync_returns_not_found_when_service_returns_not_found()
    {
        Mock<ISolarPanelOptimizationStateService> service = new();
        service.Setup(x => x.GetAsync(12, CancellationToken.None))
            .Returns(ValueTask.FromResult(Result<SolarPanelOptimizationStateDto>.NotFound(
                "solar-panel-not-found",
                "Solar panel 12 was not found.")));

        var result = await SolarPanelOptimizationStateHandlers.GetAsync(12, service.Object, CancellationToken.None);

        Assert.IsType<NotFound>(result.Result);
    }

    [Fact]
    public async Task PutAsync_returns_ok_when_service_updates_state()
    {
        Mock<ISolarPanelOptimizationStateService> service = new();
        service.Setup(x => x.UpdateAsync(12, It.IsAny<UpdateSolarPanelOptimizationStateDto>(), CancellationToken.None))
            .Returns(ValueTask.FromResult(Result<SolarPanelOptimizationStateDto>.Success(
                new SolarPanelOptimizationStateDto(12, true))));

        var result = await SolarPanelOptimizationStateHandlers.PutAsync(
            12,
            new UpdateSolarPanelOptimizationStateDto(true),
            service.Object,
            CancellationToken.None);

        Ok<SolarPanelOptimizationStateDto> ok = Assert.IsType<Ok<SolarPanelOptimizationStateDto>>(result.Result);
        Assert.True(ok.Value.IsEnabled);
    }
}
