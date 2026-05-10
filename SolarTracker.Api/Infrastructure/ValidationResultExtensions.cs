using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SolarTracker.Api.Infrastructure;

internal static class ValidationResultExtensions
{
    internal static ValidationProblem ToValidationProblem(this ValidationResult result)
    {
        Dictionary<string, string[]> errors = result.Errors
            .GroupBy(static e => string.IsNullOrEmpty(e.PropertyName) ? "_" : e.PropertyName!)
            .ToDictionary(static g => g.Key, g => g.Select(static e => e.ErrorMessage).ToArray());

        return TypedResults.ValidationProblem(errors);
    }
}
