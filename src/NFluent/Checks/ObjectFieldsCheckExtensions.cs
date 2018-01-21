// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ObjectFieldsCheckExtensions.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using Extensibility;
    using Extensions;
    using Helpers;
    using Kernel;

    internal class Criteria
    {
        private List<string> exclusion;

        internal Criteria(BindingFlags bindingFlags, bool withFields = true, bool withProperties = false)
        {
            this.BindingFlags = bindingFlags;
            this.WithFields = withFields;
            this.WithProperties = withProperties;
        }

        public BindingFlags BindingFlags { get; internal set; }

        public bool WithFields { get; internal set; }

        public bool WithProperties { get; internal set; }

        public bool IsNameExcluded(string name)
        {
            return this.exclusion?.Contains(name) ?? false;
        }

        public void SetExclusion(string[] fields)
        {
            this.exclusion = new List<string>(fields);
        }
    }

    /// <summary>
    ///     Provides check methods to be executed on an object instance.
    /// </summary>
    public static class ObjectFieldsCheckExtensions
    {
        private static readonly Criteria FlagsForFields =
            new Criteria(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);


        /// <summary>
        ///     Checks that the actual actualValue has fields equals to the expected actualValue ones.
        /// </summary>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected actualValue.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual actualValue doesn't have all fields equal to the expected actualValue ones.
        /// </exception>
        /// <remarks>
        ///     The comparison is done field by field.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use HasFieldsWithSameValues instead.")]
        public static ICheckLink<ICheck<object>> HasFieldsEqualToThose(this ICheck<object> check, object expected)
        {
            return HasFieldsWithSameValues(check, expected);
        }

        /// <summary>
        ///     Checks that the actual actualValue doesn't have all fields equal to the expected actualValue ones.
        /// </summary>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected actualValue.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual actualValue has all fields equal to the expected actualValue ones.
        /// </exception>
        /// <remarks>
        ///     The comparison is done field by field.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use HasNotFieldsWithSameValues instead.")]
        public static ICheckLink<ICheck<object>> HasFieldsNotEqualToThose(this ICheck<object> check, object expected)
        {
            return HasNotFieldsWithSameValues(check, expected);
        }

        /// <summary>
        ///     Checks that the actual actualValue has fields equals to the expected actualValue ones.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the checked actualValue.
        /// </typeparam>
        /// <typeparam name="TU">Type of the expected actualValue.</typeparam>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected actualValue.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual actualValue doesn't have all fields equal to the expected actualValue ones.
        /// </exception>
        /// <remarks>
        ///     The comparison is done field by field.
        /// </remarks>
        public static ICheckLink<ICheck<T>> HasFieldsWithSameValues<T, TU>(this ICheck<T> check, TU expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var message = CheckMemberEquality(checker, checker.Value, expected, checker.Negated, FlagsForFields, true);

            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        ///     Checks that the actual actualValue doesn't have all fields equal to the expected actualValue ones.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the checked actualValue.
        /// </typeparam>
        /// <param name="check">
        ///     The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        ///     The expected actualValue.
        /// </param>
        /// <returns>
        ///     A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        ///     The actual actualValue has all fields equal to the expected actualValue ones.
        /// </exception>
        /// <remarks>
        ///     The comparison is done field by field.
        /// </remarks>
        public static ICheckLink<ICheck<T>> HasNotFieldsWithSameValues<T>(this ICheck<T> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var negated = !checker.Negated;

            var message = CheckMemberEquality(checker, checker.Value, expected, negated, FlagsForFields, true);
            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return checker.BuildChainingObject();
        }


        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="check"></param>
        /// <returns></returns>
        public static IPublicOrNot Considering<T>(this ICheck<T> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var fieldsWrapper = ReflectionWrapper.BuildFromInstance(typeof(T), checker.Value,
                new Criteria(BindingFlags.Instance, false));
            var checkWithConsidering = new CheckWithConsidering(fieldsWrapper, checker.Negated);
            return checkWithConsidering;
        }

        /// <summary>
        /// </summary>
        /// <param name="check"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static ICheckLink<ICheck<ReflectionWrapper>> IsEqualTo<TU>(this ICheck<ReflectionWrapper> check,
            TU expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var expectedWrapper = ReflectionWrapper.BuildFromInstance(typeof(TU), expected, checker.Value.Criteria);

            var message = CompareMembers(checker, false, false, expectedWrapper, checker.Value);
            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        /// </summary>
        /// <param name="check"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ICheckLink<ICheck<ReflectionWrapper>> IsInstanceOfType(this ICheck<ReflectionWrapper> check,
            Type type)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var sut = checker.Value;
            var expectedWrapper = ReflectionWrapper.BuildFromType(type, checker.Value.Criteria);
            return checker.ExecuteCheck(() =>
            {
                var result = new List<MemberMatch>();
                expectedWrapper.MapFields(sut, 1, (expected, actual, depth) =>
                {
                    if (actual != null && expected != null && actual.ValueType != expected.ValueType)
                    {
                        return true;
                    }

                    if (actual != null && expected != null && expected.ValueType == actual.ValueType)
                    {
                        return false;
                    }

                    result.Add(new MemberMatch(expected, actual));
                    return false;
                });
                if (result.Count > 0)
                {
                    var message = result[0].BuildMessage(checker, false);
                    throw new FluentCheckException(message.ToString());
                }
            }, "");
        }

        private static string CheckMemberEquality<T, TU>(
            IChecker<T, ICheck<T>> checker,
            T value,
            TU expected,
            bool negated,
            Criteria criteria,
            bool disregardExtra)
        {
            var expectedValue =
                ReflectionWrapper.BuildFromInstance(expected?.GetType() ?? typeof(TU), expected, criteria);
            var actualValue = ReflectionWrapper.BuildFromInstance(value?.GetType() ?? typeof(T), value, criteria);

            return CompareMembers(checker, negated, disregardExtra, expectedValue, actualValue);
        }

        private static string CompareMembers<T>(IChecker<T, ICheck<T>> checker, bool negated, bool disregardExtra,
            ReflectionWrapper expectedValue, ReflectionWrapper actualValue)
        {
            var result = new List<MemberMatch>();
            expectedValue.MapFields(actualValue, 1, (expected, actual, depth) =>
            {
                if (disregardExtra && expected == null)
                {
                    return true;
                }

                if (actual?.Value == null || expected?.Value == null)
                {
                    result.Add(new MemberMatch(expected, actual));
                    return false;
                }

                if (depth <= 0 && expected.ValueType.ImplementsEquals())
                {
                    result.Add(new MemberMatch(expected, actual));
                    return false;
                }

                if (!expected.IsArray)
                {
                    return true;
                }

                if (actual.IsArray && ((Array) expected.Value).Length == ((Array) actual.Value).Length)
                {
                    return true;
                }

                result.Add(new MemberMatch(expected, actual));
                return false;
            });

            foreach (var match in result)
            {
                var message = match.BuildMessage(checker, negated);
                if (message != null)
                {
                    return message.ToString();
                }
            }

            return null;
        }
    }
}