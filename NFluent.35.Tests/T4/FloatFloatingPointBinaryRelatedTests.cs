// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FloatFloatingPointBinaryRelatedTests.cs" company="">
// //   Copyright 2014 Thomas PIERRAIN
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
    public class FloatFloatingPointBinaryRelatedTests
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------
        
        #region IsNaN

        [Test]
        public void IsNaNWorks()
        {
            const double Zero = 0D;
            const double NotANumber = Zero / Zero;

            Check.That(NotANumber).IsNaN();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked double value is a number whereas it must not.\nThe checked double value:\n\t[20]")]
        public void IsNaNThrowsWhenTheValueIsANumber()
        {
            const double Twenty = 20D;

            Check.That(Twenty).IsNaN();
        }

        [Test]
        public void NotIsNaNWorks()
        {
            const double Twenty = 20D;

            Check.That(Twenty).Not.IsNaN();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked double value is not a number (NaN) whereas it must.\nThe checked double value:\n\t[NaN]")]
        public void NotIsNaNThrowsAnExceptionWhenFailing()
        {
            const double Zero = 0D;
            const double NotANumber = Zero / Zero;

            Check.That(NotANumber).Not.IsNaN();
        }

        #endregion
    }
}
