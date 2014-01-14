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
    using System;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class LambdaRelatedTests
    {
        private readonly ExceptionTests exceptionTests = new ExceptionTests();

        [Test]
        public void DurationTest()
        {
            Check.ThatCode(() => Thread.Sleep(30)).LastsLessThan(60, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.StartsWith, ExpectedMessage = "\nThe checked code took too much time to execute.\n")]
        public void FailDurationTest()
        {
            Check.ThatCode(() => Thread.Sleep(0)).LastsLessThan(0, TimeUnit.Milliseconds);
        }

        [Test]
        public void NoExceptionRaised()
        {
            Check.That((Action)(() => new object())).DoesNotThrow();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.StartsWith, ExpectedMessage = "\nThe checked code raised an exception, whereas it must not.")]
        public void UnexpectedExceptionRaised()
        {
            Check.ThatCode(() => { throw new ApplicationException(); }).DoesNotThrow();
        }

        [Test]
        public void ExpectedExceptionRaised()
        {
            Check.ThatCode(() => { throw new InvalidOperationException(); }).Throws<InvalidOperationException>();
            Check.ThatCode(() => { throw new ApplicationException(); }).ThrowsAny();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.Contains, ExpectedMessage = "\nThe checked code raised an exception of a different type than expected.")]
        public void DidNotRaiseExpected()
        {
            Check.ThatCode(() => { throw new Exception(); }).Throws<ApplicationException>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked code did not raise an exception, whereas it must.")]
        public void DidNotRaiseAny()
        {
            Check.ThatCode(() => { new object(); }).ThrowsAny();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked code did not raise an exception, whereas it must.\nExpected exception type is:\n\t[System.Exception]")]
        public void DidNotRaiseAnyTypedCheck()
        {
            Check.ThatCode(() => { new object(); }).Throws<Exception>();
        }

        [Test]
        public void DidNotRaiseWhenUsedWithValidParameterlessFuncVariable()
        {
            Func<bool> sut = () => true;
            Check.That(sut).DoesNotThrow();
        }

        [Test]
        public void CanRaiseWhenUsedWithParameterlessFuncVariable()
        {
            Func<bool> sut = () => { throw new Exception(); };

            Check.That(sut).ThrowsAny();
        }

        [Test]
        public void DidNotRaiseWhenUsedWithValidAnonymousFuncExpression()
        {
            Check.ThatCode(() => 1).DoesNotThrow();
        }

        [Test]
        public void DidNotRaiseWhenUsedWithValidDelegateExpression()
        {
            Check.That(delegate { var obj = new object(); }).DoesNotThrow();
        }

        [Test]
        public void DidNotRaiseWhenUsedWithAValidParameterlessVoidMethod()
        {
            var sut = new AnObjectWithParameterLessMethodThatCanBeInvokedLikeLambdas();
            Check.That(sut.AVoidParameterLessMethodThatShouldNotCrash).DoesNotThrow();
        }

        [Test]
        public void DidNotRaiseWhenUsedWithAValidParameterlessMethodReturningObject()
        {
            var obj = new AnObjectWithParameterLessMethodThatCanBeInvokedLikeLambdas();
            Func<object> sut = obj.AScalarParameterLessMethodThatShouldNotCrash;
            Check.That(sut).DoesNotThrow();
        }

        [Test]
        public void CanRaiseWhenUsedWithAnonymousActionExpression()
        {
            Check.ThatCode(() => { throw new Exception(); }).ThrowsAny();
        }

        [Test]
        public void DidNotThrowWithNewObjectHavingACorrectCtor()
        {
            Check.ThatCode(() => new object()).DoesNotThrow();
        }
        
        [Test]
        public void CanRaiseWithNewObjectHavingAFailingCtor()
        {
            Check.ThatCode(() => new AnObjectThatCanCrashOnCtor(0)).Throws<DivideByZeroException>();
        }

        [Test]
        public void CanRaiseWithFailingPropertyGetter()
        {
            var sut = new AnObjectThatCanCrashWithPropertyGet(0);
            Check.ThatCode(() => sut.BeastBreaker).Throws<DivideByZeroException>();
        }

        [Test]
        public void CanCheckForAMessageOnExceptionRaised()
        {
            Check.ThatCode(() => { throw new LambdaExceptionForTest(123, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithMessage("Err #123 : my error message")
                .And.WithProperty("ExceptionNumber", 123);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe message of the checked exception is not as expected.\nThe given exception message:\n\t[\"a buggy message\"]\nThe expected value(s):\n\t[\"Err #321 : my error message\"]")]
        public void DidNotRaiseTheExpectedMessage()
        {
            Check.ThatCode(() => { throw new LambdaExceptionForTest(321, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithMessage("a buggy message");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.Contains, ExpectedMessage = "\nThere is no property [inexistingProperty] on exception type [LambdaExceptionForTest].")]
        public void DidNotHaveExpectedPropertyName()
        {
            Check.ThatCode(() => { throw new LambdaExceptionForTest(321, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithProperty("inexistingProperty", 123);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.Contains, ExpectedMessage = "\nThe property [ExceptionNumber] of the checked exception do not have the expected value.\nThe given exception:\n\t[321]\nThe expected exception:\n\t[123]")]
        public void DidNotHaveExpectedPropertyValue()
        {
            Check.ThatCode(() => { throw new LambdaExceptionForTest(321, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithProperty("ExceptionNumber", 123);
        }

        [Test]
        public void CheckReturnValue()
        {
            Check.ThatCode(() => 4).DoesNotThrow().And.WhichResult().IsEqualTo(4);
        }
        #region Lambda related Test Data

        public class LambdaExceptionForTest : Exception
        {
            public LambdaExceptionForTest(int exeptionNumber, string message) : base(FormatMessage(exeptionNumber, message))
            {
                this.ExceptionNumber = exeptionNumber;
            }

            public int ExceptionNumber { get; private set; }

            private static string FormatMessage(int exceptionNumber, string message)
            {
                return string.Format("Err #{0} : {1}", exceptionNumber, message);
            }
        }

        private class AnObjectThatCanCrashOnCtor
        {
            public AnObjectThatCanCrashOnCtor(int i)
            {
                var j = 1 / i;
            }
        }

        private class AnObjectThatCanCrashWithPropertyGet
        {
            private int devilMathValue;

            public AnObjectThatCanCrashWithPropertyGet(int i)
            {
                this.devilMathValue = i;
            }

            public int BeastBreaker
            {
                get
                {
                    return 666 / this.devilMathValue;
                }
            }
        }

        private class AnObjectWithParameterLessMethodThatCanBeInvokedLikeLambdas
        {
            public void AVoidParameterLessMethodThatCrashes()
            {
                throw new LambdaExceptionForTest(666, "test");
            }

            public void AVoidParameterLessMethodThatShouldNotCrash()
            {
                new object();
            }

            public void AScalarParameterLessMethodThatCrashes()
            {
                throw new LambdaExceptionForTest(666, "test");
            }

            public object AScalarParameterLessMethodThatShouldNotCrash()
            {
                return new object();
            }
        }
        #endregion
    }
}