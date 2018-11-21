﻿// --------------------------------------------------------------------------------------------------------------------
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
        private readonly StringWriter newOut;
        private readonly TextReader newIn;
        private readonly Stream inputStream;
        private readonly StreamWriter inputSimulator;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public CaptureConsole()
        {
            this.inputStream = new MemoryStream();
            this.inputSimulator = new StreamWriter(this.inputStream);
            this.newOut = new StringWriter();
            this.newIn = new StreamReader(this.inputStream);
            this.oldOut = Console.Out;
            this.oldIn = Console.In;
            Console.SetOut(this.newOut);
            Console.SetIn(this.newIn);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Console.SetOut(this.oldOut);
            Console.SetIn(this.oldIn);
            this.inputStream.Dispose();
            this.newIn.Dispose();
        }

        /// <summary>
        /// Gets the text output to the console as a single string.
        /// </summary>
        public string Output => this.newOut.ToString();

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
            this.inputStream.Seek(pos, SeekOrigin.Begin);
        }
    }
}