// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="LambdaRelatedTests.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
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
namespace NFluent.Tests
{
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class LambdaRelatedTests
    {
        private readonly ExceptionTests exceptionTests = new ExceptionTests();

        [Test]
        public void DurationTest()
        {
            Check.That(() => Thread.Sleep(30)).LastsLessThan(60, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.StartsWith, ExpectedMessage = "\nThe checked code took too much time to execute.\n")]
        public void FailDurationTest()
        {
            Check.That(() => Thread.Sleep(0)).LastsLessThan(0, TimeUnit.Milliseconds);
        }
    }
}