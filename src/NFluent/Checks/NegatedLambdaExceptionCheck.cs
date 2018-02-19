namespace NFluent
{
    using System;
#if !DOTNET_20 && !DOTNET_30
    using System.Linq.Expressions;
#endif
    /// <inheritdoc />
    public class NegatedLambdaExceptionCheck<T> : ILambdaExceptionCheck<T> where T:Exception
    {
        /// <inheritdoc />
        public ICheckLink<ILambdaExceptionCheck<T>> WithMessage(string exceptionMessage)
        {
            throw new InvalidOperationException("You cannot use WithMessage on negated check.");
        }

        /// <inheritdoc />
        public ICheckLink<ILambdaExceptionCheck<T>> WithProperty<TP>(string propertyName, TP propertyValue)
        {
            throw new InvalidOperationException("You cannot use WithProperty on negated check.");
        }

#if !DOTNET_20 && !DOTNET_30
        /// <inheritdoc />
        public ICheckLink<ILambdaExceptionCheck<T>> WithProperty<TP>(Expression<Func<T, TP>> propertyExpression, TP propertyValue)
        {
            throw new InvalidOperationException("You cannot use WithProperty on negated check.");
        }
#endif

        /// <inheritdoc />
        public ILambdaExceptionCheck<TU> DueTo<TU>() where TU : Exception
        {
            throw new InvalidOperationException("You cannot use DueTo on negated check.");
        }
    }
}