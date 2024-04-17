namespace AdminLTE.TWC.Core.Application.Common.Models;

/// <summary>
///     Represents the result of an operation.
/// </summary>
public class Result
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="succeeded">Indicates whether the operation succeeded.</param>
    /// <param name="errors">The errors that occurred during the operation.</param>
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }
    
    /// <summary>
    ///     Gets a value indicating whether the operation succeeded.
    /// </summary>
    public bool Succeeded { get; init; }
    
    /// <summary>
    ///     Gets the errors that occurred during the operation.
    /// </summary>
    public string[] Errors { get; init; }

    /// <summary>
    ///     Creates a new <see cref="Result"/> instance representing a successful operation.
    /// </summary>
    /// <returns>A <see cref="Result"/> instance representing a successful operation.</returns>
    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    /// <summary>
    ///     Creates a new <see cref="Result"/> instance representing a failed operation.
    /// </summary>
    /// <param name="errors">The errors that occurred during the operation.</param>
    /// <returns>A <see cref="Result"/> instance representing a failed operation.</returns>
    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }
}