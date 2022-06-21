// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ConsoleCaptureShould.cs" company="NFluent">
//   Copyright 2022 Cyrille DUPUYDAUBY
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

namespace NFluent.Tests
{
    using System;
    using Mocks;
    using NUnit.Framework;

    [TestFixture]
    internal class ConsoleCaptureShould
    {
        [Test]
        public void CaptureOutput()
        {
            using (var session = new CaptureConsole())
            {
                Console.Write("hello");
                Check.That(session.Output).IsEqualTo("hello");
            }
        }

        [Test]
        public void SimulateInput()
        {
            using (var session = new CaptureConsole())
            {
                session.InputLine("hello");
                Check.That(Console.ReadLine()).IsEqualTo("hello");
                session.Input("AB");
                Check.That((char)Console.Read()).IsEqualTo('A');
            }
        }

        [Test]
        public void PermitStreamedOutputConsumption()
        {
            using (var session = new CaptureConsole())
            {
                Console.WriteLine("hello");
                Console.WriteLine("world");
                Check.That(session.ReadLine()).IsEqualTo("hello");
                Check.That(session.ReadLine()).IsEqualTo("world");
                Console.Write("so ");
                Console.Write("great");
                Check.That(session.ReadLine()).IsEqualTo("so great");
            }
        }
    }
}