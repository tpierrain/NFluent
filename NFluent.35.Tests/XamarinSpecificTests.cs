﻿// // --------------------------------------------------------------------------------------------------------------------
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

namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class XamarinSpecificTests
    {
        [Test]
        public void MonoNamingConventionTests()
        {
            ObjectCheckExtensions.FieldKind result;
            var name = ObjectCheckExtensions.ExtractFieldNameAsInSourceCode("<autofield>", out result);
            Check.That(name).IsEqualTo("autofield");

            Check.That(result).IsEqualTo(ObjectCheckExtensions.FieldKind.AnonymousClass);
        }
    }
}
