using System.Diagnostics.CodeAnalysis;

namespace SolarTracker.Application.Results;

public readonly record struct Result<T> where T : notnull
{
    public ResultStatus Status { get; init; }

    public T? Value { get; init; }

    public ResultError? Error { get; init; }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess => Status == ResultStatus.Success;

    public bool IsNotFound => Status == ResultStatus.NotFound;

    public static Result<T> Success(T value) =>
        new()
        {
            Status = ResultStatus.Success,
            Value = value,
        };

    public static Result<T> NotFound(string code, string message) =>
        new()
        {
            Status = ResultStatus.NotFound,
            Error = new ResultError(code, message),
        };

    public static Result<T> NotFound(ResultError error) =>
        new()
        {
            Status = ResultStatus.NotFound,
            Error = error,
        };

    public static Result<T> Failure(string code, string message) =>
        new()
        {
            Status = ResultStatus.Failure,
            Error = new ResultError(code, message),
        };

    public static Result<T> Failure(ResultError error) =>
        new()
        {
            Status = ResultStatus.Failure,
            Error = error,
        };
}
