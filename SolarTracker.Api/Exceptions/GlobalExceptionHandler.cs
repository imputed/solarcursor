using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SolarTracker.Api.Errors;
using SolarTracker.Api.Logging;

namespace SolarTracker.Api.Exceptions;

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ApiLog.UnhandledException(logger, exception);

        ProblemDetails problem = new()
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = ApiProblemCatalog.ServerErrorTitleText(),
            Type = ApiProblemCatalog.ServerErrorTypeUri(),
            Detail = environment.IsDevelopment() ? exception.ToString() : null,
        };

        httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
