// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaptureConsole.cs" company="">
//   Copyright 2018 Cyrille DUPUYDAUBY
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
namespace NFluent.Mocks
{
    using System;
    using System.IO;

    /// <inheritdoc />
    /// <summary>
    /// Class that mocks the console. One can generate simulated inputs at the beginning of a test and/or
    /// along the test code.
    /// </summary>
    public sealed class CaptureConsole : IDisposable
    {
        private readonly TextWriter oldOut;
        private readonly TextReader oldIn;

        private readonly TextWriter newOut;
        private readonly Stream outputStream;

        private readonly TextReader newIn;
        private readonly Stream inputStream;
        private readonly StreamWriter inputSimulator;
        private readonly StreamReader outputCapture;
        private long readCursor;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public CaptureConsole()
        {
            this.inputStream = new MemoryStream();
            this.inputSimulator = new StreamWriter(this.inputStream);
            this.outputStream = new MemoryStream();
            this.outputCapture = new StreamReader(this.outputStream);
            this.newOut = new StreamWriter(this.outputStream);
            this.newIn = new StreamReader(this.inputStream);
            this.oldOut = Console.Out;
            this.oldIn = Console.In;
            Console.SetOut(this.newOut);
            Console.SetIn(this.newIn);
        }

        /// <inheritdoc />
        // Stryker disable once Statement: Can't cover dispose mutations
        public void Dispose()
        {
            Console.SetOut(this.oldOut);
            Console.SetIn(this.oldIn);
            this.outputStream.Dispose();
            this.outputCapture.Dispose();
            this.inputStream.Dispose();
            this.newIn.Dispose();
        }

        /// <summary>
        /// Gets the text output to the console as a single string.
        /// </summary>
        public string Output
        {
            get
            {
                this.newOut.Flush();
                var cursor = this.outputStream.Position;
                this.outputStream.Position = 0;
                var text = this.outputCapture.ReadToEnd();
                this.outputStream.Position = cursor;
                return text;
            }
        }

        /// <summary>
        /// Simulates a textual input.
        /// </summary>
        /// <param name="input">Text to use as a simulated input</param>
        /// <remarks>Automatically adds an end of line marker at the end of the input.</remarks>
        public void InputLine(string input)
        {
            this.Inject(stream => stream.WriteLine(input));
        }

        /// <summary>
        /// Simulates a textual input.
        /// </summary>
        /// <param name="input">Text to use as a simulated input</param>
        public void Input(string input)
        {
            this.Inject(stream => stream.Write(input));
        }

        private void Inject(Action<StreamWriter> writer)
        {
            var pos = this.inputStream.Position;
            this.inputStream.Seek(0, SeekOrigin.End);
            writer(this.inputSimulator);
            this.inputSimulator.Flush();
            this.inputStream.Position = pos;
        }

        /// <summary>
        /// Gets a single line from the output stream.
        /// </summary>
        /// <returns>A single line from the output stream.</returns>
        public string ReadLine()
        {
            this.newOut.Flush();
            var pos = this.outputStream.Position;
            this.outputStream.Position = this.readCursor;
            var text = this.outputCapture.ReadLine();
            this.readCursor = this.outputStream.Position;
            this.outputStream.Position = pos;
            return text;
        }
    }
}