namespace Shared.Core.Models;

public class Result<T> : Result
{
    public T Value { get; }

    protected Result(bool succeeded, T value, IEnumerable<string> errors) 
        : base(succeeded, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value) =>
        new(true, value, []);

    public new static Result<T> Failure(IEnumerable<string> errors) =>
        new(false, default, errors);
    
    public new static Result<T> Failure(string error) =>
        new(false, default, [error]);
}