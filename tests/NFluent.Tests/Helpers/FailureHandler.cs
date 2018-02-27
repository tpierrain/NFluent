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

namespace NFluent.Tests.Helpers
{
    using System;
    using Extensibility;
    using NFluent.Helpers;

    internal static class FailureHandler
    {
        /// <summary>
        ///     Verify that the code results in a failed NFluent check with a specified message.
        /// </summary>
        /// <param name="check"></param>
        /// <param name="lines"></param>
        /// <returns>A link check</returns>
        public static ICheckLink<ICodeCheck<RunTrace>> FailsWithMessage(this ICodeCheck<RunTrace> check,
            params string[] lines)
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
            return checker.ExecuteCheck(() =>
            {
                var trace = checker.Value;
                if (trace.RaisedException == null)
                {
                    throw new FluentCheckException("The check succeed, whereas it should have failed.");
                }

                if (!ExceptionHelper.IsFailedException(trace.RaisedException))
                {
                    var message = checker.BuildMessage("The exception raised is not of the expected type")
                        .On(trace.RaisedException).And.Expected(typeof(FluentCheckException));
                    throw new FluentCheckException(message.ToString());
                }

                var raisedExceptionMessage = trace.RaisedException.Message;
                if (lines.Length>1)
                {
                    Check.That(raisedExceptionMessage).AsLines().ContainsExactly(lines);
                }
                else
                {
                    Check.That(raisedExceptionMessage).IsEqualTo(lines[0]);
                }
            }, "This check should have failed.");
        }
    }
}