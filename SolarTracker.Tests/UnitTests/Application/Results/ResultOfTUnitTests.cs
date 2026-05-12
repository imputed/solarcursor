using SolarTracker.Application.Results;

namespace SolarTracker.Tests.UnitTests.Application.Results;

public sealed class ResultOfTUnitTests
{
    [Fact]
    public void Success_ShouldReturnValueAndSuccessStatus_WhenCalled()
    {
        // Arrange

        // Act
        Result<string> result = Result<string>.Success("ok");

        // Assert
        Assert.Equal(ResultStatus.Success, result.Status);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsNotFound);
        Assert.Equal("ok", result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void NotFound_ShouldReturnErrorWithoutValue_WhenCalled()
    {
        // Arrange
        const string code = "installation-site-not-found";
        const string message = "Installation site 3 was not found.";

        // Act
        Result<string> result = Result<string>.NotFound(code, message);

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.False(result.IsSuccess);
        Assert.True(result.IsNotFound);
        Assert.Null(result.Value);
        Assert.Equal(new ResultError(code, message), result.Error);
    }

    [Fact]
    public void Failure_ShouldReturnErrorWithoutValue_WhenCalled()
    {
        // Arrange
        const string code = "validation-failed";
        const string message = "The input was invalid.";

        // Act
        Result<string> result = Result<string>.Failure(code, message);

        // Assert
        Assert.Equal(ResultStatus.Failure, result.Status);
        Assert.False(result.IsSuccess);
        Assert.False(result.IsNotFound);
        Assert.Null(result.Value);
        Assert.Equal(new ResultError(code, message), result.Error);
    }
}
