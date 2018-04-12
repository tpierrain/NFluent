// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ReflectionWrapperChecks.cs" company="NFluent">
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
    using Extensibility;
    using Extensions;
    using Helpers;

    /// <summary>
    ///     Hosts reflection-based checks <see cref="ObjectFieldsCheckExtensions.Considering{T}" />
    /// </summary>
    public static class ReflectionWrapperChecks
    {
        /// <summary>
        /// </summary>
        /// <param name="check"></param>
        /// <param name="expectedValue"></param>
        /// <returns></returns>
        public static ICheckLink<ICheck<ReflectionWrapper>> IsEqualTo<TU>(this ICheck<ReflectionWrapper> check,
            TU expectedValue)
        {
            FieldEqualTest(check, expectedValue, true);
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.BuildChainingObject();
        }

        internal static void FieldEqualTest<TU>(ICheck<ReflectionWrapper> check, TU expectedValue, bool mustMatch)
        {
            ExtensibilityHelper.BeginCheck(check)
                .Analyze((sut, test) =>
                {
                    foreach (var match in sut.MemberMatches(expectedValue))
                    {
                        if (match.DoValuesMatches != mustMatch)
                        {
                            match.Check(test);
                        }
                    }
                }).Negates("The {0} is equal to one of {1} whereas it should not.")
                .EndCheck();
        }

        /// <summary>
        ///     Checks if the extracted members match one of the provided expected values.
        /// </summary>
        /// <param name="check">Checker logiv.</param>
        /// <param name="values">List of possible values</param>
        /// <returns>A link object</returns>
        public static ICheckLink<ICheck<ReflectionWrapper>> IsOneOf(this ICheck<ReflectionWrapper> check,
            params object[] values)
        {
            ExtensibilityHelper.BeginCheck(check)
                .ExpectingValues(values, values.Length, "one of", "none of")
                .Analyze((sut, test) =>
                {
                    bool match = false;
                    foreach (var value in values)
                    {
                        match = true;
                        foreach (var memberMatch in sut.MemberMatches(value))
                        {
                            if (!memberMatch.DoValuesMatches)
                            {
                                match = false;
                                break;
                            }

                        }

                        if (match)
                        {
                            break;
                        }
                    }

                    test.FailsIf(_ => !match, "The {0} is equal to none of the {1} whereas it should.");
                }).Negates("The {0} is equal to one of {1} whereas it should not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <inheritdoc cref="ObjectCheckExtensions.IsNull{T}(NFluent.ICheck{T})" />
        public static ICheckLink<ICheck<ReflectionWrapper>> IsNull(this ICheck<ReflectionWrapper> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheck(() =>
                {
                    checker.Value.ScanFields((scan, depth) =>
                    {
                        if (depth <= 0 && scan.Value != null)
                        {
                            var message = checker.BuildMessage("The {0} has a non null member, whereas it should not.")
                                .On(scan);
                            throw new FluentCheckException(message.ToString());
                        }

                        return scan.Value != null;
                    });
                },
                checker.BuildShortMessage("The {0} has only null member, whereas it should not.").ToString());
        }

        /// <inheritdoc cref="ObjectCheckExtensions.IsNotNull{T}(NFluent.ICheck{T})" />
        public static ICheckLink<ICheck<ReflectionWrapper>> IsNotNull(this ICheck<ReflectionWrapper> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheck(() =>
                {
                    checker.Value.ScanFields((scan, depth) =>
                    {
                        if (depth > 0 || scan.Value != null)
                        {
                            return scan.Value != null;
                        }

                        var message =
                            checker.BuildShortMessage("The {0} has a null member, whereas it should not.");
                        throw new FluentCheckException(message.ToString());
                    });
                },
                checker.BuildMessage("The {0} has no null member, whereas it should.").ToString());
        }

        /// <inheritdoc cref="ObjectCheckExtensions.IsSameReferenceAs{T,TU}" />
        public static ICheckLink<ICheck<ReflectionWrapper>> IsSameReferenceAs<TU>(this ICheck<ReflectionWrapper> check,
            TU expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheck(() =>
                {
                    var expectedWrapper =
                        ReflectionWrapper.BuildFromInstance(typeof(TU), expected, checker.Value.Criteria);
                    expectedWrapper.MapFields(checker.Value, 1, (scan, match, depth) =>
                    {
                        if (depth > 0)
                        {
                            return true;
                        }

                        if (match == null)
                        {
                            var result = checker.BuildShortMessage(
                                    $"The {{1}}'s {scan.MemberLabel.DoubleCurlyBraces()} is absent from the {{0}}.")
                                .For("value").Expected(scan.Value)
                                .Label($"The {{0}} {scan.MemberLabel.DoubleCurlyBraces()}:");
                            throw new FluentCheckException(result.ToString());
                        }

                        if (scan == null)
                        {
                            var result = checker.BuildShortMessage(
                                    $"The {{0}}'s {match.MemberLabel.DoubleCurlyBraces()} is absent from the {{1}}.")
                                .For("value").On(match.Value)
                                .Label($"The {{1}} {match.MemberLabel.DoubleCurlyBraces()}:");
                            throw new FluentCheckException(result.ToString());
                        }

                        if (!ReferenceEquals(scan.Value, match.Value))
                        {
                            var message =
                                checker.BuildShortMessage(
                                        $"The {{0}}'s {match.MemberLabel.DoubleCurlyBraces()} does not reference the expected instance.")
                                    .For("value").On(match).And.Expected(scan);
                            throw new FluentCheckException(message.ToString());
                        }

                        return scan.Value != null;
                    });
                },
                checker.BuildMessage("The {0} has no null member, whereas it should.").ToString());
        }

        /// <inheritdoc cref="ObjectCheckExtensions.IsDistinctFrom{T,TU}" />
        public static ICheckLink<ICheck<ReflectionWrapper>> IsDistinctFrom<TU>(this ICheck<ReflectionWrapper> check,
            TU expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheck(() =>
                {
                    var expectedWrapper =
                        ReflectionWrapper.BuildFromInstance(typeof(TU), expected, checker.Value.Criteria);
                    expectedWrapper.MapFields(checker.Value, 1, (scan, match, depth) =>
                    {
                        if (depth > 0)
                        {
                            return true;
                        }

                        if (match == null)
                        {
                            var result = checker.BuildShortMessage(
                                    $"The {{1}}'s {scan.MemberLabel.DoubleCurlyBraces()} is absent from the {{0}}.")
                                .For("value").Expected(scan.Value)
                                .Label($"The {{0}} {scan.MemberLabel.DoubleCurlyBraces()}:");
                            throw new FluentCheckException(result.ToString());
                        }

                        if (scan == null)
                        {
                            var result = checker.BuildShortMessage(
                                    $"The {{0}}'s {match.MemberLabel.DoubleCurlyBraces()} is absent from the {{1}}.")
                                .For("value").On(match.Value)
                                .Label($"The {{1}} {match.MemberLabel.DoubleCurlyBraces()}:");
                            throw new FluentCheckException(result.ToString());
                        }

                        if (ReferenceEquals(scan.Value, match.Value))
                        {
                            var message =
                                checker.BuildShortMessage(
                                        $"The {{0}}'s {match.MemberLabel.DoubleCurlyBraces()} does reference the reference instance, whereas it should not.")
                                    .For("value").On(match);
                            throw new FluentCheckException(message.ToString());
                        }

                        return scan.Value != null;
                    });
                },
                checker.BuildMessage("The {0} has no null member, whereas it should.").ToString());
        }
    }
}