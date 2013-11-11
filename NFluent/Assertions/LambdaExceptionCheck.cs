using System;
using NFluent.Helpers;

namespace NFluent
{
    /// <summary>
    /// Implements specific exception check after lambda ckecls
    /// </summary>
    public class LambdaExceptionCheck : ILambdaExceptionCheck<ILambdaCheck>,IForkableCheck
    {

        /// <summary>
        /// Set with the parent class that fluently called this one
        /// </summary>
        private ILambdaCheck parent;

        private Exception exception;

        /// <summary>
        /// This check can only be fluently called after a lambda check
        /// </summary>
        public LambdaExceptionCheck(ILambdaCheck parent, Exception exception)
        {
            this.parent = parent;
            this.exception = exception;
        }

        /// <summary>
        /// verify that the checked exception has a specific message
        /// </summary>
        /// <param name="exceptionMessage">the expected message</param>
        /// <returns></returns>
        public ICheckLink<ILambdaExceptionCheck<ILambdaCheck>> WithMessage(string exceptionMessage)
        {
            if (this.exception == null)
            {
                var message =
                    FluentMessage.BuildMessage("You try to check a message [{0}]on an exception that was null wheras it must not").For("code").ToString();
                throw new FluentCheckException(message);
            }

            if (this.exception.Message != exceptionMessage)
            {
                var message =
                    FluentMessage
                        .BuildMessage("The message is not the expected one")
                        .ExpectedValues(this.exception.Message)
                        .And.WithGivenValue(exceptionMessage)
                        .ToString();
                        
                throw new FluentCheckException(message);
            }

            return new CheckLink<ILambdaExceptionCheck<ILambdaCheck>>(this);
        }

        /// <summary>
        /// chaining operator
        /// </summary>
        public ICheckLink<ILambdaExceptionCheck<ILambdaCheck>> And
        {
            get
            {
                return new CheckLink<ILambdaExceptionCheck<ILambdaCheck>>(this);
            }
        }
        
        /// <summary>
        /// verify that the checked exception has a property with a specific value
        /// </summary>
        /// <param name="propertyName">the name of the property to check</param>
        /// <param name="propertyValue">the value of the property to check</param>
        /// <returns></returns>
        public ICheckLink<ILambdaExceptionCheck<ILambdaCheck>> WithProperty(string propertyName, object propertyValue)
        {
            var type = exception.GetType();
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                var message =
                    FluentMessage.BuildMessage(string.Format("The property [{0}] does not exist on exception of type {1}", propertyName, type.Name))
                        .ToString();
                throw new FluentCheckException(message);
            }
            var value = property.GetValue(this.exception, null);
            if (!value.Equals(propertyValue))
            {
                var message = FluentMessage
                    .BuildMessage(string.Format("The property [{0}] do not have the expected value", propertyName))
                    .Expected(propertyValue)
                    .And.WithGivenValue(value).ToString();

                throw new FluentCheckException(message);
            }

            return new CheckLink<ILambdaExceptionCheck<ILambdaCheck>>(this);
        }

        /// <summary>
        /// create a new instance of the same check, injecting necessary properties
        /// </summary>
        /// <returns></returns>
        public object ForkInstance()
        {
            return new LambdaExceptionCheck(this.parent, this.exception);
        }
    }
}