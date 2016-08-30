// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StreamCheckExtensions.cs" company="">
// //   Copyright 2016 Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
namespace NFluent.Assertions
{
    using System.IO;
    using NFluent.Extensibility;

    /// <summary>
    /// Provides check methods to be executed on a stream instance.
    /// </summary>
    public static class StreamCheckExtensions
    {
        #region HasSameContentAs

        /// <summary>
        /// Checks that the actual stream has the same content as another one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The stream to compare content with.</param>
        /// <returns>A check link</returns>
        public static ICheckLink<ICheck<Stream>> HasSameContentAs(this ICheck<Stream> check, Stream expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var value = checker.Value;

            return checker.ExecuteCheck(
                () =>
                    {
                        if (value.Length != expected.Length)
                        {
                            ThrowTheyDontEvenHaveTheSameLength(expected, checker, value);
                        }

                        // Keeps initial positions to be able to restore them after the check
                        var valueInitialPosition = value.Position;
                        var otherInitialPosition = expected.Position;

                        value.Seek(0, SeekOrigin.Begin);
                        expected.Seek(0, SeekOrigin.Begin);
                        while (value.Position < value.Length)
                        {
                            if (value.ReadByte() != expected.ReadByte())
                            {
                                ThrowTheyDontHaveTheSameContentButHaveTheSameLength(expected, checker, value);
                            }
                        }

                        // Side-effect free. Restore initial positions of streams
                        value.Position = valueInitialPosition;
                        expected.Position = otherInitialPosition;
                    },
                BuildNegatedMessage(expected, value).ToString());
        }

        private static MessageBlock BuildNegatedMessage(Stream expected, Stream value)
        {
            // TODO: find out how can I rely on {0} to remove hard-coded "The checked Stream"
            // BUG: Specifying a Label prevents Comparison to work.
            var negatedMessage =
                FluentMessage.BuildMessage("The checked Stream has the same content as the other one, whereas it must not.")
                    .On(value)
                    .Comparison($"(Length: {value.Length})")
                    .And.Expected(expected)
                    .Label("The other one:")
                    .Comparison($"(Length: {expected.Length})");
            return negatedMessage;
        }

        private static void ThrowTheyDontHaveTheSameContentButHaveTheSameLength(Stream expected, IChecker<Stream, ICheck<Stream>> checker, Stream value)
        {
            var message =
                checker.BuildMessage(
                    "The {0} doesn't have the same content as the expected one (despite the fact that they have the same Length: " + value.Length + ").")
                    .On(value)
                    .And.Expected(expected);

            throw new FluentCheckException(message.ToString());
        }

        private static void ThrowTheyDontEvenHaveTheSameLength(Stream expected, IChecker<Stream, ICheck<Stream>> checker, Stream value)
        {
            var message =
                checker.BuildMessage("The {0} doesn't have the same content as the expected one. They don't even have the same Length!")
                    .On(value)
                    .Comparison($"(Length: {value.Length})")
                    .And.Expected(expected)
                    .Comparison($"(Length: {expected.Length})");

            throw new FluentCheckException(message.ToString());
        }

        #endregion

    }
}