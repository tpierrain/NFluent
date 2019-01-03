// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableFluentSyntaxExtension.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <summary>
//   Implements fluent chaine syntax for IEnumerables.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NFluent
{
    using System.Collections;
    using System.Collections.Generic;
    using Extensibility;
    using Extensions;

    /// <summary>
    /// Provides extension method on a ICheckLink for IEnumerable types.
    /// </summary>
    public static class EnumerableFluentSyntaxExtension
    {
        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains only the authorized items. Can only be used after a call to Contains.
        /// </summary>
        /// <param name="chainedCheckLink">
        /// The chained fluent check.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static IExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> Only(this IExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> chainedCheckLink)
        {
            chainedCheckLink.AccessCheck.IsOnlyMadeOf(chainedCheckLink.OriginalComparand);
            return chainedCheckLink;
        }

        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains the expected list of items only once.
        /// </summary>
        /// <param name="chainedCheckLink">
        /// The chained fluent check.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static IExtendableCheckLink<ICheck<T>, IEnumerable> Once<T>(this IExtendableCheckLink<ICheck<T>, IEnumerable> chainedCheckLink) where T: IEnumerable
        {
            ExtensibilityHelper.BeginCheck(chainedCheckLink.AccessCheck).
                CantBeNegated("Once").
                ComparingTo(chainedCheckLink.OriginalComparand, "once of", "").
                Analyze((sut, test) =>
                {
                    var itemIndex = 0;
                    var expectedList = ToNewList(chainedCheckLink);
                    var listedItems = new List<object>();
                    foreach (var item in sut)
                    {
                        if (expectedList.Contains(item))
                        {
                            listedItems.Add(item);
                            expectedList.Remove(item);
                        }
                        else if (listedItems.Contains(item))
                        {
                            test.Fail(
                                $"The {{0}} has extra occurrences of the expected items. Item [{item.ToStringProperlyFormatted().DoubleCurlyBraces()}] at position {itemIndex} is redundant.");
                            return;
                        }

                        itemIndex++;
                    }
                }).
                EndCheck();

            return chainedCheckLink;
        }

        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains items in the expected order.
        /// </summary>
        /// <param name="chainedCheckLink">
        /// The chained fluent check.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static IExtendableCheckLink<ICheck<T>, IEnumerable> InThatOrder<T>(this IExtendableCheckLink<ICheck<T>, IEnumerable> chainedCheckLink) where T: IEnumerable
        {
            ExtensibilityHelper.BeginCheck(chainedCheckLink.AccessCheck).CantBeNegated("InThatOrder").
                ComparingTo(chainedCheckLink.OriginalComparand, "in that order", "").
                Analyze((sut, test) => 
                {
                    var orderedList = ToNewList(chainedCheckLink);

                    var failingIndex = 0;
                    var scanIndex = 0;
                    foreach (var item in sut)
                    {
                        if (item != orderedList[scanIndex])
                        {
                            // if current item is part of current list, check order
                            var index = orderedList.IndexOf(item, scanIndex);
                            if (index < 0)
                            {
                                // if not found at the end of the list, try the full list
                                index = orderedList.IndexOf(item);
                                if (index >= 0)
                                {
                                    test.Fail(
                                        $"The {{0}} does not follow to the expected order. Item [{item.ToStringProperlyFormatted().DoubleCurlyBraces()}] appears too late in the list, at index '{failingIndex}'.");
                                    break;
                                }
                            }
                            else
                            {
                                var currentReference = orderedList[scanIndex];
                                
                                // skip all similar entries in the expected list (tolerance: the checked enumerable may contain as many instances of one item as expected)
                                while (currentReference == orderedList[++scanIndex])
                                {}

                                // check if skipped only similar items
                                if (scanIndex < index)
                                {
                                    test.Fail(
                                        $"The {{0}} does not follow to the expected order. Item [{item.ToStringProperlyFormatted().DoubleCurlyBraces()}] appears too early in the list, at index '{failingIndex}'.");
                                    break;

                                }
                            }

                            if (index >= 0)
                            {
                                scanIndex = index;
                            }
                        }

                        failingIndex++;
                    }
                }).EndCheck();
            return chainedCheckLink;
        }

        private static List<object> ToNewList<T>(IExtendableCheckLink<ICheck<T>, IEnumerable> chainedCheckLink)
        {
            var orderedList = new List<object>();
            foreach (var item in chainedCheckLink.OriginalComparand)
            {
                orderedList.Add(item);
            }

            return orderedList;
        }
    }
}