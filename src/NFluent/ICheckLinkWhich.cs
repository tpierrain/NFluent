// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICheckLinkWhich.cs" company="">
//   
// </copyright>
// <summary>
//   offer chaining for checks as well as zooming on a subcheck
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    ///     Offer chaining for checks as well as zooming on a sub check.
    /// </summary>
    /// <typeparam name="T">Main checker.</typeparam>
    /// <typeparam name="TU">Alternative checker.</typeparam>
    public interface ICheckLinkWhich<out T, out TU> : ICheckLink<T>
        where T : IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        where TU : IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
    {
        /// <summary>
        ///     Checks item identifier in previous check.
        /// </summary>
        /// <value>A checker for the item.</value>
        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1623:PropertySummaryDocumentationMustMatchAccessors",
            Justification = "Reviewed. Suppression is OK here.")]
        TU Which { get; }
    }
}