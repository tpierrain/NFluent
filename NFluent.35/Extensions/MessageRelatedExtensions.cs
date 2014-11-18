namespace NFluent
{
    using NFluent.Extensibility;

    /// <summary>
    /// 
    /// </summary>
    public static class MessageRelatedExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="check"></param>
        /// <param name="sutLabel"></param>
        public static ICheck<T> As<T>(this ICheck<T> check, string sutLabel)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            checker.SetSutLabel(sutLabel);
            return check;
        }
    }
}
