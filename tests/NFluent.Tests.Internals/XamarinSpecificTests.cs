// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="XamarinSpecificTests.cs" company="">
// //   Copyright 2014 Cyrille DUPUYDAUBY
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

// ReSharper disable once CheckNamespace
namespace NFluent.Tests.Internals
{
    using System.Reflection;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class XamarinSpecificTests
    {
        [Test]
        public void MonoNamingConventionTests()
        {
            // check that mono naming strategy is recognized
            var name = ReflectionWrapper.BuildFromField(string.Empty, "<autofield>", typeof(int), 2, new Criteria(BindingFlags.Public));
            Check.That(name.MemberLongName).IsEqualTo("autofield");
        }
    }
}
