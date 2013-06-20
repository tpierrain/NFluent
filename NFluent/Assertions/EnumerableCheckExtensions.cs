// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EnumerableCheckExtensions.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on an <see cref="IEnumerable"/> value.
    /// </summary>
    public static class EnumerableCheckExtensions
    {
        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public static ICheckLink<ICheck<IEnumerable>> IsEqualTo(this ICheck<IEnumerable> check, object expected)
        {
            var checkRunner = check as ICheckRunner<IEnumerable>;
            var runnableCheck = check as IRunnableCheck<IEnumerable>;

            var expectedEnumerable = expected as IEnumerable;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        EqualityHelper.IsEqualTo(runnableCheck.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expectedEnumerable, true));
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is equal to the expected value.</exception>
        public static ICheckLink<ICheck<IEnumerable>> IsNotEqualTo(this ICheck<IEnumerable> check, object expected)
        {
            var checkRunner = check as ICheckRunner<IEnumerable>;
            var runnableCheck = check as IRunnableCheck<IEnumerable>;

            var expectedEnumerable = expected as IEnumerable;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        EqualityHelper.IsNotEqualTo(runnableCheck.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expectedEnumerable, false));
        }

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A chainable fluent check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual instance is not of the provided type.</exception>
        public static ICheckLink<ICheck<IEnumerable>> IsInstanceOf<T>(this ICheck<IEnumerable> check)
        {
            var checkRunner = check as ICheckRunner<IEnumerable>;
            var runnableCheck = check as IRunnableCheck<IEnumerable>;

            return checkRunner.ExecuteCheck(
                () =>
                {
                    IsInstanceHelper.IsInstanceOf(runnableCheck.Value, typeof(T));
                },
                IsInstanceHelper.BuildErrorMessage(runnableCheck.Value, typeof(T), true));
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A chainable fluent check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual instance is of the provided type.</exception>
        public static ICheckLink<ICheck<IEnumerable>> IsNotInstanceOf<T>(this ICheck<IEnumerable> check)
        {
            var checkRunner = check as ICheckRunner<IEnumerable>;
            var runnableCheck = check as IRunnableCheck<IEnumerable>;

            return checkRunner.ExecuteCheck(
                () =>
                {
                    IsInstanceHelper.IsNotInstanceOf(runnableCheck.Value, typeof(T));
                },
                IsInstanceHelper.BuildErrorMessage(runnableCheck.Value, typeof(T), false));
        }

        /// <summary>
        /// Checks that the enumerable contains all the given expected values, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the enumerable.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contain all the expected values.</exception>
        public static IExtendableCheckLink<IEnumerable> Contains<T>(this ICheck<IEnumerable> check, params T[] expectedValues)
        {
            IEnumerable properExpectedValues = ExtractEnumerableValueFromPossibleOneValueArray(expectedValues);
            return check.Contains(properExpectedValues);
        }

        /// <summary>
        /// Checks that the enumerable contains all the values present in another enumerable, in any order.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="otherEnumerable">The enumerable containing the expected values to be found.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contain all the expected values present in the other enumerable.</exception>
        public static IExtendableCheckLink<IEnumerable> Contains(this ICheck<IEnumerable> check, IEnumerable otherEnumerable)
        {
            var checkRunner = check as ICheckRunner<IEnumerable>;
            var runnableCheck = check as IRunnableCheck<IEnumerable>;

            checkRunner.ExecuteCheck(
                () =>
                {
                    if (runnableCheck.Value == null && otherEnumerable == null)
                    {
                        return;
                    }

                    if (runnableCheck.Value == null && otherEnumerable != null)
                    {
                        var message = FluentMessage.BuildMessage("The {0} is null and thus, does not contain the given expected value(s).").For("enumerable").On(runnableCheck.Value).And.Expected(otherEnumerable).ToString();
                        throw new FluentCheckException(message);
                    }

                    var notFoundValues = ExtractNotFoundValues(runnableCheck.Value, otherEnumerable);

                    if (notFoundValues.Count > 0)
                    {
                        var message = FluentMessage.BuildMessage(string.Format("The {{0}} does not contain the expected value(s):\n\t[{0}]", notFoundValues.ToEnumeratedString())).For("enumerable").On(runnableCheck.Value).And.Expected(otherEnumerable).ToString();
                        throw new FluentCheckException(message);
                    }
                }, 
                FluentMessage.BuildMessage(string.Format("The {{0}} contains all the given values whereas it must not.")).For("enumerable").On(runnableCheck.Value).And.Expected(otherEnumerable).ToString());

            return new ExtendableCheckLink<IEnumerable>(check, otherEnumerable);
        }

        /// <summary>
        /// Checks that the enumerable contains only the given values and nothing else, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the expected values to be found.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contain only the expected values provided.</exception>
        public static ICheckLink<ICheck<IEnumerable>> ContainsOnly<T>(this ICheck<IEnumerable> check, params T[] expectedValues)
        {
            IEnumerable properExpectedValues = ExtractEnumerableValueFromPossibleOneValueArray(expectedValues);

            return check.ContainsOnly(properExpectedValues);
       }

        /// <summary>
        /// Checks that the enumerable contains only the values present in another enumerable, and nothing else, in any order.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contain only the expected values present in the other enumerable.</exception>
        public static ICheckLink<ICheck<IEnumerable>> ContainsOnly(this ICheck<IEnumerable> check, IEnumerable expectedValues)
        {
            var checkRunner = check as ICheckRunner<IEnumerable>;
            var runnableCheck = check as IRunnableCheck<IEnumerable>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        // TODO: refactor this implementation?
                        if (runnableCheck.Value == null && expectedValues == null)
                        {
                            return;
                        }

                        if (runnableCheck.Value == null && expectedValues != null)
                        {
                            var message = FluentMessage.BuildMessage("The {0} is null and thus, does not contain exactly the given value(s).").For("enumerable").On(runnableCheck.Value).And.Expected(expectedValues).ToString();
                            throw new FluentCheckException(message);
                        }

                        if (runnableCheck.Value != null && runnableCheck.Value.Count() == 0 && expectedValues.Count() != 0)
                        {
                            var message = FluentMessage.BuildMessage("The {0} does not contain only the given value(s).\nIt contains no value at all!").For("enumerable").On(runnableCheck.Value).And.Expected(expectedValues).ToString();
                            throw new FluentCheckException(message);
                        }

                        var unexpectedValuesFound = ExtractUnexpectedValues(runnableCheck.Value, expectedValues);

                        if (unexpectedValuesFound.Count > 0)
                        {
                            var message = FluentMessage.BuildMessage(string.Format("The {{0}} does not contain only the given value(s).\nIt contains also other values:\n\t[{0}]", unexpectedValuesFound.ToEnumeratedString())).For("enumerable").On(runnableCheck.Value).And.Expected(expectedValues).ToString();
                            throw new FluentCheckException(message);
                        }
                }, 
                FluentMessage.BuildMessage("The {0} contains only the given values whereas it must not.").For("enumerable").On(runnableCheck.Value).And.Expected(expectedValues).ToString());
        }

        /// <summary>
        /// Checks that the enumerable contains only the given expected values and nothing else, in order.
        /// This check should only be used with IEnumerable that have a consistent iteration order
        /// (i.e. don't use it with <see cref="Hashtable" />, prefer <see cref="ContainsOnly{T}" /> in that case).
        /// </summary>
        /// <typeparam name="T">Type of the elements to be found.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contains only the exact given values and nothing else, in order.</exception>
        public static ICheckLink<ICheck<IEnumerable>> ContainsExactly<T>(this ICheck<IEnumerable> check, params T[] expectedValues)
        {
            IEnumerable properExpectedValues = ExtractEnumerableValueFromPossibleOneValueArray(expectedValues);
            return check.ContainsExactly(properExpectedValues);
        }

        /// <summary>
        /// Checks that the enumerable contains only the values of another enumerable and nothing else, in order.
        /// This check should only be used with IEnumerable that have a consistent iteration order
        /// (i.e. don't use it with <see cref="Hashtable" />, prefer <see cref="ContainsOnly{T}" /> in that case).
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="otherEnumerable">The other enumerable containing the exact expected values to be found.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable does not contains only the exact given values and nothing else, in order.</exception>
        public static ICheckLink<ICheck<IEnumerable>> ContainsExactly(this ICheck<IEnumerable> check, IEnumerable otherEnumerable)
        {
            var checkRunner = check as ICheckRunner<IEnumerable>;
            var runnableCheck = check as IRunnableCheck<IEnumerable>;

            return checkRunner.ExecuteCheck(
                () => 
                {
                    // TODO: refactor this implementation
                    if (runnableCheck.Value == null && otherEnumerable == null)
                    {
                        return;
                    }

                    if (runnableCheck.Value == null && otherEnumerable != null)
                    {
                        var message = FluentMessage.BuildMessage("The {0} is null and thus, does not contain exactly the given value(s).").For("enumerable").On(runnableCheck.Value).And.Expected(otherEnumerable).ToString();
                        throw new FluentCheckException(message);
                    }

                    if (otherEnumerable == null)
                    {
                        ThrowsNotExactlyException(runnableCheck.Value, null);
                    }

                    var first = runnableCheck.Value.GetEnumerator();
                    var enumerable = otherEnumerable as IList<object> ?? otherEnumerable.Cast<object>().ToList();
                    var second = enumerable.GetEnumerator();

                    while (first.MoveNext())
                    {
                        if (!second.MoveNext() 
                            || !object.Equals(first.Current, second.Current))
                        {
                            ThrowsNotExactlyException(runnableCheck.Value, enumerable);
                        }
                    }

                    if (second.MoveNext())
                    {
                        ThrowsNotExactlyException(runnableCheck.Value, enumerable);
                    }
                },
                BuildExceptionMessageForContainsExactly(runnableCheck.Value, otherEnumerable));
        }

        /// <summary>
        /// Checks that the enumerable has the proper number of elements.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedSize">The expected size to be found.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable has not the expected number of elements.</exception>
        public static ICheckLink<ICheck<IEnumerable>> HasSize(this ICheck<IEnumerable> check, long expectedSize)
        {
            var checkRunner = check as ICheckRunner<IEnumerable>;
            var runnableCheck = check as IRunnableCheck<IEnumerable>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        HasSizeImpl(runnableCheck.Value, expectedSize);
                    },
                BuildHasSizeExceptionMessage(runnableCheck.Value));
        }

        private static void HasSizeImpl(IEnumerable checkedEnumerable, long expectedSize)
        {
            long itemsCount = checkedEnumerable.Cast<object>().LongCount();

            if (expectedSize != itemsCount)
            {
                var foundElementsNumberDescription = BuildElementNumberLiteral(itemsCount);

                var elements = checkedEnumerable.ToEnumeratedString();

                throw new FluentCheckException(string.Format("\nThe actual enumerable has {0} instead of {1}.\nActual content is:\n\t[{2}].", foundElementsNumberDescription, expectedSize, elements));
            }
        }

        private static string BuildHasSizeExceptionMessage(IEnumerable checkedEnumerable)
        {
            long itemsCount = checkedEnumerable.Cast<object>().LongCount();
            var foundElementsNumberDescription = BuildElementNumberLiteral(itemsCount);

            return string.Format("\nThe actual enumerable has {0} which is unexpected.\nActual content is:\n\t[{1}].", foundElementsNumberDescription, checkedEnumerable.ToEnumeratedString());
        }

        private static string BuildElementNumberLiteral(long itemsCount)
        {
            string foundElementsNumberDescription = itemsCount.ToString();
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

        /// <summary>
        /// Checks that the enumerable is empty.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The enumerable is not empty.</exception>
        public static ICheckLink<ICheck<IEnumerable>> IsEmpty(this ICheck<IEnumerable> check)
        {
            var checkRunner = check as ICheckRunner<IEnumerable>;
            var runnableCheck = check as IRunnableCheck<IEnumerable>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        if (runnableCheck.Value.Cast<object>().Any())
                        {
                            throw new FluentCheckException(string.Format("\nThe actual enumerable is not empty. Contains:\n\t[{0}]", runnableCheck.Value.ToEnumeratedString()));
                        }
                    },
                FluentMessage.BuildMessage("The actual enumerable is empty, which is unexpected.").ToString());
        }

        #region private or internal methods

        /// <summary>
        /// Returns all expected values that aren't present in the enumerable.
        /// </summary>
        /// <param name="enumerable">The enumerable to inspect.</param>
        /// <param name="expectedValues">The expected values to search within the enumerable.</param>
        /// <returns>
        /// A list containing all the expected values that aren't present in the enumerable.
        /// </returns>
        internal static IList ExtractNotFoundValues(IEnumerable enumerable, IEnumerable expectedValues)
        {
            // Prepares the list to return
            var notFoundValues = new List<object>();
            foreach (var expectedValue in expectedValues)
            {
                notFoundValues.Add(expectedValue);
            }

            foreach (var element in enumerable)
            {
                foreach (var expectedValue in expectedValues)
                {
                    if (object.Equals(element, expectedValue))
                    {
                        notFoundValues.RemoveAll(one => one.Equals(expectedValue));
                        break;
                    }
                }
            }

            return notFoundValues;
        }

        /// <summary>
        /// Returns all the values of the enumerable that don't belong to the expected ones.
        /// </summary>
        /// <param name="enumerable">The enumerable to inspect.</param>
        /// <param name="expectedValues">The allowed values to be part of the enumerable.</param>
        /// <returns>
        /// A list with all the values found in the enumerable that don't belong to the expected ones.
        /// </returns>
        internal static IList ExtractUnexpectedValues(IEnumerable enumerable, IEnumerable expectedValues)
        {
            var unexpectedValuesFound = new List<object>();
            foreach (var element in enumerable)
            {
                var isExpectedValue = expectedValues.Cast<object>().Contains(element);

                if (!isExpectedValue)
                {
                    unexpectedValuesFound.Add(element);
                }
            }

            return unexpectedValuesFound;
        }

        private static bool IsAOneValueArrayWithOneCollectionInside<T>(T[] expectedValues)
        {
            // For every collections like ArrayList, List<T>, IEnumerable<T>, StringCollection, etc.
            return expectedValues != null && (expectedValues.LongLength == 1) && IsAnEnumerableButNotAnEnumerableOfChars(expectedValues[0]);
        }

        private static bool IsAnEnumerableButNotAnEnumerableOfChars<T>(T element)
        {
            return (element is IEnumerable) && !(element is IEnumerable<char>);
        }

        private static string BuildExceptionMessageForContainsExactly(IEnumerable checkedValue, IEnumerable enumerable)
        {
            return FluentMessage.BuildMessage("The {0} contains exactly the given values whereas it must not.")
                                    .For("enumerable")
                                    .On(checkedValue)
                                    .WithEnumerableCount(checkedValue.Count())
                                    .ToString();
        }

        private static void ThrowsNotExactlyException(IEnumerable checkedValue, IList<object> enumerable)
        {
            var message = FluentMessage.BuildMessage("The {0} does not contain exactly the expected value(s).")
                                        .For("enumerable")
                                        .On(checkedValue)
                                        .WithEnumerableCount(checkedValue.Count())
                                        .And.Expected(enumerable)
                                        .WithEnumerableCount(enumerable.Count())
                                        .ToString();

            throw new FluentCheckException(message);
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
                properExpectedValues = expectedValues as IEnumerable;
            }

            return properExpectedValues;
        }

        #endregion
    }
}
