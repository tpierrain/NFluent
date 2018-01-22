// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ObjectChecksNonGenericVariants.cs" company="NFluent">
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
    using Extensibility;
    using Extensions;
    using Helpers;

    /// <summary>
    /// Hosts non generic variants of some checks.
    /// </summary>
    public static class ObjectChecksNonGenericVariants
    {
        /// <summary>
        /// Checks if an object is an instance of a specified type.
        /// </summary>
        /// <param name="check">checker logic object</param>
        /// <param name="type">expected type</param>
        /// <typeparam name="T">type of checked object</typeparam>
        /// <returns>A check link</returns>
        public static ICheckLink<ICheck<T>> IsInstanceOfType<T>(this ICheck<T> check, Type type)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            ICheckLink<ICheck<T>> result;
            if (typeof(T) == typeof(ReflectionWrapper))
            {
                var checkerWrapper = ExtensibilityHelper.ExtractChecker((ICheck<ReflectionWrapper>) check);
                var sut = checkerWrapper.Value;
                var expectedWrapper = ReflectionWrapper.BuildFromType(type, checkerWrapper.Value.Criteria);
                result = checker.ExecuteCheck(() =>
                {
                    var scanResult = new List<MemberMatch>();
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

                        scanResult.Add(new MemberMatch(expected, actual));
                        return false;
                    });
                    if (scanResult.Count > 0)
                    {
                        var message = scanResult[0].BuildMessage(checker, false);
                        throw new FluentCheckException(message.ToString());
                    }
                }, IsInstanceHelper.BuildErrorMessage(checker.Value, type, true));
            }
            else if (typeof(T).IsNullable())
            {
                result = checker.ExecuteCheck(
                    () => IsInstanceHelper.IsSameType(typeof(T), type, checker.Value), 
                    IsInstanceHelper.BuildErrorMessageForNullable(typeof(T), type, checker.Value, true));
            }
            else
            {
                    result = checker.ExecuteCheck(() => IsInstanceHelper.IsInstanceOf(checker.Value, type),
                        IsInstanceHelper.BuildErrorMessage(checker.Value, type, true));
            }
            return result;
        }

        /// <summary>
        /// Checks if an object is not an instance of a specified type.
        /// </summary>
        /// <param name="check">checker logic object</param>
        /// <param name="type">expected type</param>
        /// <typeparam name="T">type of checked object</typeparam>
        /// <returns>A check link</returns>
        public static ICheckLink<ICheck<T>> IsNoInstanceOfType<T>(this ICheck<T> check, Type type)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            ICheckLink<ICheck<T>> result;
            if (typeof(T) == typeof(ReflectionWrapper))
            {
                var checkerWrapper = ExtensibilityHelper.ExtractChecker((ICheck<ReflectionWrapper>) check);
                var sut = checkerWrapper.Value;
                var expectedWrapper = ReflectionWrapper.BuildFromType(type, checkerWrapper.Value.Criteria);
                result = checker.ExecuteCheck(() =>
                {
                    var scanResult = new List<MemberMatch>();
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

                        scanResult.Add(new MemberMatch(expected, actual));
                        return false;
                    });
                    if (scanResult.Count != 0)
                    {
                        return;
                    }

                    var message = IsInstanceHelper.BuildErrorMessage(checker.Value, type, true);
                    throw new FluentCheckException(message);
                }, IsInstanceHelper.BuildErrorMessage(checker.Value, type, false));
            }
            else if (typeof(T).IsNullable())
            {
                result = checker.ExecuteCheck(
                    () => IsInstanceHelper.IsDifferentType(typeof(T), type, checker.Value),
                    IsInstanceHelper.BuildErrorMessageForNullable(typeof(T), type, checker.Value, false));
            }
            else
            {
                result = checker.ExecuteCheck(
                        () => IsInstanceHelper.IsNotInstanceOf(checker.Value, type),
                        IsInstanceHelper.BuildErrorMessage(checker.Value, type, false));
            }
            return result;
        }
    }
}