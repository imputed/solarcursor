using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SolarTracker.Api.Endpoints.SolarTrackingConfiguration;
using SolarTracker.Api.Validation.SolarTrackingConfiguration;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;

namespace SolarTracker.Tests.Unit.Api;

public sealed class SolarTrackingConfigurationHandlersTests
{
    [Fact]
    public async Task GetAsync_returns_ok_when_service_returns_configuration()
    {
        Mock<ISolarTrackingConfigurationService> service = new();
        service.Setup(x => x.GetAsync(4, CancellationToken.None))
            .Returns(ValueTask.FromResult(Result<SolarTrackingConfigurationDto>.Success(
                new SolarTrackingConfigurationDto(4, 1.5d, 600, 20))));

        var result = await SolarTrackingConfigurationHandlers.GetAsync(4, service.Object, CancellationToken.None);

        Ok<SolarTrackingConfigurationDto> ok = Assert.IsType<Ok<SolarTrackingConfigurationDto>>(result.Result);
        SolarTrackingConfigurationDto dto = Assert.IsType<SolarTrackingConfigurationDto>(ok.Value);
        Assert.Equal(4, dto.SolarPanelId);
    }

    [Fact]
    public async Task PutAsync_returns_validation_problem_when_request_is_invalid()
    {
        Mock<ISolarTrackingConfigurationService> service = new();
        IValidator<UpdateSolarTrackingConfigurationDto> validator =
            new UpdateSolarTrackingConfigurationDtoValidator();

        var result = await SolarTrackingConfigurationHandlers.PutAsync(
            4,
            new UpdateSolarTrackingConfigurationDto(0d, 40, 0),
            validator,
            service.Object,
            CancellationToken.None);

        Assert.IsType<ValidationProblem>(result.Result);
        service.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateSolarTrackingConfigurationDto>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
