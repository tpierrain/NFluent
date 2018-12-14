// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StringFluentSyntaxExtension.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
namespace NFluent
{
    using System;
    using Extensibility;
    using Extensions;

    /// <summary>
    /// Provides extension method on a ICheckLink for IEnumerable types.
    /// </summary>
    public static class StringFluentSyntaxExtension
    {
        /// <summary>
        /// Checks that the checked <see cref="string"/> contains the expected list of strings only once.
        /// </summary>
        /// <param name="chainedCheckLink">
        /// The chained fluent check.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static IExtendableCheckLink<ICheck<string>, string[]> Once(this IExtendableCheckLink<ICheck<string>, string[]> chainedCheckLink)
        {
            ExtensibilityHelper.BeginCheck(chainedCheckLink.And).
                CantBeNegated("Once").
                Analyze((sut, test) =>
                {
                    foreach (var content in chainedCheckLink.OriginalComparand)
                    {
                        var firstIndex = sut.IndexOf(content, StringComparison.Ordinal);
                        var lastIndexOf = sut.LastIndexOf(content, StringComparison.Ordinal);
                        if (firstIndex != lastIndexOf)
                        {
                            test.Fail(
                                $"The {{0}} contains {content.ToStringProperlyFormatted().DoubleCurlyBraces()} at {firstIndex} and {lastIndexOf}, where as it must contains it once.");
                            return;
                        }
                    }
                }).
                DefineExpectedValues(chainedCheckLink.OriginalComparand, chainedCheckLink.OriginalComparand.Length, "once", "").EndCheck();
            return chainedCheckLink;
        }

        /// <summary>
        /// Checks that the checked <see cref="string"/> contains the expected list of strings in the correct order.
        /// </summary>
        /// <param name="chainedCheckLink">
        /// The chained fluent check.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static IExtendableCheckLink<ICheck<string>, string[]> InThatOrder(this IExtendableCheckLink<ICheck<string>, string[]> chainedCheckLink)
        {
            ExtensibilityHelper.BeginCheck(chainedCheckLink.And).
                CantBeNegated("InThatOrder").
                Analyze((sut, test) =>
                {
                    var lastIndex = 0;
                    foreach (var content in chainedCheckLink.OriginalComparand)
                    {
                        lastIndex = sut.IndexOf(content, lastIndex, StringComparison.Ordinal);
                        if (lastIndex >= 0)
                        {
                            continue;
                        }

                        test.Fail("The {0} does not contain the expected strings in the correct order.");
                        return;
                    }
                }).
                DefineExpectedValues(chainedCheckLink.OriginalComparand, chainedCheckLink.OriginalComparand.Length, "in this order", "")
                .EndCheck();    
            return chainedCheckLink;
        }
    }
}