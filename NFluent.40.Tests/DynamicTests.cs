// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DynamicTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
    using Microsoft.CSharp.RuntimeBinder;

    using NUnit.Framework;

    [TestFixture]
    public class DynamicTests
    {
        // Dynamics are not compliant with .NET extension method which is the way we introduce checks within NFluent

        [Test]
        [ExpectedException(typeof(RuntimeBinderException), ExpectedMessage = "'string' does not contain a definition for 'FuelLevel'")]
        public void CanCheckThatOnADynamic()
        {
            dynamic car = "car";
            Check.That(car.FuelLevel).IsEmpty();
        }

        [Test]
        [ExpectedException(typeof(RuntimeBinderException), ExpectedMessage = "'NFluent.LambdaCheck' does not contain a definition for 'IsNotNull'")]
        public void DynamicSutIsConsideredAsALambda()
        {
            var cmd = new Command();
            
            Check.That(cmd.Subject).IsNotNull();
        }

        [Test]
        [ExpectedException(typeof(RuntimeBinderException), ExpectedMessage = "'NFluent.FluentCheck<string>' does not contain a definition for 'IsNotNull'")]
        public void DynamicPropertiesAreCheckableWithoutThrowing()
        {
            var cmd = new Command();
            cmd.Subject = "test";

            Check.That(cmd.Subject).IsNotNull();
        }

        public class Command
        {
            public dynamic Subject { get; set; }
        }
    }
}
