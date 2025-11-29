namespace Shared.Core.Models;

public class Result
{
    public bool Succeeded { get; }
    public string[] Errors { get; }

    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = [.. errors];
    }

    public static Result Success() =>
        new(true, []);

    public static Result Failure(IEnumerable<string> errors) =>
        new(false, errors);
    
    public static Result Failure(string error) =>
        new(false, [error]);
}