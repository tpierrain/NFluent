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
    using System.Text.RegularExpressions;
    using Extensibility;
    using Extensions;

    /// <summary>
    /// Hosts meta checks (checks for checks)
    /// </summary>
    public static class FailureHandler
    {
        /// <inheritdoc cref="IsAFailingCheckWithMessage"/>
        [Obsolete("Use IsAFailingCheckWithMessage instead (typo)")]
        public static ICheckLink<ICodeCheck<RunTrace>> IsAFaillingCheckWithMessage(this ICodeCheck<RunTrace> check,
            params string[] lines)
        {
            return check.IsAFailingCheckWithMessage(lines);
        }

        /// <summary>
        ///     Verify that the code results in a failed NFluent check with a specified message.
        /// </summary>
        /// <param name="check"></param>
        /// <param name="lines"></param>
        /// <returns>A link check</returns>
        public static ICheckLink<ICodeCheck<RunTrace>> IsAFailingCheckWithMessage(this ICodeCheck<RunTrace> check,
            params string[] lines)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("fluent check")
                .CheckSutAttributes((sut) => sut.RaisedException, "raised exception")
                .FailIfNull("The check succeeded whereas it should have failed.")
                .FailWhen((sut) => !ExceptionHelper.IsFailedException(sut),
                    $"The exception raised is not of the expected type.")
                .CheckSutAttributes((sut) => sut.Message.SplitAsLines(), "error message")
                .Analyze((messageLines, test) =>
                {
                    var expectedLines = (lines.Length == 1) ? lines[0].SplitAsLines() : lines;
                    for (var i = 0; i < expectedLines.Count; i++)
                    {
                        if (expectedLines[i] == "*")
                        {
                            //any line
                            continue;
                        }

                        if (messageLines.Count <= i)
                        {
                            test.Fail($"Lines are missing in the error message starting at #{i}");
                            break;
                        }

                        if (expectedLines[i].StartsWith("#"))
                        {
                            if (!Regex.IsMatch(messageLines[i], expectedLines[i].Substring(1)))
                            {
                                test.Fail($"Line {i} is different from what is expected"+Environment.NewLine+
                                          "Act:"+messageLines[i].DoubleCurlyBraces()+Environment.NewLine+
                                          "Exp (regex):"+expectedLines[i].DoubleCurlyBraces()
                                );
                                break;
                            }
                        }
                        else if (messageLines[i] != expectedLines[i])
                        {
                            test.Fail($"Line {i} is different from what is expected"+Environment.NewLine+
                                       "Act:"+messageLines[i].DoubleCurlyBraces()+Environment.NewLine+
                                        "Exp:"+expectedLines[i].DoubleCurlyBraces() );
                            break;
                        }
                    }

                    if (messageLines.Count > expectedLines.Count)
                    {
                        test.Fail($"Too many lines in the error message starting at #{expectedLines.Count}");
                    }
                }).
                DefineExpectedValue(lines).
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Verify that the code results in a failed NFluent check with a specified message.
        /// </summary>
        /// <param name="check"></param>
        /// <returns>A link check</returns>
        public static ICheckLink<ICodeCheck<RunTrace>> IsAFaillingCheck(this ICodeCheck<RunTrace> check)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("fluent check")
                .CheckSutAttributes((sut) => sut.RaisedException, "raised exception")
                .FailIfNull("The fluent check did not raise an exception, where as it must.")
                .FailWhen((sut) => !ExceptionHelper.IsFailedException(sut),
                    "The exception raised is not of the expected type").
                DefineExpectedType(ExceptionHelper.BuildException(string.Empty).GetType()).
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }
    }
}