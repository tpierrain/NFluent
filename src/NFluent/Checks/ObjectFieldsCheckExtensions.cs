#region File header

 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="ObjectFieldsCheckExtensions.cs" company="">
 //   Copyright 2014 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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

#endregion

namespace NFluent
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using Extensibility;
    using Helpers;
    using Kernel;

    /// <summary>
    /// 
    /// </summary>
    public class Criteria
    {
        internal Criteria(BindingFlags bindingFlags)
        {
            this.BindingFlags = bindingFlags;
        }

        /// <summary>
        /// 
        /// </summary>
        public BindingFlags BindingFlags { get;}
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Private
    {
        /// <summary>
        /// 
        /// </summary>
        public static Criteria Fields { get; } = new Criteria(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
    }

    /// <summary>
    /// 
    /// </summary>
    public class Public
    {
        /// <summary>
        /// 
        /// </summary>
        public static Criteria Fields { get; } = new Criteria(BindingFlags.Instance | BindingFlags.Public);
    }

    /// <summary>
    ///     Provides check methods to be executed on an object instance.
    /// </summary>
    public static class ObjectFieldsCheckExtensions
    {
        private const BindingFlags FlagsForFields =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;


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
            var message = CheckFieldEquality(checker, checker.Value, expected, checker.Negated, FlagsForFields);

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

            var message = CheckFieldEquality(checker, checker.Value, expected, negated, FlagsForFields);

            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="check"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static  ICheck<ReflectionWrapper> Considering<T>(this ICheck<T> check, Criteria criteria)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var fieldsWrapper = new ReflectionWrapper(typeof(T), criteria.BindingFlags);
            fieldsWrapper.SetFieldValue(checker.Value);
            return new FluentCheck<ReflectionWrapper>(fieldsWrapper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static ICheckLink<ICheck<ReflectionWrapper>> IsEqualTo<TU>(this ICheck<ReflectionWrapper> check,
            TU expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var expectedWrapper = new ReflectionWrapper(typeof(TU), checker.Value.Flags);
            expectedWrapper.SetFieldValue(expected);

            var message = CompareFields(checker, false, expectedWrapper, checker.Value);
            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return checker.BuildChainingObject();
        }

        private static string CheckFieldEquality<T, TU>(
            IChecker<T, ICheck<T>> checker,
            T value,
            TU expected,
            bool negated,
            BindingFlags flags)
        {
            var expectedValue = new ReflectionWrapper(expected?.GetType() ?? typeof(TU), flags);
            expectedValue.SetFieldValue(expected);
            var actualValue = new ReflectionWrapper(value?.GetType() ?? typeof(T), flags);
            actualValue.SetFieldValue(value);

            return CompareFields(checker, negated, expectedValue, actualValue);
        }

        private static string CompareFields<T>(IChecker<T, ICheck<T>> checker, bool negated,
            ReflectionWrapper expectedValue, ReflectionWrapper actualValue)
        {
            var analysis = expectedValue.CompareValue(actualValue, new List<object>(), 1);

            foreach (var fieldMatch in analysis)
            {
                var result = fieldMatch.BuildMessage(checker, negated);
                if (result != null)
                {
                    return result.ToString();
                }
            }

            return null;
        }
    }
}