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

// ReSharper disable once CheckNamespace
namespace NFluent
{
    using System;
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
            ExtensibilityHelper.BeginCheck(check)
                .Analyze((sut, test) =>
                {
                    var reflectionSut = sut as ReflectionWrapper;
                    if (reflectionSut != null)
                    {
                        var expectedWrapper = ReflectionWrapper.BuildFromType(type, reflectionSut.Criteria);
                        expectedWrapper.MapFields(reflectionSut, 1, (expected, actual, depth) =>
                        {
                            if (actual != null && expected != null && actual.ValueType != expected.ValueType)
                            {
                                if (actual.ValueType.IsPrimitive() || expected.ValueType.IsPrimitive())
                                {
                                    test.CheckSutAttributes(_ => actual.Value, actual.MemberLabel)
                                        .Fails("The {0} is of a different type than the {1}.")
                                        .ExpectingType(expected.ValueType, "", "");
                                    return false;
                                }

                                return true;
                            }

                            if (actual != null && expected != null && expected.ValueType == actual.ValueType)
                            {
                                return false;
                            }

                            if (actual == null) 
                            {
                                test.CheckSutAttributes(_ => expectedWrapper.Value, expected.MemberLabel)
                                    .Expecting(expected)
                                    .Fails("The {1} is absent from the {0}.", MessageOption.NoCheckedBlock);
                            }
                            else
                            {
                                test.CheckSutAttributes(_ => actual, actual.MemberLabel.DoubleCurlyBraces())
                                    .Fails("The {0} is absent from the {1}.");
                            }
                            return false;
                        });
                    }
                    else if (typeof(T).IsNullable())
                    {
                        test.FailWhen(sut2 => typeof(T)!= type || (sut2 == null && !typeof(T).IsNullable()),
                            $"The {{0}} is not an instance of [{type.ToStringProperlyFormatted()}].", MessageOption.WithType);
                    }
                    else
                    {
                        test.FailWhen(sut2 => sut2.GetTypeWithoutThrowingException() != type,
                            $"The {{0}} is not an instance of [{type.ToStringProperlyFormatted()}].", sut != null ? MessageOption.WithType : MessageOption.None);
                    }
                })
                .ExpectingType(type, "", "different from")
                .Negates($"The {{0}} is an instance of [{type.ToStringProperlyFormatted()}] whereas it must not.", MessageOption.WithType)
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
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
            return check.Not.IsInstanceOfType(type);
        }
    }
}