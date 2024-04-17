using FluentValidation.Results;

namespace AdminLTE.TWC.Core.Application.Common.Exceptions
{
    /// <summary>
    ///     Represents an exception that is thrown when validation failures occur.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationException"/> class with a default message.
        /// </summary>
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationException"/> class with a collection of validation failures.
        /// </summary>
        /// <param name="failures">The collection of validation failures.</param>
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        /// <summary>
        ///     Gets the dictionary containing validation errors.
        /// </summary>
        public IDictionary<string, string[]> Errors { get; }
    }
}
