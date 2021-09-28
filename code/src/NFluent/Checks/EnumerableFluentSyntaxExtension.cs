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
//   Implements fluent chained syntax for enumerable types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NFluent
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Extensibility;
    using Extensions;
    using Kernel;

    /// <summary>
    /// Provides extension method on a ICheckLink for IEnumerable types.
    /// </summary>
    public static class EnumerableFluentSyntaxExtension
    {
        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains only the authorized items. Can only be used after a call to Contains.
        /// </summary>
        /// <param name="check">
        /// The chained fluent check.
        /// </param>
        /// <typeparam name="T">type of enumerated items</typeparam>
        /// <returns>
        /// A check link.
        /// </returns>
        public static ExtendableCheckLink<ICheck<IEnumerable<T>>, IEnumerable<T>> Only<T>(this ExtendableCheckLink<ICheck<IEnumerable<T>>, IEnumerable<T>> check)
        {
            IExtendableCheckLink<ICheck<IEnumerable<T>>, IEnumerable<T>> chainedCheckLink = check;
            chainedCheckLink.AccessCheck.IsOnlyMadeOf(chainedCheckLink.OriginalComparand);
            return check;
        }

        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains only the authorized items. Can only be used after a call to Contains.
        /// </summary>
        /// <param name="check">
        /// The chained fluent check.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static ExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> Only(this ExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> check)
        {
            IExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> chainedCheckLink = check;
            chainedCheckLink.AccessCheck.IsOnlyMadeOf(chainedCheckLink.OriginalComparand);
            return check;
        }

        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains the expected list of items only once.
        /// </summary>
        /// <param name="check">
        /// The chained fluent check.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static ExtendableCheckLink<ICheck<IEnumerable<T>>, IEnumerable<T>> Once<T>(this ExtendableCheckLink<ICheck<IEnumerable<T>>, IEnumerable<T>> check)
        {
            IExtendableCheckLink<ICheck<IEnumerable<T>>, IEnumerable<T>> chainedCheckLink = check;
            ImplementOnce(ExtensibilityHelper.BeginCheck(chainedCheckLink.AccessCheck), chainedCheckLink.OriginalComparand);
            return check;
        }

        private static void ImplementOnce<T>(ICheckLogic<IEnumerable<T>> check, IEnumerable<T> originalComparand)
        {
            check.
                CantBeNegated("Once").
                ComparingTo(originalComparand, "once of", "").
                Analyze((sut, test) =>
            {
                var itemIndex = 0;
                var expectedList = new List<T>(originalComparand);
                var listedItems = new List<T>();
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
                        test.SetValuesIndex(itemIndex, -1);
                        return;
                    }

                    itemIndex++;
                }
            }).
                EndCheck();
        }

        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains the expected list of items only once.
        /// </summary>
        /// <param name="check">
        /// The chained fluent check.
        /// </param>
        /// <returns> bv             
        /// A check link.
        /// </returns>
        public static ExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> Once(this ExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> check)
        {
            IExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> chainedCheckLink = check;
            ImplementOnce(
                ExtensibilityHelper.BeginCheckAs(chainedCheckLink.AccessCheck, enumerable => enumerable.Cast<object>()),
                chainedCheckLink.OriginalComparand.Cast<object>());
            return check;
        }

        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains items in the expected order.
        /// </summary>
        /// <param name="check">
        /// The chained fluent check.
        /// </param>
        /// <typeparam name="T">Enumerated item type.</typeparam>
        /// <returns>
        /// A check link.
        /// </returns>
        public static IExtendableCheckLink<ICheck<IEnumerable<T>>, IEnumerable<T>> InThatOrder<T>(this IExtendableCheckLink<ICheck<IEnumerable<T>>, IEnumerable<T>> check)
        {
            var orderedList = new List<T>(check.OriginalComparand);
            var checker = ExtensibilityHelper.BeginCheck(check.AccessCheck);
            ImplementInThatOrder(checker, orderedList);
            return check;
        }

        /// <summary>
        /// Checks that the checked <see cref="IEnumerable"/> contains items in the expected order.
        /// </summary>
        /// <param name="check">
        /// The chained fluent check.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        public static IExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> InThatOrder(this IExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> check)
        {
            var orderedList = new List<object>(check.OriginalComparand.Cast<object>());
            var checker = ExtensibilityHelper.BeginCheckAs(check.AccessCheck, enumerable => enumerable.Cast<object>());
            ImplementInThatOrder(checker, orderedList);
            return check;
        }

        private static void ImplementInThatOrder<T>(ICheckLogic<IEnumerable<T>> checker, List<T> orderedList)
        {
            checker.CantBeNegated("InThatOrder").
                DefineExpectedValues(orderedList, orderedList.Count,"in that order", "").
                Analyze((sut, test) =>
            {
                var failingIndex = 0;
                var scanIndex = 0;
                foreach (var item in sut)
                {
                    // if current item is part of current list, check order
                    var index = orderedList.IndexOf(item, scanIndex);
                    if (index >= 0)
                    {
                        var currentReference = orderedList[scanIndex];

                        // skip all similar entries in the expected list (tolerance: the checked enumerable may contain as many instances of one item as expected)
                        while (currentReference.Equals(orderedList[++scanIndex]))
                        {}

                        // check if skipped only similar items
                        if (scanIndex < index)
                        {
                            test.Fail(
                                $"The {{0}} does not follow to the expected order. Item [{item.ToStringProperlyFormatted().DoubleCurlyBraces()}] appears too early in the list, at index '{failingIndex}'.");
                            test.SetValuesIndex(failingIndex, index);
                            break;
                        }
                    }
                    else
                    {
                        // if not found at the end of the list, try the full list
                        index = orderedList.IndexOf(item);
                        if (index >= 0)
                        {
                            test.Fail(
                                $"The {{0}} does not follow to the expected order. Item [{item.ToStringProperlyFormatted().DoubleCurlyBraces()}] appears too late in the list, at index '{failingIndex}'.");
                            test.SetValuesIndex(failingIndex, index);
                            break;
                        }
                    }

                    if (index >= 0)
                    {
                        scanIndex = index;
                    }

                    failingIndex++;
                }
            }).EndCheck();
        }
    }
}