// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StreamTests.cs" company="">
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
using System;

namespace NFluent.Tests
{
    using System.IO;
    using System.Text;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class CheckOnStreamShould
    {

        [Test]
        public void HasSameSequenceOfBytesAs_works()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    writer.Write("What else!");
                    writer.Flush();

                    using (var secondStream = new MemoryStream())
                    {
                        memoryStream.WriteTo(secondStream);

                        Check.That(memoryStream.Length).IsEqualTo(secondStream.Length);
                        Check.That(memoryStream).HasSameSequenceOfBytesAs(secondStream);
                    }
                }
            }
        }

        [Test]
        public void Not_HasSameSequenceOfBytesAs_throws_with_same_content()
        {
            Check.ThatCode(() =>
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                    {
                        writer.Write("What else!");
                        writer.Flush();

                        using (var secondStream = new MemoryStream())
                        {
                            memoryStream.WriteTo(secondStream);

                            Check.That(memoryStream).Not.HasSameSequenceOfBytesAs(secondStream);
                        }
                    }
                }
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked stream has the same content as the other one, whereas it must not.",
                    "The checked stream:",
                    "\t[System.IO.MemoryStream (Length: 13)]",
                    "The expected stream: different from",
                    "\t[System.IO.MemoryStream (Length: 13)]");
            
        }

        [Test]
        public void HasSameSequenceOfBytesAs_doesnt_produce_side_effects()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    writer.Write("What else!");
                    writer.Flush();

                    using (var otherStream = new MemoryStream())
                    {
                        memoryStream.WriteTo(otherStream);

                        // Change stream Position to setup a state
                        memoryStream.Position = 2;
                        otherStream.Position = 3;
                        Check.That(memoryStream.Position).IsEqualTo(2);
                        Check.That(otherStream.Position).IsEqualTo(3);

                        // Compares
                        Check.That(memoryStream).HasSameSequenceOfBytesAs(otherStream);

                        // Ensures our comparison is side-effect free
                        Check.That(memoryStream.Position).IsEqualTo(2);
                        Check.That(otherStream.Position).IsEqualTo(3);
                    }
                }
            }
        }

        [Test]
        public void HasSameSequenceOfBytesAs_throws_exception_with_different_content_with_different_size()
        {
            Check.ThatCode(() =>
            {
                using (var memoryStream = new MemoryStream())
                using (var otherStream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                    using (var otherWriter = new StreamWriter(otherStream, Encoding.UTF8))
                    {
                        writer.Write("Spinoza FTW ;-)");
                        writer.Flush();

                        otherWriter.Write("Kant ;-(");
                        otherWriter.Flush();

                        Check.That(memoryStream).HasSameSequenceOfBytesAs(otherStream);
                    }
                }
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked stream doesn't have the same content as the expected one. They don't even have the same Length!",
                    "The checked stream:",
                    "\t[System.IO.MemoryStream (Length: 18)]",
                    "The expected stream:",
                    "\t[System.IO.MemoryStream (Length: 11)]");
        }

        [Test]
        public void HasSameSequenceOfBytesAs_throws_exception_with_different_content_but_same_size()
        {
            Check.ThatCode(() =>
            {
                using (var memoryStream = new MemoryStream())
                using (var otherStream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                    using (var otherWriter = new StreamWriter(otherStream, Encoding.UTF8))
                    {
                        writer.Write("123456789");
                        writer.Flush();

                        otherWriter.Write("981234567");
                        otherWriter.Flush();

                        Check.That(memoryStream).HasSameSequenceOfBytesAs(otherStream);
                    }
                }
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked stream doesn't have the same content as the expected one (despite the fact that they have the same Length: 12).",
                    "The checked stream:",
                    "\t[System.IO.MemoryStream (Length: 12)]",
                    "The expected stream:",
                    "\t[System.IO.MemoryStream (Length: 12)]");
        }

        [Test]
        public void Not_HasSameSequenceOfBytesAs_works()
        {
            using (var memoryStream = new MemoryStream())
            using (var otherStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var otherWriter = new StreamWriter(otherStream, Encoding.UTF8))
                {
                    writer.Write("Spinoza FTW ;-)");
                    writer.Flush();

                    otherWriter.Write("Kant ;-(");
                    otherWriter.Flush();

                    Check.That(memoryStream).Not.HasSameSequenceOfBytesAs(otherStream);
                }
            }
        }
    }
}