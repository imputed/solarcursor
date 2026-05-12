using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SolarTracker.Api.Endpoints.InstallationSites;
using SolarTracker.Application.Dtos.InstallationSite;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Tests.UnitTests.Api.Endpoints.InstallationSites;

public sealed class InstallationSiteHandlersUnitTests
{
    [Fact]
    public async Task MoveToOptimumAsync_ShouldReturnProblem_WhenServiceReturnsFailure()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        Mock<IInstallationSiteService> service = new();
        Mock<ILoggerFactory> loggerFactory = new();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns((ILogger)NullLogger.Instance);
        service.Setup(x => x.Optimize(7, cancellationToken))
            .ReturnsAsync(
                Result.Failure(
                    "solar-panel-threshold-not-met",
                    "Solar panel 12 did not reach the configured threshold in the allowed number of steps."));

        // Act
        var result = await InstallationSiteHandlers.MoveToOptimumAsync(
            7,
            service.Object,
            loggerFactory.Object,
            cancellationToken);

        // Assert
        ProblemHttpResult problem = Assert.IsType<ProblemHttpResult>(result.Result);
        Assert.Equal(StatusCodes.Status409Conflict, problem.StatusCode);
        Assert.Equal("Installation site movement failed", problem.ProblemDetails.Title);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnProblem_WhenEntityCannotBeLoadedAfterCreate()
    {
        // Arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        CreateInstallationSiteDto dto = new("North site", 50.1m, 8.6m);
        Mock<IValidator<CreateInstallationSiteDto>> validator = new();
        Mock<IInstallationSiteService> service = new();
        Mock<IInstallationSiteQueryHandler> queryHandler = new();
        Mock<ILoggerFactory> loggerFactory = new();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns((ILogger)NullLogger.Instance);
        validator.Setup(x => x.ValidateAsync(dto, cancellationToken))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        service.Setup(x => x.AddAsync(dto, cancellationToken))
            .Returns(ValueTask.FromResult(7));
        queryHandler.Setup(x => x.GetByIdAsync(7, cancellationToken))
            .Returns(ValueTask.FromResult<InstallationSite?>(null));

        // Act
        var result = await InstallationSiteHandlers.CreateAsync(
            dto,
            validator.Object,
            service.Object,
            queryHandler.Object,
            loggerFactory.Object,
            cancellationToken);

        // Assert
        ProblemHttpResult problem = Assert.IsType<ProblemHttpResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, problem.StatusCode);
        Assert.Equal("Installation site persistence failed", problem.ProblemDetails.Title);
    }
}
