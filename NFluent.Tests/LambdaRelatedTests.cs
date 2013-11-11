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

        [Test]
        public void NoExceptionRaised()
        {
            Check.That((Action) (() => new object())).DoesNotThrow();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.StartsWith, ExpectedMessage = "\nThe checked code raised an exception, whereas it must not.")]
        public void UnexpectedExceptionRaised()
        {
            Check.That(() => { throw new ApplicationException(); }).DoesNotThrow();
        }

        [Test]
        public void ExpectedExceptionRaised()
        {
            Check.That(() => { throw new InvalidOperationException(); }).Throws<InvalidOperationException>();
            Check.That(() => { throw new ApplicationException(); }).ThrowsAny();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.Contains, ExpectedMessage = "\nThe checked code raised an exception of a different type than expected.")]
        public void DidNotRaiseExpected()
        {
            Check.That(() => { throw new Exception(); }).Throws<ApplicationException>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked code did not raise an exception, whereas it must.")]
        public void DidNotRaiseAny()
        {
            Check.That(() => { new object(); }).ThrowsAny();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked code did not raise an exception, whereas it must.\nExpected exception type is:\n\t[System.Exception]")]
        public void DidNotRaiseAnyTypedCheck()
        {
            Check.That(() => { new object(); }).Throws<Exception>();
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
            Check.That(() => 1).DoesNotThrow();
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
            Check.That(() => { throw new Exception(); }).ThrowsAny();
        }

        [Test]
        public void DidNotThrowWithNewObjectHavingACorrectCtor()
        {
            Check.That(()=>new Object()).DoesNotThrow();
        }
        
        [Test]
        public void CanRaiseWithNewObjectHavingAFailingCtor()
        {
            Check.That(()=>new AnObjectThatCanCrashOnCtor(0)).Throws<DivideByZeroException>();
        }

        

        [Test]
        public void CanRaiseWithFailingPropertyGetter()
        {
            var sut = new AnObjectThatCanCrashWithPropertyGet(0);
            Check.That(()=>sut.BeastBreaker).Throws<DivideByZeroException>();
        }



        [Test]
        public void CanCheckForAMessageOnExceptionRaised()
        {
            Check.That(() => { throw new LambdaExceptionForTest(123, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithMessage("Err #123 : my error message")
                .And.WithProperty("ExceptionNumber", 123);
        }


        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe message is not the expected one\nThe given value:\n\t[\"a buggy message\"]\nThe expected value(s):\n\t[\"Err #321 : my error message\"]")]
        public void DidNotRaiseTheExpectedMessage()
        {
            Check.That(() => { throw new LambdaExceptionForTest(321, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithMessage("a buggy message");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.Contains, ExpectedMessage = "\nThe property [inexistingProperty] does not exist on exception of type LambdaExceptionForTest")]
        public void DidNotHaveExpectedPropertyName()
        {
            Check.That(() => { throw new LambdaExceptionForTest(321, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithProperty("inexistingProperty", 123);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.Contains, ExpectedMessage = "\nThe property [ExceptionNumber] do not have the expected value\nThe given value:\n\t[321]\nThe expected value:\n\t[123]")]
        public void DidNotHaveExpectedPropertyValue()
        {
            Check.That(() => { throw new LambdaExceptionForTest(321, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithProperty("ExceptionNumber", 123);
        }


        #region Lambda related Test Data

        public class LambdaExceptionForTest : Exception
        {
            public int ExceptionNumber { get; private set; }
            public LambdaExceptionForTest(int exeptionNumber, string message) : base(formatMessage(exeptionNumber,message))
            {
                ExceptionNumber = exeptionNumber;
            }

            private static string formatMessage(int exceptionNumber, string message)
            {
                return string.Format("Err #{0} : {1}", exceptionNumber, message);
            }
        }
        private class AnObjectThatCanCrashOnCtor
        {
            public AnObjectThatCanCrashOnCtor(int i)
            {
                var j = 1/i;
            }
        }

        private class AnObjectThatCanCrashWithPropertyGet
        {
            private int devilMathValue;
            public AnObjectThatCanCrashWithPropertyGet(int i)
            {
                devilMathValue = i;
            }

            public int BeastBreaker
            {
                get
                {
                    return 666/devilMathValue;
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