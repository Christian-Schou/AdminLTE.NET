namespace AdminLTE.TWC.Core.Application.Common.Exceptions
{
    /// <summary>
    ///     Represents an exception that is thrown when access to a resource is forbidden.
    /// </summary>
    public class ForbiddenAccessException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ForbiddenAccessException"/> class with a default message.
        /// </summary>
        public ForbiddenAccessException() : base() { }
    }
}
