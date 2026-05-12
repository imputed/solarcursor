using SolarTracker.Application.Results;

namespace SolarTracker.Tests.UnitTests.Application.Results;

public sealed class ResultUnitTests
{
    [Fact]
    public void Success_ShouldReturnSuccessStatusWithoutError_WhenCalled()
    {
        // Arrange

        // Act
        Result result = Result.Success();

        // Assert
        Assert.Equal(ResultStatus.Success, result.Status);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsNotFound);
        Assert.Null(result.Error);
    }

    [Fact]
    public void NotFound_ShouldReturnNotFoundStatusAndError_WhenCalled()
    {
        // Arrange
        const string code = "solar-panel-not-found";
        const string message = "Solar panel 7 was not found.";

        // Act
        Result result = Result.NotFound(code, message);

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.False(result.IsSuccess);
        Assert.True(result.IsNotFound);
        Assert.Equal(new ResultError(code, message), result.Error);
    }

    [Fact]
    public void NotFound_ShouldUseProvidedResultError_WhenCalledWithResultError()
    {
        // Arrange
        ResultError error = new("solar-panel-not-found", "Solar panel 7 was not found.");

        // Act
        Result result = Result.NotFound(error);

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Failure_ShouldReturnFailureStatusAndError_WhenCalled()
    {
        // Arrange
        const string code = "movement-failed";
        const string message = "The motor movement could not be completed.";

        // Act
        Result result = Result.Failure(code, message);

        // Assert
        Assert.Equal(ResultStatus.Failure, result.Status);
        Assert.False(result.IsSuccess);
        Assert.False(result.IsNotFound);
        Assert.Equal(new ResultError(code, message), result.Error);
    }

    [Fact]
    public void Failure_ShouldUseProvidedResultError_WhenCalledWithResultError()
    {
        // Arrange
        ResultError error = new("movement-failed", "The motor movement could not be completed.");

        // Act
        Result result = Result.Failure(error);

        // Assert
        Assert.Equal(ResultStatus.Failure, result.Status);
        Assert.Equal(error, result.Error);
    }
}
