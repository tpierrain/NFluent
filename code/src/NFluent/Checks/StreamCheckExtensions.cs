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
namespace NFluent
{
    using System.IO;
    using Extensibility;

    /// <summary>
    /// Provides check methods to be executed on a stream instance.
    /// </summary>
    public static class StreamCheckExtensions
    {
        /// <summary>
        /// Checks that the actual stream has the same content as another one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The stream to compare content with.</param>
        /// <returns>A check link</returns>
        public static ICheckLink<ICheck<Stream>> HasSameSequenceOfBytesAs(this ICheck<Stream> check, Stream expected)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailIfNull()
                .SetSutName("stream")
                .FailWhen(sut => sut.Length != expected.Length, 
                    "The {0} doesn't have the same content as the expected one. They don't even have the same Length!")
                .DefineExpectedValue(expected)
                .Analyze((sut, test) =>
                {
                    // Keeps initial positions to be able to restore them after the check
                    var valueInitialPosition = sut.Position;
                    var otherInitialPosition = expected.Position;

                    sut.Seek(0, SeekOrigin.Begin);
                    expected.Seek(0, SeekOrigin.Begin);
                    while (sut.Position < sut.Length)
                    {
                        if (sut.ReadByte() == expected.ReadByte())
                        {
                            continue;
                        }

                        test.Fail(
                            $"The {{0}} doesn't have the same content as the expected one (despite the fact that they have the same Length: {sut.Length}).");
                        break;
                    }

                    // Side-effect free. Restore initial positions of streams
                    sut.Position = valueInitialPosition;
                    expected.Position = otherInitialPosition;
                })
                .OnNegate("The {0} has the same content as the other one, whereas it must not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }
    }
}