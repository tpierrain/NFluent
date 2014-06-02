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
    using NFluent.Extensibility;
    using NFluent.Extensions;

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
        public static IExtendableCheckLink<string, string[]> Once(this IExtendableCheckLink<string, string[]> chainedCheckLink)
        {
            var checker = chainedCheckLink.And as ICheckForExtensibility<string, ICheck<string>>;
            var value = checker.Value;
            var comparand = chainedCheckLink.OriginalComparand;
            foreach (var text in comparand)
            {
                var firstIndex = value.IndexOf(text);
                var lastIndexOf = value.LastIndexOf(text);
                if (firstIndex != lastIndexOf)
                {
                    // failed 
                    var message =
                        FluentMessage.BuildMessage(string.Format("The {{0}} contains {0} at {1} and {2}, where as it must contains it once.", text.ToStringProperlyFormated().DoubleCurlyBraces(), firstIndex, lastIndexOf))
                                     .For("string")
                                     .On(value)
                                     .And.Expected(comparand).Comparison("one");
                    throw new FluentCheckException(message.ToString());
                }
            }

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
        public static IExtendableCheckLink<string, string[]> InThatOrder(this IExtendableCheckLink<string, string[]> chainedCheckLink)
        {
            var checker = chainedCheckLink.And as ICheckForExtensibility<string, ICheck<string>>;
            var value = checker.Value;
            var comparand = chainedCheckLink.OriginalComparand;
            var lastIndex = 0;
            foreach (var text in comparand)
            {
                lastIndex = value.IndexOf(text, lastIndex);
                if (lastIndex < 0)
                {
                    // failed 
                    var message =
                        FluentMessage.BuildMessage(
                            "The {0} does not contain the expected strings in the correct order.")
                                     .For("string")
                                     .On(value)
                                     .And.Expected(comparand)
                                     .Label("Expected content: ");
                    throw new FluentCheckException(message.ToString());
                }
            }

            return chainedCheckLink;
        }
    }
}