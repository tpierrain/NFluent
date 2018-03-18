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
    ///     Provides check methods to be executed on an <see cref="IEnumerable" /> value.
    /// </summary>
    public static class EnumerableCheckExtensions
    {
        /// <summary>
        ///     Checks that the enumerable contains all the given expected values, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the enumerable.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contain all the expected values.</exception>
        public static IExtendableCheckLink<IEnumerable, IEnumerable> Contains<T>(
            this ICheck<IEnumerable> check,
            params T[] expectedValues)
        {
            var properExpectedValues = ExtractEnumerableValueFromPossibleOneValueArray(expectedValues);
            return check.Contains(properExpectedValues);
        }

        /// <summary>
        ///     Checks that the enumerable contains all the values present in another enumerable, in any order.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="otherEnumerable">The enumerable containing the expected values to be found.</param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The enumerable does not contain all the expected values present in the other
        ///     enumerable.
        /// </exception>
        public static IExtendableCheckLink<IEnumerable, IEnumerable> Contains(
            this ICheck<IEnumerable> check,
            IEnumerable otherEnumerable)
        {

            IList<object> notFoundValues = null;
            ExtensibilityHelper.BeginCheck(check).
                FailsIf((sut) => sut == null && otherEnumerable != null, "The {0} is null and thus, does not contain the given expected value(s).").
                ExpectingValues(otherEnumerable, otherEnumerable.Count()).
                Analyze((sut, _) =>  notFoundValues = ExtractNotFoundValues(sut, otherEnumerable)).
                FailsIf((_) => notFoundValues.Any(), string.Format(
                    "The {{0}} does not contain the expected value(s):" + Environment.NewLine + "\t[{0}]", notFoundValues.ToEnumeratedString().DoubleCurlyBraces())).
                Negates("The {0} contains all the given values whereas it must not.").
                EndCheck();

            return ExtensibilityHelper.BuildExtendableCheckLink(check, otherEnumerable);
        }

        /// <summary>
        ///     Checks that the enumerable contains only the given expected values and nothing else, in order.
        ///     This check should only be used with IEnumerable that have a consistent iteration order
        ///     (i.e. don't use it with Hashtable, prefer <see cref="IsOnlyMadeOf{T}" /> in that case).
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
        public static ICheckLink<ICheck<IEnumerable>> ContainsExactly<T>(
            this ICheck<IEnumerable> check,
            params T[] expectedValues)
        {
            var properExpectedValues = ExtractEnumerableValueFromPossibleOneValueArray(expectedValues);

            return check.ContainsExactly(properExpectedValues);
        }

        /// <summary>
        ///     Checks that the enumerable contains only the values of another enumerable and nothing else, in order.
        ///     This check should only be used with IEnumerable that have a consistent iteration order
        ///     (i.e. don't use it with Hashtable, prefer <see cref="IsOnlyMadeOf{T}" /> in that case).
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
            this ICheck<IEnumerable> check,
            IEnumerable otherEnumerable)
        {

            var enumerable =  otherEnumerable== null ? null : otherEnumerable as IList<object> ?? otherEnumerable.Cast<object>().ToList();
            var test = ExtensibilityHelper.BeginCheck(check).ExpectingValues(enumerable, enumerable?.Count ?? 0)
                .FailsIf(sut => sut == null && enumerable != null,
                    "The {0} is null and thus does not contain exactly the {1}.", MessageOption.NoCheckedBlock)
                .FailsIf(sut => sut != null && enumerable == null, "The {0} is not null whereas it should.",
                    MessageOption.NoExpectedBlock)
                .Negates("The {0} contains exactly the given values whereas it must not.", MessageOption.NoExpectedBlock);
            test.Analyze((sut, runner) =>
            {
                if (sut == null || otherEnumerable == null)
                {
                    return;
                }

                var index = 0;
                var first = sut.GetEnumerator();
                var comparer = new EqualityHelper.EqualityComparer<object>();

                var sutCount = sut.Count();
                var expectedCount = enumerable.Count;
                var failed = false;
                using (var second = enumerable.GetEnumerator())
                {
                    while (first.MoveNext())
                    {
                        if (!second.MoveNext() || !comparer.Equals(first.Current, second.Current))
                        {
                            if (sutCount > expectedCount && index == expectedCount)
                            {
                                test.Fails($"The {{0}} does not contain exactly the expected value(s). There are extra elements starting at index #{index}.");
                            }
                            else
                            {
                                test.Fails($"The {{0}} does not contain exactly the expected value(s). First difference is at index #{index}.");
                            }

                            test.SetValuesIndex(index);
                            failed = true;
                            break;
                        }

                        index++;
                    }

                    if (second.MoveNext() && !failed)
                    {
                        test.Fails($"The {{0}} does not contain exactly the expected value(s). Elements are missing starting at index #{index}.");
                        test.SetValuesIndex(index);
                    }
                    
                }
            });
            test.EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Checks that the given enumerable does contain all items matching a predicate.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="check">Check item.</param>
        /// <param name="predicate">Predicate to evaluate.</param>
        /// <returns>A linkable check.</returns>
        public static ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> ContainsOnlyElementsThatMatch<T>(
            this ICheck<IEnumerable<T>> check,
            Func<T, bool> predicate)
        {
            FluentCheck<T> chk = null;
            ExtensibilityHelper.BeginCheck(check).
                FailsIfNull().
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

                            test.Fails($"The {{0}} does contain an element at index #{index} that does not match the given predicate: ({scan.Current}).");
                            chk = new FluentCheck<T>(scan.Current);
                            chk.Checker.SetSutLabel($"element #{index}");
                            return;
                        }

                        chk = new FluentCheck<T>(scan.Current);
                        chk.Checker.SetSutLabel("all elements");
                    }
                }).
                Negates("The {0} contains only element(s) that match the given predicate, whereas it must not.").
                EndCheck();
            return new CheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>>(check, chk);
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
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheckAndProvideSubItem(
                () =>
                {
                    if (checker.Value == null)
                    {
                        throw new FluentCheckException(checker.BuildShortMessage(
                                $"The {{0}} is null, whereas it must have an element with number {index}.")
                            .ToString());
                    }

                    if (!TryGetElementByNumber(checker.Value, index, out var item))
                    {
                        throw new FluentCheckException(checker.BuildMessage(
                            $"The {{0}} does not have an element at index {index}.").ToString());
                    }
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    var itemCheck = Check.That(item);
                    var subChecker = ExtensibilityHelper.ExtractChecker(itemCheck);
                    subChecker.SetSutLabel($"element #{index}");
                    return itemCheck;
                },
                checker.BuildMessage("The {{0}} does have an element at index {0} whereas it should not.").ToString());
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
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheckAndProvideSubItem(
                () =>
                {
                    using (var scan = checker.Value.GetEnumerator())
                    {
                        int? index = null;
                        for (var i = 0; scan.MoveNext(); i++)
                        {
                            if (predicate(scan.Current))
                            {
                                index = i;
                                break;
                            }
                        }

                        if (!index.HasValue)
                        {
                            var message =
                                checker.BuildMessage(
                                    "The {0} does not contain any element that matches the given predicate.");
                            throw new FluentCheckException(message.ToString());
                        }

                        var itemCheck = Check.That(scan.Current);
                        var subChecker = ExtensibilityHelper.ExtractChecker(itemCheck);
                        subChecker.SetSutLabel($"element #{index}");
                        return itemCheck;
                    }
                },
                checker.BuildMessage(
                    "The {0} contains element(s) that matches the given predicate, whereas it must not.").ToString());
        }

        /// <summary>
        ///     Checks that the enumerable has a first element, and returns a check on that element.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check on the first element.</returns>
        public static ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> HasFirstElement<T>(
            this ICheck<IEnumerable<T>> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheckAndProvideSubItem(
                () =>
                {
                    if (checker.Value == null)
                    {
                        var errorMessage =
                            checker.BuildShortMessage("The {0} is null, whereas it must have a first element.");
                        throw new FluentCheckException(errorMessage.ToString());
                    }

                    if (!TryGetElementByNumber(checker.Value, 0, out var first))
                    {
                        var errorMessage =
                            checker.BuildShortMessage("The {0} is empty, whereas it must have a first element.");
                        throw new FluentCheckException(errorMessage.ToString());
                    }

                    var subChecker = Check.That(first);
                    ExtensibilityHelper.ExtractChecker(subChecker).SetSutLabel("First item");
                    return subChecker;
                },
                checker.BuildMessage("The {0} has a first element, whereas it should not.").ToString());
        }

        /// <summary>
        ///     Checks that the enumerable has a last element, and returns a check on that element.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check on the last element.</returns>
        public static ICheckLinkWhich<ICheck<IEnumerable<T>>, ICheck<T>> HasLastElement<T>(
            this ICheck<IEnumerable<T>> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheckAndProvideSubItem(
                () =>
                {
                    if (checker.Value == null)
                    {
                        var nullErrorMessage =
                            checker.BuildShortMessage("The {0} is null, whereas it must have a last element.");
                        throw new FluentCheckException(nullErrorMessage.ToString());
                    }

                    if (!TryGetLastElement(checker.Value, out var last))
                    {
                        var emptyErrorMessage =
                            checker.BuildShortMessage("The {0} is empty, whereas it must have a last element.");
                        throw new FluentCheckException(emptyErrorMessage.ToString());
                    }

                    var subChecker = Check.That(last);
                    ExtensibilityHelper.ExtractChecker(subChecker).SetSutLabel("Last item");
                    return subChecker;
                },
                checker.BuildMessage("The {0} has a last element, whereas it must be empty.").ToString());
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
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheckAndProvideSubItem(
                () =>
                {
                    if (checker.Value == null)
                    {
                        var errorMessage =
                            checker.BuildShortMessage("The {0} is null, whereas it must have one element.");
                        throw new FluentCheckException(errorMessage.ToString());
                    }
                    using (var enumerator = checker.Value.GetEnumerator())
                    {
                        if (!enumerator.MoveNext())
                        {
                            var errorMessage =
                                checker.BuildMessage("The {0} is empty, whereas it must have one element.");
                            throw new FluentCheckException(errorMessage.ToString());
                        }

                        var first = enumerator.Current;

                        if (enumerator.MoveNext())
                        {
                            var errorMessage = checker.BuildMessage(
                                "The {0} contains more than one element, whereas it must have one element only.");
                            throw new FluentCheckException(errorMessage.ToString());
                        }

                        var subChecker = Check.That(first);
                        ExtensibilityHelper.ExtractChecker(subChecker).SetSutLabel("single element");
                        return subChecker;
                    }
                },
                checker.BuildMessage("The {0} has exactly one element, whereas it should not.").ToString());
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
                FailsIfNull().
                Analyze((sut, _) => actualSize = sut.Count()).
                FailsIf((_) => actualSize != expectedSize, $"The {{0}} has {BuildElementNumberLiteral(actualSize).DoubleCurlyBraces()} instead of {expectedSize}.").
                Negates($"The {{0}} has {BuildElementNumberLiteral(expectedSize).DoubleCurlyBraces()} which is unexpected.").
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
        public static ICheckLink<ICheck<IEnumerable>> CountIs(this ICheck<IEnumerable> check, long expectedCount)
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
            ExtensibilityHelper.BeginCheck(check).FailsIfNull()
                .FailsIf((sut) => sut.Cast<object>().Any(), "The {0} is not empty.")
                .Negates("The checked enumerable is empty, which is unexpected.", MessageOption.NoCheckedBlock).EndCheck();
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
            ExtensibilityHelper.BeginCheck(check).FailsIf((sut) => sut != null && sut.Count()>0,
                    "The {0} contains elements, whereas it must be null or empty.")
                .NegatesIf((sut) => sut == null, "The {0} is null, where as it must contain at least one element.", MessageOption.NoCheckedBlock)
                .Negates("The {0} is empty, where as it must contain at least one element.", MessageOption.NoCheckedBlock)
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
        public static ICheckLink<ICheck<IEnumerable>> IsOnlyMadeOf<T>(
            this ICheck<IEnumerable> check,
            params T[] expectedValues)
        {
            var properExpectedValues = ExtractEnumerableValueFromPossibleOneValueArray(expectedValues);

            return check.IsOnlyMadeOf(properExpectedValues);
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
        public static ICheckLink<ICheck<IEnumerable>> IsOnlyMadeOf(
            this ICheck<IEnumerable> check,
            IEnumerable expectedValues)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (Equals(checker.Value, expectedValues))
                    {
                        return;
                    }

                    if (checker.Value == null && expectedValues != null)
                    {
                        var message = checker
                            .BuildMessage("The {0} is null and thus, does not contain exactly the given value(s).")
                            .ExpectedValues(expectedValues).ToString();
                        throw new FluentCheckException(message);
                    }

                    var unexpectedValuesFound = ExtractUnexpectedValues(checker.Value, expectedValues);

                    if (unexpectedValuesFound.Count <= 0)
                    {
                        return;
                    }
                        var message2 = checker
                            .BuildMessage(
                                string.Format(
                                    "The {{0}} does not contain only the given value(s)." + Environment.NewLine
                                    + "It contains also other values:" + Environment.NewLine + "\t[{0}]",
                                    unexpectedValuesFound.ToEnumeratedString().DoubleCurlyBraces()))
                            .ExpectedValues(expectedValues).ToString();
                        throw new FluentCheckException(message2);
                },
                checker.BuildMessage("The {0} contains only the given values whereas it must not.")
                    .ExpectedValues(expectedValues).ToString());
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

        private static IEnumerable ExtractEnumerableValueFromPossibleOneValueArray<T>(T[] expectedValues)
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
            var values = expectedValues.Cast<object>().ToList();

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
    }
}