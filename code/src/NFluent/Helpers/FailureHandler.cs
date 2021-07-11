// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="FailureHandler.cs" company="NFluent">
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

namespace NFluent.Helpers
{
    using System;
    using System.Linq;
    using Extensibility;
    using Extensions;

    /// <summary>
    /// Hosts meta checks (checks for checks)
    /// </summary>
    public static class FailureHandler
    {
        /// <summary>
        ///     Verify that the code results in a failed NFluent check with a specified message.
        /// </summary>
        /// <param name="check"></param>
        /// <param name="lines"></param>
        /// <returns>A link check</returns>
        public static ICheckLink<ICheck<RunTrace>> IsAFailingCheckWithMessage(this ICheck<RunTrace> check,
            params Criteria[] lines)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("fluent check")
                .CheckSutAttributes((sut) => sut.RaisedException, "raised exception")
                .FailIfNull("The check succeeded whereas it should have failed.")
                .FailWhen(sut => ExceptionHelper.FailedExceptionType() != sut.GetTypeWithoutThrowingException(),
                    $"The exception raised is not of the expected type.")
                .DefineExpectedType(ExceptionHelper.FailedExceptionType())
                .CheckSutAttributes((sut) => sut.Message, "error message")
                .Analyze((message, test) =>
                {
                    var messageLines = message.SplitAsLines();
                    var expectedLines = (lines.Length == 1) ? lines[0].SplitAsLines() : lines;
                    for (var i = 0; i < expectedLines.Length; i++)
                    {
                        if (messageLines.Count <= i)
                        {
                            test.Fail($"Lines are missing in the error message starting at #{i}");
                            break;
                        }

                        if (!expectedLines[i].IsEqualTo(messageLines[i]))
                        {
                            test.Fail($"Line {i} is different from what is expected" + Environment.NewLine +
                                      "Act:" + messageLines[i].DoubleCurlyBraces() + Environment.NewLine +
                                      "Exp:" + expectedLines[i].ToString().DoubleCurlyBraces());
                            break;
                        }

                        if (messageLines.Count > expectedLines.Length)
                        {
                            test.Fail($"Too many lines in the error message starting at #{expectedLines.Length}");
                        }
                    }
                }).DefineExpectedValue(string.Join(Environment.NewLine, lines.Select(l => l.ToString()).ToArray())).
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the code results in a failed NFluent check with a specified message.
        /// </summary>
        /// <param name="check"></param>
        /// <returns>A link check</returns>
        public static ICheckLink<ICheck<RunTrace>> IsAFailingCheck(this ICheck<RunTrace> check)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("fluent check")
                .CheckSutAttributes((sut) => sut.RaisedException, "raised exception")
                .FailIfNull("The fluent check did not raise an exception, where as it must.")
                .FailWhen((sut) => ExceptionHelper.FailedExceptionType() != sut.GetTypeWithoutThrowingException(),
                    "The exception raised is not of the expected type").
                DefineExpectedType(ExceptionHelper.FailedExceptionType()).
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the code results in a failed NFluent assumption.
        /// </summary>
        /// <param name="check"></param>
        /// <returns>A link check</returns>
        public static ICheckLink<ICheck<RunTrace>> IsAFailingAssumption(this ICheck<RunTrace> check)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("fluent assumption")
                .CheckSutAttributes((sut) => sut.RaisedException, "raised exception")
                .FailIfNull("The assumption succeeded whereas it should have failed.")
                .FailWhen(sut => ExceptionHelper.InconclusiveExceptionType() != sut.GetTypeWithoutThrowingException(),
                    "The exception raised is not of the expected type").
                DefineExpectedType(ExceptionHelper.InconclusiveExceptionType()).
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the code results in a failed NFluent assumption with a specified message.
        /// </summary>
        /// <param name="check"></param>
        /// <param name="criteria"></param>
        /// <returns>A link check</returns>
        public static ICheckLink<ICheck<RunTrace>> IsAFailingAssumptionWithMessage(this ICheck<RunTrace> check, params Criteria[] criteria)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("fluent assumption")
                .CheckSutAttributes((sut) => sut.RaisedException, "raised exception")
                .FailIfNull("The assumption succeeded whereas it should have failed.")
                .FailWhen((sut) => ExceptionHelper.InconclusiveExceptionType() != sut.GetTypeWithoutThrowingException(),
                    $"The exception raised is not of the expected type.")
                .DefineExpectedType(ExceptionHelper.InconclusiveExceptionType())
                .CheckSutAttributes((sut) => sut.Message.SplitAsLines(), "error message")
                .Analyze((messageLines, test) =>
                {
                    var expectedLines = (criteria.Length == 1) ? criteria[0].SplitAsLines() : criteria;
                    for (var i = 0; i < expectedLines.Length; i++)
                    {
                        if (messageLines.Count <= i)
                        {
                            test.Fail($"Lines are missing in the error message starting at #{i}");
                            break;
                        }

                        if (!expectedLines[i].IsEqualTo(messageLines[i]))
                        {
                            test.Fail($"Line {i} is different from what is expected"+Environment.NewLine+
                                       "Act:"+messageLines[i].DoubleCurlyBraces()+Environment.NewLine+
                                        "Exp:"+expectedLines[i].ToString().DoubleCurlyBraces());
                            break;
                        }
                    }

                    if (messageLines.Count > expectedLines.Length)
                    {
                        test.Fail($"Too many lines in the error message starting at #{expectedLines.Length}");
                    }
                }).
                DefineExpectedValue(criteria).
                EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

    }
}