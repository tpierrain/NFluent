// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableCheckExtensions.cs" company="">
//   Copyright 2013 Thomas PIERRAIN, Cyrille Dupuydauby
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Extensibility;
    using Extensions;
    using Helpers;
    using Kernel;

#if !DOTNET_30 && !DOTNET_20
    using System.Linq;
#endif

    /// <summary>
    /// Provides check methods to be executed on an <see cref="IEnumerable" /> value.
    /// </summary>
    public static class EnumerableCheckExtensions
    {
        /// <summary>
        /// Checks that the enumerable contains all the given expected values, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the enumerable.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        ///  A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contain all the expected values.</exception>
        public static ExtendableCheckLink<ICheck<IEnumerable<T>>, IEnumerable<T>> Contains<T>(
            this ICheck<IEnumerable<T>> check,
            params T[] expectedValues) 
        {
            var properExpectedValues = ExtractEnumerableValueFromPossibleOneValueArray(expectedValues);
            ImplementContains(ExtensibilityHelper.BeginCheck(check), properExpectedValues);
            return ExtensibilityHelper.BuildExtendableCheckLink(check, properExpectedValues);
        }

        /// <summary>
        /// Checks that the enumerable contains all the given expected values, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the enumerable.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        ///  A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contain all the expected values.</exception>
        public static ExtendableCheckLink<ICheck<IEnumerable<T>>, IEnumerable<T>> Contains<T>(
            this ICheck<IEnumerable<T>> check,
            IEnumerable<T> expectedValues) 
        {
            ImplementContains(ExtensibilityHelper.BeginCheck(check), expectedValues);
            return ExtensibilityHelper.BuildExtendableCheckLink(check, expectedValues);
        }

        /// <summary>
        /// Checks that the enumerable contains all the values present in another enumerable, in any order.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="otherEnumerable">The enumerable containing the expected values to be found.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The enumerable does not contain all the expected values present in the other enumerable.
        /// </exception>
        public static ExtendableCheckLink<ICheck<IEnumerable>, IEnumerable> Contains(
            this ICheck<IEnumerable> check, params object[] otherEnumerable)
        {
            var properExpectedValues = ExtractEnumerableValueFromSingleEntry(otherEnumerable).Cast<object>();
            var checker = ExtensibilityHelper.BeginCheckAs(check, enumerable => enumerable.Cast<object>());
            ImplementContains(checker, properExpectedValues);

            return ExtensibilityHelper.BuildExtendableCheckLink(check,(IEnumerable) properExpectedValues);
        }

        private static void ImplementContains<T>(ICheckLogic<IEnumerable<T>> checker, IEnumerable<T> otherEnumerable)
        {
            IList<object> notFoundValues = null;
            checker.FailWhen((sut) => sut == null && otherEnumerable != null,
                    "The {0} is null and thus, does not contain the given expected value(s).")
                .DefineExpectedValues(otherEnumerable, otherEnumerable.Count())
                .Analyze((sut, _) => notFoundValues = ExtractNotFoundValues(sut, otherEnumerable)).FailWhen(
                    (_) => notFoundValues.Any(), string.Format(
                        "The {{0}} does not contain the expected value(s):" + Environment.NewLine + "\t{0}",
                        notFoundValues.ToStringProperlyFormatted().DoubleCurlyBraces()))
                .OnNegate("The {0} contains all the given values whereas it must not.").EndCheck();
        }
        
        /// <summary>
        ///     Checks that the enumerable contains only the given expected values and nothing else, in order.
        ///     This check should only be used with IEnumerable that have a consistent iteration order
        ///     (i.e. don't use it with Hashtable, prefer <see cref="IsOnlyMadeOf" /> in that case).
        /// </summary>
        /// <typeparam name="T">Type of the elements to be found.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The enumerable does not contains only the exact given values and nothing else,
        ///     in order.
        /// </exception>
        public static ICheckLink<ICheck<IEnumerable<T>>> ContainsExactly<T>(this ICheck<IEnumerable<T>> check, IEnumerable<T> expectedValues)
        {
            ImplementContainsExactly(ExtensibilityHelper.BeginCheck(check), expectedValues);
            return ExtensibilityHelper.BuildCheckLink(check);
        }
     
        /// <summary>
        ///     Checks that the enumerable contains only the given expected values and nothing else, in order.
        ///     This check should only be used with IEnumerable that have a consistent iteration order
        ///     (i.e. don't use it with Hashtable, prefer <see cref="IsOnlyMadeOf" /> in that case).
        /// </summary>
        /// <typeparam name="T">Type of the elements to be found.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The enumerable does not contains only the exact given values and nothing else,
        ///     in order.
        /// </exception>
        public static ICheckLink<ICheck<IEnumerable<T>>> ContainsExactly<T>(this ICheck<IEnumerable<T>> check, params T[] expectedValues) 
        {
            if (typeof(T) == typeof(object))
            {
                var objectList = ExtractEnumerableValueFromSingleEntry(expectedValues)?.Cast<object>();
                ImplementContainsExactly(ExtensibilityHelper.BeginCheckAs(check, u => u.Cast<object>()), objectList);
            }
            else
            {
                var properExpectedValues = ExtractEnumerableValueFromPossibleOneValueArray(expectedValues);
                ImplementContainsExactly(ExtensibilityHelper.BeginCheck(check), properExpectedValues);
            }
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Checks that the enumerable contains only the values of another enumerable and nothing else, in order.
        ///     This check should only be used with IEnumerable that have a consistent iteration order
        ///     (i.e. don't use it with Hashtable, prefer <see cref="IsOnlyMadeOf" /> in that case).
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="otherEnumerable">The other enumerable containing the exact expected values to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The enumerable does not contains only the exact given values and nothing else,
        ///     in order.
        /// </exception>
        public static ICheckLink<ICheck<IEnumerable>> ContainsExactly(
            this ICheck<IEnumerable> check, params object[] otherEnumerable)
        {
            var properExpectedValues = ExtractEnumerableValueFromSingleEntry(otherEnumerable).Cast<object>();

            ImplementContainsExactly(ExtensibilityHelper.BeginCheckAs(check, u => u.Cast<object>()), properExpectedValues);
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        private static void ImplementContainsExactly<T, TU>(ICheckLogic<T> test, IEnumerable<TU> enumerable) where T:IEnumerable<TU>
        {
            var count = enumerable?.Count() ?? 0;
            test.DefineExpectedValues(enumerable, count).FailWhen(sut => sut == null && enumerable != null,
                "The {0} is null and thus does not contain exactly the {1}.", MessageOption.NoCheckedBlock).FailWhen(
                sut => sut != null && enumerable == null, "The {0} is not null whereas it should.",
                MessageOption.NoExpectedBlock).OnNegate("The {0} contains exactly the given values whereas it must not.",
                MessageOption.NoExpectedBlock);
            test.Analyze((sut, runner) =>
            {
                if (sut == null)
                {
                    return;
                }

                var index = 0;
                var first = sut.GetEnumerator();
                var comparer = new EqualityHelper.EqualityComparer<object>();

                // ReSharper disable once PossibleNullReferenceException
                var expectedCount = count;
                var failed = false;
                using (var second = enumerable.GetEnumerator())
                {
                    while (first.MoveNext())
                    {
                        if (!second.MoveNext() || !comparer.Equals(first.Current, second.Current))
                        {
                            test.Fail(
                                index == expectedCount
                                    ? $"The {{0}} does not contain exactly the expected value(s). There are extra elements starting at index #{index}."
                                    : $"The {{0}} does not contain exactly the expected value(s). First difference is at index #{index}.");

                            test.SetValuesIndex(index);
                            failed = true;
                            break;
                        }

                        index++;
                    }

                    if (second.MoveNext() && !failed)
                    {
                        test.Fail(
                            $"The {{0}} does not contain exactly the expected value(s). Elements are missing starting at index #{index}.");
                        test.SetValuesIndex(index);
                    }
                }
            });
            test.EndCheck();
        }

        /// <summary>
        /// Checks if the sut contains the same element than a given list.
        /// </summary>
        /// <param name="context">Context for the check</param>
        /// <param name="content"></param>
        /// <returns>A chainable link.</returns>
        public static ICheckLink<ICheck<IEnumerable>> IsEquivalentTo(this ICheck<IEnumerable> context,
            params object[] content)
        {
            ImplementEquivalentTo(ExtensibilityHelper.BeginCheckAs(context, enumerable => enumerable.Cast<object>()), content);
            return ExtensibilityHelper.BuildCheckLink(context);
        }

        /// <summary>
        /// Checks if the sut contains the same element than a given list.
        /// </summary>
        /// <param name="context">Context for the check</param>
        /// <param name="content"></param>
        /// <typeparam name="T">Type of enumerable content</typeparam>
        /// <returns>A chainable link.</returns>
        public static ICheckLink<ICheck<IEnumerable<T>>> IsEquivalentTo<T>(this ICheck<IEnumerable<T>> context,
            params T[] content)
        {
            var checker = ExtensibilityHelper.BeginCheck(context);
                ImplementEquivalentTo(checker, content);
            return ExtensibilityHelper.BuildCheckLink(context);
        }

        /// <summary>
        /// Checks if the sut contains the same element than a given list.
        /// </summary>
        /// <param name="context">Context for the check</param>
        /// <param name="content">Expected content</param>
        /// <typeparam name="T">Type of enumerable content</typeparam>
        /// <returns>A chainable link.</returns>
        // GH #249
        public static ICheckLink<ICheck<IEnumerable<T>>> IsEquivalentTo<T>(this ICheck<IEnumerable<T>> context,
            IEnumerable<T> content)
        {
            return IsEquivalentTo(context, content?.ToArray());
        }

        private static void ImplementEquivalentTo<T>(ICheckLogic<IEnumerable<T>> checker, ICollection<T> content)
        {
            var length = content?.Count ?? 0;
            checker.Analyze((sut, test) =>
                {
                    if (sut == null)
                    {
                        if (content != null)
                        {
                            test.Fail("The {checked} is null whereas it should not.");
                        }

                        return;
                    }

                    if (content == null)
                    {
                        test.Fail("The {checked} must be null.");
                        return;
                    }

                    var expectedContent = new List<T>(content);
                    var isOk = true;
                    foreach (var item in sut)
                    {
                        if (!expectedContent.Remove(item))
                        {
                            test.Fail(
                                $"The {{checked}} does contain [{item.ToStringProperlyFormatted().DoubleCurlyBraces()}] whereas it should not.");
                            isOk = false;
                        }
                    }

                    if (isOk && expectedContent.Count > 0)
                    {
                        if (expectedContent.Count == 1)
                        {
                            test.Fail(
                                $"The {{checked}} is missing: [{expectedContent[0].ToStringProperlyFormatted().DoubleCurlyBraces()}].");
                        }
                        else
                        {
                            test.Fail(
                                $"The {{checked}} is missing {expectedContent.Count} items: {expectedContent.ToStringProperlyFormatted().DoubleCurlyBraces()}.");
                        }
                    }
                }).DefineExpectedValues(content, length)
                .OnNegate("The {checked} is equivalent to the {expected} whereas it should not.").EndCheck();
        }

        /// <summary>
        /// Checks if the sut is a sub set of another collection
        /// </summary>
        /// <param name="context">Check context</param>
        /// <param name="expectedSuperset">Expected superset</param>
        /// <typeparam name="T">Type of items in the collection</typeparam>
        /// <returns>A chainable link.</returns>
        public static ICheckLink<ICheck<IEnumerable<T>>> IsSubSetOf<T>(this ICheck<IEnumerable<T>> context,
            IEnumerable<T> expectedSuperset)
        {
            var superSet = new List<T>(expectedSuperset);
            ExtensibilityHelper.BeginCheck(context).
                FailIfNull().
                DefineExpectedValues(expectedSuperset, superSet.Count).
                Analyze((sut, test) =>
                {
                    foreach (var item in sut)
                    {
                        if (!superSet.Remove(item))
                        {
                            test.Fail(
                                $"The {{checked}} contains {item.ToStringProperlyFormatted().DoubleCurlyBraces()} which is absent from {{expected}}.");
                            break;
                        }
                    }
                }).
                OnNegate("The {checked} is a subset of {given} whereas it should not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }

        /// <summary>
        /// Checks that the given enumerable does contain all items matching a predicate.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="check">Check item.</param>
        /// <param name="predicate">Predicate to evaluate.</param>
        /// <returns>A linkable check.</returns>
        public static ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> ContainsOnlyElementsThatMatch<T>(
            this ICheck<IEnumerable<T>> check,
            Func<T, bool> predicate)
        {
            var item = default(T);
            var label = string.Empty;
            ExtensibilityHelper.BeginCheck(check).
                FailIfNull().
                Analyze((sut, test) =>
                {
                    var index = 0;
                    using (var scan = sut.GetEnumerator())
                    {
                        while (scan.MoveNext())
                        {
                            if (predicate(scan.Current))
                            {
                                index++;
                                continue;
                            }
                            test.Fail($"The {{0}} does contain an element at index #{index} that does not match the given predicate: ({scan.Current.ToStringProperlyFormatted().DoubleCurlyBraces()}).");
                            item = scan.Current;
                            label = $"element #{index}";
                            return;
                        }

                        label = "default element";
                    }
                }).
                OnNegate("The {0} contains only element(s) that match the given predicate, whereas it must not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLinkWhich(check, item, label);
        }

        /// <summary>
        ///     Check a specific item in the <see cref="IEnumerable" />.
        /// </summary>
        /// <typeparam name="T">
        ///     Enumerated type.
        /// </typeparam>
        /// <param name="check">
        ///     The checker.
        /// </param>
        /// <param name="index">
        ///     Index to check.
        /// </param>
        /// <returns>
        ///     An extensible checker.
        /// </returns>
        public static ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> HasElementAt<T>(
            this ICheck<IEnumerable<T>> check, int index)
        {
            var item = default(T);
            ExtensibilityHelper.BeginCheck(check)
                .FailIfNull($"The {{0}} is null, whereas it must have an element with number {index}.")
                .FailWhen(sut => !TryGetElementByNumber(sut, index, out item),
                    $"The {{0}} does not have an element at index {index}.")
                .OnNegate($"The {{0}} does have an element at index {index} whereas it should not.")
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLinkWhich(check, item, $"element #{index}");
        }

        /// <summary>
        ///     Checks that the given enumerable does contain at least one item matching a predicate.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="check">Check item.</param>
        /// <param name="predicate">Predicate to evaluate.</param>
        /// <returns>A linkable check.</returns>
        public static ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> HasElementThatMatches<T>(
            this ICheck<IEnumerable<T>> check,
            Func<T, bool> predicate)
        {
            var item = default(T);
            var label = string.Empty;
            ExtensibilityHelper.BeginCheck(check).
                FailIfNull().
                Analyze((sut, test) =>
                {
                    long? index = null;
                    using (var scan = sut.GetEnumerator())
                    {
                        for (var i = 0; scan.MoveNext(); i++)
                        {
                            if (!predicate(scan.Current))
                            {
                                continue;
                            }

                            index = i;
                            item = scan.Current;
                            break;

                        }
                        if (!index.HasValue)
                        {
                            test.Fail("The {0} does not contain any element that matches the given predicate.");
                            return;
                        }
                        label = $"element #{index}";
                    }
                }).
                OnNegate("The {0} contains element(s) that matches the given predicate, whereas it must not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLinkWhich(check, item, label);
        }

        /// <summary>
        /// Checks that the enumerable has a first element, and returns a check on that element.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check on the first element.</returns>
        public static ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> HasFirstElement<T>(
            this ICheck<IEnumerable<T>> check)
        {
            var item = default(T);
            ExtensibilityHelper.BeginCheck(check)
                .FailIfNull("The {0} is null, whereas it must have a first element.")
                .FailWhen(sut => !TryGetElementByNumber(sut, 0, out item),
                    "The {0} is empty, whereas it must have a first element.", MessageOption.NoCheckedBlock)
                .OnNegate("The {0} has a first element, whereas it must be empty.")
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLinkWhich(check, item, "First element");
        }

        /// <summary>
        ///  Checks that the enumerable has a last element, and returns a check on that element.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check on the last element.</returns>
        public static ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> HasLastElement<T>(
            this ICheck<IEnumerable<T>> check)
        {
            var item = default(T);
            ExtensibilityHelper.BeginCheck(check)
                .FailIfNull("The {0} is null, whereas it must have a last element.")
                .FailWhen(sut => !TryGetLastElement(sut, out item),
                    "The {0} is empty, whereas it must have a last element.", MessageOption.NoCheckedBlock)
                .OnNegate("The {0} has a last element, whereas it must be empty.")
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLinkWhich(check, item, "Last element");
        }

        /// <summary>
        ///     Checks that the enumerable has a single element, and returns a check on that element.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check on the single element.</returns>
        public static ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> HasOneElementOnly<T>(
            this ICheck<IEnumerable<T>> check)
        {
            var item = default(T);
            ExtensibilityHelper.BeginCheck(check)
                .FailIfNull("The {0} is null, whereas it must have one element.")
                .Analyze((sut, test) =>
                    {
                        using (var enumerator = sut.GetEnumerator())
                        {
                            if (!enumerator.MoveNext())
                            {
                                test.Fail("The {0} is empty, whereas it must have one element.");
                                return;
                            }

                            item = enumerator.Current;

                            if (enumerator.MoveNext())
                            {
                                test.Fail("The {0} contains more than one element, whereas it must have one element only.");
                            }
                        }
                    })
                .OnNegate("The {0} has exactly one element, whereas it should not.")
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLinkWhich(check, item, "single element");
        }

        /// <summary>
        ///     Checks that the enumerable has the proper number of elements.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedSize">The expected size to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable has not the expected number of elements.</exception>
        public static ICheckLink<ICheck<IEnumerable<T>>> HasSize<T>(this ICheck<IEnumerable<T>> check, long expectedSize)
        {
            long actualSize=0;
            ExtensibilityHelper.BeginCheck(check).
                FailIfNull().
                Analyze((sut, _) => actualSize = sut.Count()).
                FailWhen(_ => actualSize != expectedSize, $"The {{0}} has {BuildElementNumberLiteral(actualSize).DoubleCurlyBraces()} instead of {expectedSize}.").
                OnNegate($"The {{0}} has {BuildElementNumberLiteral(expectedSize).DoubleCurlyBraces()} which is unexpected.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Checks that the enumerable has the proper number of elements.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedSize">The expected size to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable has not the expected number of elements.</exception>
        public static ICheckLink<ICheck<IEnumerable>> HasSize(this ICheck<IEnumerable> check, long expectedSize)
        {
            long actualSize=0;
            ExtensibilityHelper.BeginCheck(check).
                FailIfNull().
                Analyze((sut, _) => actualSize = sut.Count()).
                FailWhen(_ => actualSize != expectedSize, $"The {{0}} has {BuildElementNumberLiteral(actualSize).DoubleCurlyBraces()} instead of {expectedSize}.").
                OnNegate($"The {{0}} has {BuildElementNumberLiteral(expectedSize).DoubleCurlyBraces()} which is unexpected.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Checks that the enumerable has the proper number of elements.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedCount">The expected count to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable has not the expected number of elements.</exception>
        
        public static ICheckLink<ICheck<IEnumerable<T>>> CountIs<T>(this ICheck<IEnumerable<T>> check, long expectedCount)
        {
            return HasSize(check, expectedCount);
        }
        

        /// <summary>
        ///     Checks that the enumerable is empty.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable is not empty.</exception>
        public static ICheckLink<ICheck<T>> IsEmpty<T>(this ICheck<T> check) where T: IEnumerable
        {
            ExtensibilityHelper.BeginCheck(check).FailIfNull()
                .FailWhen((sut) => sut.Cast<object>().Any(), "The {0} is not empty.")
                .OnNegate("The checked enumerable is empty, which is unexpected.", MessageOption.NoCheckedBlock).EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Checks that the enumerable is null or empty.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable is not empty.</exception>
        public static ICheckLink<ICheck<T>> IsNullOrEmpty<T>(this ICheck<T> check) where T : IEnumerable
        {
            ExtensibilityHelper.BeginCheck(check).FailWhen((sut) => sut != null && sut.Count()>0,
                    "The {0} contains elements, whereas it must be null or empty.")
                .OnNegateWhen((sut) => sut == null, "The {0} is null, where as it must contain at least one element.", MessageOption.NoCheckedBlock)
                .OnNegate("The {0} is empty, where as it must contain at least one element.", MessageOption.NoCheckedBlock)
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Checks that the enumerable contains only the given values and nothing else, in any order.
        ///     Note: this check succeeded with empty value.
        /// </summary>
        /// <typeparam name="T">Type of the expected values to be found.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contain only the expected values provided.</exception>
        public static ICheckLink<ICheck<IEnumerable<T>>> IsOnlyMadeOf<T>(
            this ICheck<IEnumerable<T>> check,
            params T[] expectedValues)
        {
            var properExpectedValues = ExtractEnumerableValueFromPossibleOneValueArray(expectedValues);
            return check.IsOnlyMadeOf(properExpectedValues);
        }

        /// <summary>
        ///     Checks that the enumerable contains only the given values and nothing else, in any order.
        ///     Note: this check succeeded with empty value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contain only the expected values provided.</exception>
        public static ICheckLink<ICheck<IEnumerable>> IsOnlyMadeOf(
            this ICheck<IEnumerable> check,
            params object[] expectedValues)
        {
            var properExpectedValues = ExtractEnumerableValueFromSingleEntry(expectedValues);
            ImplementIsOnlyMadeOf(ExtensibilityHelper.BeginCheckAs(check, enumerable => enumerable.Cast<object>()), properExpectedValues);
           
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Checks that the enumerable contains only the values present in another enumerable, and nothing else, in any order.
        ///     Note: this check succeeded with empty value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The enumerable does not contain only the expected values present in the other
        ///     enumerable.
        /// </exception>
        public static ICheckLink<ICheck<IEnumerable<T>>> IsOnlyMadeOf<T>(
            this ICheck<IEnumerable<T>> check,
            IEnumerable<T> expectedValues)
        {
            ImplementIsOnlyMadeOf(ExtensibilityHelper.BeginCheck(check), expectedValues);
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        private static void ImplementIsOnlyMadeOf<T>(ICheckLogic<IEnumerable<T>> checker, IEnumerable expectedValues)
        {
            checker.DefineExpectedValues(expectedValues, expectedValues.Count(), comparison: "only elements from",
                negatedComparison: "at least one element different from").FailWhen(sut => sut == null & expectedValues != null,
                "The {0} is null and thus, does not contain exactly the given value(s).").Analyze((sut, test) =>
            {
                if (sut == null && expectedValues == null)
                {
                    return;
                }

                var unexpectedValuesFound = ExtractUnexpectedValues(sut, expectedValues);

                if (unexpectedValuesFound.Count <= 0)
                {
                    return;
                }

                test.Fail(
                    string.Format(
                        "The {{0}} does not contain only the given value(s)."
                        + Environment.NewLine
                        + "It contains also other values:"
                        + Environment.NewLine + "\t{0}",
                        unexpectedValuesFound.ToStringProperlyFormatted().DoubleCurlyBraces()));
            }).OnNegate("The {0} contains only the given values whereas it must not.").EndCheck();
        }

        /// <summary>
        /// Checks that all items in an enumeration are unique.
        /// </summary>
        /// <param name="context">Context for the check</param>
        /// <typeparam name="T">Type of enumeration</typeparam>
        /// <returns>A context link.</returns>
        public static ICheckLink<ICheck<T>> ContainsNoDuplicateItem<T>(this ICheck<T> context) where T : IEnumerable
        {
            ExtensibilityHelper.BeginCheck(context).
                Analyze((sut, test) =>
                {
                    var store  = new List<object>();
                    foreach (var entry in sut)
                    {
                        if (store.Contains(entry, new EqualityHelper.EqualityComparer<object>()))
                        {
                            test.Fail($"The {{checked}} contains a duplicate item at position {store.Count}: [{entry}].");
                            return;
                        }

                        store.Add(entry);
                    }
                }).
                OnNegate("The {checked} should contain duplicates.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }
        
        /// <summary>
        /// Checks that all items in an enumeration are non null.
        /// </summary>
        /// <param name="context">Context for the check</param>
        /// <typeparam name="T">Type of enumeration</typeparam>
        /// <returns>A context link.</returns>
        public static ICheckLink<ICheck<T>> ContainsNoNull<T>(this ICheck<T> context) where T : IEnumerable
        {
            ExtensibilityHelper.BeginCheck(context).
                Analyze((sut, test) =>
                {
                    var index = 0;
                    foreach (var entry in sut)
                    {
                        if (entry == null)
                        {
                            test.Fail($"The {{checked}} contains a null item at position {index}.");
                            return;
                        }

                        index++;
                    }
                }).
                OnNegate("The {checked} should contain at least one null entry.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }

        /// <summary>
        /// Checks that all items in an enumeration are non null.
        /// </summary>
        /// <param name="context">Context for the check</param>
        /// <param name="type">Expected type</param>
        /// <typeparam name="T">Type of enumeration</typeparam>
        /// <returns>A context link.</returns>
        public static ICheckLink<ICheck<T>> ContainsOnlyInstanceOfType<T>(this ICheck<T> context, Type type) where T : IEnumerable
        {
            ExtensibilityHelper.BeginCheck(context).
                Analyze((sut, test) =>
                {
                    var index = 0;
                    foreach (var entry in sut)
                    {
                        if (!type.IsInstanceOfType(entry))
                        {
                            test.Fail($"The {{checked}} contains an entry of a type different from {type.Name} at position {index}.");
                            return;
                        }

                        index++;
                    }
                }).
                OnNegate($"The {{checked}} should contain at least one entry of a type different from {type.Name}.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }

        private static string BuildElementNumberLiteral(long itemsCount)
        {
            var foundElementsNumberDescription = itemsCount.ToString();
            if (itemsCount > 1)
            {
                foundElementsNumberDescription += " elements";
            }
            else
            {
                foundElementsNumberDescription += " element";
            }

            return foundElementsNumberDescription;
        }

        private static IEnumerable<T> ExtractEnumerableValueFromPossibleOneValueArray<T>(T[] expectedValues)
        {
            IEnumerable<T> properExpectedValues;
            if (IsAOneValueArrayWithOneCollectionInside(expectedValues))
            {
                properExpectedValues = expectedValues[0] as IEnumerable<T>;
            }
            else
            {
                properExpectedValues = expectedValues;
            }

            return properExpectedValues;
        }

        private static IEnumerable ExtractEnumerableValueFromSingleEntry<T>(T[] expectedValues)
        {
            IEnumerable properExpectedValues;
            if (IsAOneValueArrayWithOneCollectionInside(expectedValues))
            {
                properExpectedValues = expectedValues[0] as IEnumerable;
            }
            else
            {
                properExpectedValues = expectedValues;
            }

            return properExpectedValues;
        }

        /// <summary>
        ///     Returns all expected values that aren't present in the enumerable.
        /// </summary>
        /// <param name="enumerable">The enumerable to inspect.</param>
        /// <param name="expectedValues">The expected values to search within the enumerable.</param>
        /// <returns>
        ///     A list containing all the expected values that aren't present in the enumerable.
        /// </returns>
        private static IList<object> ExtractNotFoundValues(IEnumerable enumerable, IEnumerable expectedValues)
        {
            if (expectedValues == null)
            {
                return new List<object>();
            }

            // Prepares the list to return
            var values = expectedValues as IList<object> ?? expectedValues.Cast<object>().ToList();
            var notFoundValues = values.ToList();

            var comparer = new EqualityHelper.EqualityComparer<object>();

            foreach (var element in enumerable)
            {
                foreach (var expectedValue in values)
                {
                    if (!comparer.Equals(element, expectedValue))
                    {
                        continue;
                    }

                    notFoundValues.RemoveAll(one => comparer.Equals(one, expectedValue));
                    break;
                }
            }

            return notFoundValues;
        }

        /// <summary>
        ///     Returns all the values of the enumerable that don't belong to the expected ones.
        /// </summary>
        /// <param name="enumerable">The enumerable to inspect.</param>
        /// <param name="expectedValues">The allowed values to be part of the enumerable.</param>
        /// <returns>
        ///     A list with all the values found in the enumerable that don't belong to the expected ones.
        /// </returns>
        private static IList<object> ExtractUnexpectedValues(IEnumerable enumerable, IEnumerable expectedValues)
        {
            var equalityComparer = new EqualityHelper.EqualityComparer<object>();
            var values = (expectedValues ?? new List<object>()).Cast<object>().ToList();

            return enumerable.Cast<object>().Where(element => !values.Contains(element, equalityComparer)).ToList();
        }

        private static bool IsAnEnumerableButNotAnEnumerableOfChars<T>(T element)
        {
            return element is IEnumerable && !(element is IEnumerable<char>);
        }

        private static bool IsAOneValueArrayWithOneCollectionInside<T>(T[] expectedValues)
        {
            // For every collections like ArrayList, List<T>, IEnumerable<T>, StringCollection, etc.
            return expectedValues != null && expectedValues.LongLength() == 1 &&
                   IsAnEnumerableButNotAnEnumerableOfChars(expectedValues[0]);
        }

        private static bool TryGetElementByNumber<T>(IEnumerable<T> collection, int number, out T element)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number),
                    "The specified number is less than zero, whereas it must be a 0-based index.");
            }

            if (collection is IList<T> list)
            {
                if (list.Count > number)
                {
                    element = list[number];
                    return true;
                }
            }
            else
            {
                var currentNumber = 0;
                foreach (var currentElement in collection)
                {
                    if (currentNumber == number)
                    {
                        element = currentElement;
                        return true;
                    }

                    currentNumber++;
                }
            }

            element = default(T);
            return false;
        }

        private static bool TryGetLastElement<T>(IEnumerable<T> collection, out T last)
        {
            if (collection is IList<T> list)
            {
                if (list.Count != 0)
                {
                    last = list[list.Count - 1];
                    return true;
                }
            }
            else
            {
                using (var enumerator = collection.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        last = enumerator.Current;
                        while (enumerator.MoveNext())
                        {
                            last = enumerator.Current;
                        }

                        return true;
                    }
                }
            }

            last = default(T);
            return false;
        }

        private class BasicComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var comparable = x as IComparable;
                return comparable?.CompareTo(y) ?? (y == null ? 0 : -1);
            }
        }
        /// <summary>
        /// Checks if the IEnumerable is naturally sorted
        /// </summary>
        /// <param name="context">context for the check</param>
        /// <param name="comparer">optional comparer</param>
        /// <typeparam name="T">enumerable type</typeparam>
        /// <returns>A check link</returns>
        public static ICheckLink<ICheck<T>> IsInAscendingOrder<T>(this ICheck<T> context, IComparer comparer = null) where T : IEnumerable
        {
            if (comparer == null)
            {
                comparer = new BasicComparer();
            }

            ExtensibilityHelper.BeginCheck(context).
                FailIfNull().
                Analyze((sut, test) =>
                {
                    object previous = null;
                    var index = 0;
                    var first = true;
                    foreach (var current in sut)
                    {
                        if (!first)
                        {
                            if (comparer.Compare(previous, current) > 0)
                            {
                                test.Fail($"The {{checked}} is not in ascending order, whereas it should.{Environment.NewLine}At #{index}: [{previous.ToStringProperlyFormatted().DoubleCurlyBraces()}] comes after [{current.ToStringProperlyFormatted().DoubleCurlyBraces()}].");
                                return;
                            }
                        }
                        else
                        {
                            first = false;
                        }

                        previous = current;
                        index++;
                    }
                }).
                OnNegate("The {checked} is in ascending order whereas it should not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }

        /// <summary>
        /// Checks if the IEnumerable is naturally sorted
        /// </summary>
        /// <param name="context">context for the check</param>
        /// <param name="comparer">optional comparer</param>
        /// <typeparam name="T">enumerable type</typeparam>
        /// <returns>A check link</returns>
        public static ICheckLink<ICheck<T>> IsInDescendingOrder<T>(this ICheck<T> context, IComparer comparer = null) where T : IEnumerable
        {
            if (comparer == null)
            {
                comparer = new BasicComparer();
            }

            ExtensibilityHelper.BeginCheck(context).
                FailIfNull().
                Analyze((sut, test) =>
                {
                    IComparable previous = null;
                    var index = 0;
                    var first = true;
                    foreach (IComparable current in sut)
                    {
                        if (!first)
                        {
                            if (comparer.Compare(previous, current) < 0)
                            {
                                test.Fail($"The {{checked}} is not in descending order, whereas it should.{Environment.NewLine}At #{index}: [{previous.ToStringProperlyFormatted().DoubleCurlyBraces()}] comes before [{current.ToStringProperlyFormatted().DoubleCurlyBraces()}].");
                                return;
                            }
                        }
                        else
                        {
                            first = false;
                        }

                        previous = current;
                        index++;
                    }
                }).
                OnNegate("The {checked} is in ascending order whereas it should not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }
    }
}