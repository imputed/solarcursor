namespace SolarTracker.Application.Results;

public readonly record struct Result
{
    public ResultStatus Status { get; init; }

    public ResultError? Error { get; init; }

    public bool IsSuccess => Status == ResultStatus.Success;

    public bool IsNotFound => Status == ResultStatus.NotFound;

    public static Result Success() => new() { Status = ResultStatus.Success };

    public static Result NotFound(string code, string message) =>
        new()
        {
            Status = ResultStatus.NotFound,
            Error = new ResultError(code, message),
        };

    public static Result NotFound(ResultError error) =>
        new()
        {
            Status = ResultStatus.NotFound,
            Error = error,
        };

    public static Result Failure(string code, string message) =>
        new()
        {
            Status = ResultStatus.Failure,
            Error = new ResultError(code, message),
        };

    public static Result Failure(ResultError error) =>
        new()
        {
            Status = ResultStatus.Failure,
            Error = error,
        };
}
