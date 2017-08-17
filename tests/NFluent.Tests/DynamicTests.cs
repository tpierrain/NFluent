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


#if PORTABLE || NETSTANDARD1_3 || DOTNET_45
namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class DynamicTests
    {
        /*
    [Test]
    public void CanCheckThatOnADynamic()
    {
        dynamic car = "car";
            Check.ThatCode(() =>
            {
                Check.That(car.FuelLevel).IsEmpty();
            })
            .ThrowsAny();
        } */
        class Command
        {
            internal dynamic Subject { get; set; }
        }

        [Test]
        public void CanCheckNulls()
        {
            var cmd = new Command();
            dynamic sut = "test";

            Check.ThatDynamic(sut).IsNotNull();
            // this check fails
            Check.ThatCode(() => { Check.ThatDynamic(cmd.Subject).IsNotNull(); }).Throws<FluentCheckException>();
        }

        [Test]
        public void CanCheckReference()
        {
            dynamic sut = "test";

            // this check fails
            Check.ThatDynamic(sut).IsSameReferenceAs(sut);
            Check.ThatCode(() => { Check.ThatDynamic(sut).IsSameReferenceAs("tes"); }).Throws<FluentCheckException>();
        }
    }
}
#endif