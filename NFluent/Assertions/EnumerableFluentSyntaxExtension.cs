// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableFluentSyntaxExtension.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
//   // //   Licensed under the Apache License, Version 2.0 (the "License");
//   // //   you may not use this file except in compliance with the License.
//   // //   You may obtain a copy of the License at
//   // //       http://www.apache.org/licenses/LICENSE-2.0
//   // //   Unless required by applicable law or agreed to in writing, software
//   // //   distributed under the License is distributed on an "AS IS" BASIS,
//   // //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   // //   See the License for the specific language governing permissions and
//   // //   limitations under the License.
// </copyright>
// <summary>
//   Implements fluent chaine syntax for IEnumerables.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent
{
    using System.Collections;
    using System.Text;

    /// <summary>
    /// Provides extension method on a IChainableFluentAssertion for IEnumerable types.
    /// </summary>
    public static class EnumerableFluentSyntaxExtension
    {
        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains only the authorized items. Can only be used after a call to Contains.
        /// </summary>
        /// <param name="chainedFluentAssertion">
        /// The chained Fluent Assertion.
        /// </param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        public static IExtendableFluentAssertion<IEnumerable> Only(this IExtendableFluentAssertion<IEnumerable> chainedFluentAssertion)
        {
            chainedFluentAssertion.And.ContainsOnly(chainedFluentAssertion.OriginalComparand);
            return chainedFluentAssertion;
        }

        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains items in the expected order.
        /// </summary>
        /// <param name="chainedFluentAssertion">
        /// The chained fluent assertion.
        /// </param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        public static IExtendableFluentAssertion<IEnumerable> InThatOrder(
            this IExtendableFluentAssertion<IEnumerable> chainedFluentAssertion)
        {
            var orderedList = new ArrayList();
            foreach (var item in chainedFluentAssertion.OriginalComparand)
            {
                orderedList.Add(item);
            }
            
            var faillingIndex = 0;
            var scanIndex = 0;
            foreach (var item in chainedFluentAssertion.And.Value)
            {
                if (item != orderedList[scanIndex])
                {
                    var failed = false;

                    // if current item is part of current list, check order
                    var index = orderedList.IndexOf(item, scanIndex);
                    if (index < 0)
                    {
                        // if not found at the end of the list, try the full list
                        index = orderedList.IndexOf(item);
                        if (index >= 0)
                        {
                            failed = true;
                        }
                    }
                    else
                    {
                        var currentReference = orderedList[scanIndex];

                        while (currentReference == orderedList[++scanIndex] 
                            && scanIndex < orderedList.Count)
                        {
                        }

                        if (scanIndex < index)
                        {
                            failed = true;
                        }
                    }

                    if (failed)
                    {
                        // check failed. Now we have to refine the issue type.
                        // we assume that Contains was executed (imposed by chaining syntax)
                        // the item violating the order is the previous one!
                        var message = new StringBuilder(200);
                        message.AppendFormat(
                            "The checked enumerable does not follow to the expected order. Item '{0}' appears too {2} in the list, at index '{1}'.",
                            item,
                            faillingIndex,
                            index > scanIndex ? "early" : "late");
                        message.AppendFormat(
                            "\nChecked enumerable:\n\t[{0}]\nExpected enumerable order:\n\t[{1}]",
                            chainedFluentAssertion.And.Value.ToEnumeratedString(),
                            chainedFluentAssertion.OriginalComparand.ToEnumeratedString());
                        throw new FluentAssertionException(message.ToString());
                    }

                    if (index >= 0)
                    {
                        scanIndex = index;
                    }
                }

                faillingIndex++;
            }

            return chainedFluentAssertion;
        }
    }
}