// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnumCheckExtensions.cs" company="NFluent">
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
    using System.Diagnostics.CodeAnalysis;
    using Extensions;

    /// <summary>
    /// Hosts Enum specific checks
    /// </summary>
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public static class EnumCheckExtensions
    {
        /// <summary>
        /// Checks that an enum does not have a flag set
        /// </summary>
        /// <typeparam name="T">type of checked enum</typeparam>
        /// <param name="check">check object</param>
        /// <param name="flag">flag to check if it is present</param>
        /// <returns>link object for further checks</returns>
        public static ICheckLink<ICheck<T>> DoesNotHaveFlag<T>(this ICheck<T> check, T flag)
            where T : struct, IConvertible
        {
            var checker = ((ICheckForExtensibility<T, ICheck<T>>)check).Checker;
            // TODO: factorize this into a method
            return checker.Negated ? checker.BuildChainingObject().And.HasFlag(flag) : check.Not.HasFlag(flag);
        }

        /// <summary>
        /// Checks that an enum does have a flag set
        /// </summary>
        /// <typeparam name="T">type of checked enum</typeparam>
        /// <param name="check">check object</param>
        /// <param name="flag">flag to check if it is present</param>
        /// <returns>link object for further checks</returns>
        public static ICheckLink<ICheck<T>> HasFlag<T>(this ICheck<T> check, T flag) where T : struct, IConvertible
        {
            var checker = ((ICheckForExtensibility<T, ICheck<T>>)check).Checker;
            if (!typeof(T).TypeHasAttribute(typeof(FlagsAttribute)))
            {
                var message  = checker.BuildMessage("The checked enum type is not a set of flags. You must add [Flags] attribute to its declaration.")
                    .For("enum")
                    .On(checker.Value)
                    .And
                    .Expected(flag);
                throw new FluentCheckException(message.ToString());
            }

            if (Convert.ToInt64(flag) == 0)
            {
                var message  = checker.BuildMessage("Wrong chek: The expected flag is 0. You must use IsEqualTo, or a non zero flag value.")
                        .For("enum")
                        .On(checker.Value)
                        .And
                        .Expected(flag);
                throw new FluentCheckException(message.ToString());
            }

            return checker.ExecuteCheck(() =>
                {
                    var sutAsInt = Convert.ToInt64(checker.Value);
                    var exptectedAsInt = Convert.ToInt64(flag);
                    if ((sutAsInt & exptectedAsInt) == exptectedAsInt)
                    {
                        return;
                    }

                    var message  = checker.BuildMessage("The {0} does not have the expected flag.").For("enum").On(checker.Value).And
                        .Expected(flag);
                    throw new FluentCheckException(message.ToString());
                },
                checker.BuildMessage("The {0} does have the expected flag, whereas it should not.").For("enum").On(checker.Value).And
                    .Expected(flag).ToString());
        }
    }
}