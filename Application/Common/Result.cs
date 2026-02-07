namespace Assistant.Application.Common;

public sealed record Result(bool IsSuccess, string? Message = null)
{
    public static Result Success(string? message = null) => new(true, message);
    public static Result Failure(string message) => new(false, message);
}
