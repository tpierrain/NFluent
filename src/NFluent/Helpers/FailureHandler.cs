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
        public static ICheckLink<ICodeCheck<RunTrace>> IsAFaillingCheckWithMessage(this ICodeCheck<RunTrace> check,
            params string[] lines)
        {
            var test = ExtensibilityHelper.BeginCheck(check)
                .GetSutProperty((sut) => sut.RaisedException, "raised exception").SutNameIs("fluent check")
                .FailsIfNull()
                .FailsIf((sut) => !ExceptionHelper.IsFailedException(sut),
                    "The exception raised is not of the expected type")
                .GetSutProperty((sut) => sut.Message, "error message").SutNameIs("fluent check");
                test.Analyze((message) =>
                {
                    var expectedLines = (lines.Length == 1) ? lines[0].SplitAsLines() : lines;
                    var messageLines = message.SplitAsLines();
                    for (var i = 0; i < expectedLines.Count; i++)
                    {
                        if (expectedLines[i] == "*")
                        {
                            continue;
                        }

                        if (messageLines.Count <= i)
                        {
                            test.Fails($"Lines are missing in the error message starting at #{i}");
                            break;
                        }
                        Check.That(messageLines[i]).As($"line #{i}").IsEqualTo(expectedLines[i]);
                    }

                    if (messageLines.Count > expectedLines.Count)
                    {
                        test.Fails($"Too many lines in the error message starting at #{expectedLines.Count-1}");
                    }
                }).Expecting(lines).
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }
    }
}