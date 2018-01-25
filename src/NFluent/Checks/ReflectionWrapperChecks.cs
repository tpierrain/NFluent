namespace NFluent
{
    using Extensibility;
    using Extensions;
    using Helpers;

    /// <summary>
    /// Hosts reflection-based checks <see cref="ObjectFieldsCheckExtensions.Considering{T}"/>
    /// </summary>
    public static class ReflectionWrapperChecks
    {
        /// <summary>
        /// </summary>
        /// <param name="check"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static ICheckLink<ICheck<ReflectionWrapper>> IsEqualTo<TU>(this ICheck<ReflectionWrapper> check,
            TU expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var expectedWrapper = ReflectionWrapper.BuildFromInstance(typeof(TU), expected, checker.Value.Criteria);

            var message = ObjectFieldsCheckExtensions.CompareMembers(checker, false, false, expectedWrapper, checker.Value);
            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        /// Checks if the extracted members match one of the provided expected values.
        /// </summary>
        /// <param name="check">Checker logiv.</param>
        /// <param name="values">List of possible values</param>
        /// <returns>A link object</returns>
        public static ICheckLink<ICheck<ReflectionWrapper>> IsOneOf(this ICheck<ReflectionWrapper> check,
            params object[] values)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            checker.ExecuteCheck(() =>
                {
                    foreach (var expected in values)
                    {
                        var expectedWrapper = ReflectionWrapper.BuildFromInstance(expected.GetTypeWithoutThrowingException(), expected, checker.Value.Criteria);

                        var message = ObjectFieldsCheckExtensions.CompareMembers(checker, false, false, expectedWrapper, checker.Value);
                        if (message == null)
                        {
                            return;
                        }
                    }

                    var libel = checker.BuildMessage("The {0} is equal to none of {1} whereas it should.").Expected(values);
                    throw new FluentCheckException(libel.ToString());
                }
            , checker.BuildMessage("The {0} is equal to one of {1} whereas it should not.").ToString());
            return checker.BuildChainingObject();
        }

    }
}
