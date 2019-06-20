// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ReverseEngineeringExceptionMessagesHelper.cs" company="">
//   Copyright 2016 Thomas Pierrain
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once EmptyNamespace
// ReSharper disable once CheckNamespace
namespace NFluent.Extensibility
{
    using System.Text;
    /// <summary>
    /// Helper class that helps to retrieve well formated strings exception messages from a failing NFluent Check execution.
    /// 1. You provide the lambda containing the failing check. You run it (from within a test for instance)
    /// 2. You copy the content of the file generated  (with path provided as argument, or default value: ).
    /// 3. You paste the exception message to your nunit ExpectedMessage value.
    /// </summary>
    public static class ReverseEngineeringExceptionMessagesHelper
    {
#if SCANEXCEPTION
        private const string DefaultFilePath = @"C:\\Temp\\NFluentTroubleShoot.txt";
        /// <summary>
        /// Generate a file containing the ready-to-be-copied-and-pasted-in-a-test exception message that occured while executing the provided lambda.
        /// </summary>
        /// <param name="lambda">A lambda that generates a FluentException.</param>
        public static void DumpReadyToCopyAndPasteExceptionMessageInAFile(Action lambda)
        {
            DumpReadyToCopyAndPasteExceptionMessageInAFile(lambda, DefaultFilePath);
        }

        /// <summary>
        /// Generate a file containing the ready-to-be-copied-and-pasted-in-a-test exception message that occured while executing the provided lambda.
        /// </summary>
        /// <param name="lambda">A lambda that generates a FluentException.</param>
        /// <param name="dumpedMessageFilePath">The path of the file to be generated with the (ready to be copied and pasted) exception message as content.</param>
        public static void DumpReadyToCopyAndPasteExceptionMessageInAFile(Action lambda, string dumpedMessageFilePath)
        {
            try
            {
                lambda();
            }
            catch (FluentCheckException fce)
            {
                DumpProperlyEscapedMessage(dumpedMessageFilePath, fce.Message);
            }
        }

        private static void DumpProperlyEscapedMessage(string outputfilePath, string message)
        {
            var properlyEscapedMessage = GetProperlyEscapedMessage(message);
            File.WriteAllText(outputfilePath, properlyEscapedMessage);
        }
#endif
        /// <summary>
        /// Build a ready-to-be-copied-and-pasted-in-a-string message (it will "escape" tabs, CRLF, and quote characters).
        /// </summary>
        /// <param name="input">The input message.</param>
        /// <returns>The message "escaped" and ready to be copied-and-pasted in a .NET string.</returns>
        public static string GetProperlyEscapedMessage(string input)
        {
            var result = new StringBuilder(input.Length);
            foreach (var character in input)
            {
                switch (character)
                {
                    case '\t':
                        result.Append("\\t");
                        break;

                    case '\"':
                        result.Append("\\\"");
                        break;

                    case '\r':
                        result.Append("\\r");
                        break;

                    case '\n':
                        result.Append("\\n");
                        break;

                    default:
                        result.Append(character);
                        break;
                }
            }

            return result.ToString();
        }
    }
}