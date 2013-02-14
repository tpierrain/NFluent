namespace NFluent
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when an assertion failed.
    /// </summary>
    public class FluentAssertionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentAssertionException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public FluentAssertionException(string message)
            : base(message)
        {
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="FluentAssertionException" /> class.
        ///// </summary>
        ///// <param name="info">The info.</param>
        ///// <param name="context">The context.</param>
        //protected FluentAssertionException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }
}