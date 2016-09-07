// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="LambdaRelatedTests.cs" company="">
// //   Copyright 2016 Cyrille DUPUYDAUBY & Thomas PIERRAIN
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

using System.Globalization;
using System.Threading;
using NFluent.ApiChecks;

namespace NFluent.Tests
{
    using System;
    using System.Runtime.Serialization;

    using NUnit.Framework;

    [TestFixture]
    public class LambdaRelatedTests
    {
        private readonly ExceptionTests exceptionTests = new ExceptionTests();
        private CultureInfo savedCulture;

        [SetUp]
        public void SetUp()
        {
            // Important so that exception message are in english.
            this.savedCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        }

        [TearDown]
        public void TearDown()
        {
            // Boy scout rule ;-)
            Thread.CurrentThread.CurrentUICulture = this.savedCulture;
        }

        [Test]
        public void NoExceptionRaised()
        {
            Check.ThatCode((Action)(() => new object())).DoesNotThrow();
        }

        [Test]
        public void NotWords()
        {
            Check.ThatCode((Action)(() => new object())).Not.ThrowsAny();
        }

        [Test]
        public void UnexpectedExceptionRaised()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new ApplicationException(); }).DoesNotThrow();
            })
            .Throws<FluentCheckException>()
            .AndWhichMessage().StartsWith("\nThe checked code raised an exception, whereas it must not.\nThe raised exception:\n\t[{System.ApplicationException}:"); // TODO: mimic StartsWith
        }

        [Test]
        public void ExpectedExceptionRaised()
        {
            Check.ThatCode(() => { throw new InvalidOperationException(); }).Throws<InvalidOperationException>();
            Check.ThatCode(() => { throw new ApplicationException(); }).ThrowsAny();
        }

        [Test]
        public void DidNotRaiseExpected()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new Exception(); }).Throws<ApplicationException>();
            })
            .Throws<FluentCheckException>()
            .AndWhichMessage().StartsWith("\nThe checked code raised an exception of a different type than expected.\nRaised Exception\n\t[{System.Exception}:"); // TODO: mimic StartsWith
        }

        [Test]
        public void DidNotRaiseAny()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { new object(); }).ThrowsAny();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked code did not raise an exception, whereas it must.");
        }

        [Test]
            Check.ThatCode(() =>
            {
                // obsolete signature, kept for coverage
                Check.That(() => { new object(); }).ThrowsAny();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked code did not raise an exception, whereas it must.");
        public void DidNotRaiseAnyTypedCheck()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { new object(); }).Throws<Exception>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked code did not raise an exception, whereas it must.\nThe expected exception:\n\tan instance of type: [System.Exception]");
        }

        [Test]
        public void DidNotRaiseWhenUsedWithValidParameterlessFuncVariable()
        {
            Func<bool> sut = () => true;
//            Check.That(sut).DoesNotThrow();
        }

        [Test]
        public void CanRaiseWhenUsedWithParameterlessFuncVariable()
        {
            Func<bool> sut = () => { throw new Exception(); };

            Check.ThatCode(sut).ThrowsAny();
        }

        [Test]
        public void DidNotRaiseWhenUsedWithValidAnonymousFuncExpression()
        {
            Check.ThatCode(() => 1).DoesNotThrow();
        }

        [Test]
        public void DidNotRaiseWhenUsedWithValidDelegateExpression()
        {
            Check.ThatCode(delegate { var obj = new object(); }).DoesNotThrow();
        }

        [Test]
        public void DidNotRaiseWhenUsedWithAValidParameterlessVoidMethod()
        {
            var sut = new AnObjectWithParameterLessMethodThatCanBeInvokedLikeLambdas();
            Check.ThatCode(sut.AVoidParameterLessMethodThatShouldNotCrash).DoesNotThrow();
        }

        [Test]
        public void DidNotRaiseWhenUsedWithAValidParameterlessMethodReturningObject()
        {
            var obj = new AnObjectWithParameterLessMethodThatCanBeInvokedLikeLambdas();
            Func<object> sut = obj.AScalarParameterLessMethodThatShouldNotCrash;
            Check.ThatCode(sut).DoesNotThrow();
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
            Check.ThatCode(() => { throw new LambdaRelatedTests.LambdaExceptionForTest(123, "my error message"); }).Throws<LambdaRelatedTests.LambdaExceptionForTest>().WithMessage("Err #123 : my error message").And.WithProperty("ExceptionNumber", 123);
        }

        [Test]
        public void CanUseStringChecksOnMessage()
        {
            Check.ThatCode(() => { throw new LambdaRelatedTests.LambdaExceptionForTest(123, "my error message"); }).Throws<LambdaRelatedTests.LambdaExceptionForTest>().WhichMessage.Contains("error");

        }
        [Test]
        public void DidNotRaiseTheExpectedMessage()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new LambdaRelatedTests.LambdaExceptionForTest(321, "my error message"); }).Throws<LambdaRelatedTests.LambdaExceptionForTest>().WithMessage("a buggy message");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe message of the checked exception is not as expected.\nThe checked exception message:\n\t[\"Err #321 : my error message\"]\nThe expected exception message:\n\t[\"a buggy message\"]");
        }

        [Test]
        public void DidNotHaveExpectedPropertyName()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new LambdaRelatedTests.LambdaExceptionForTest(321, "my error message"); }).Throws<LambdaRelatedTests.LambdaExceptionForTest>().WithProperty("inexistingProperty", 123);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThere is no property [inexistingProperty] on exception type [LambdaExceptionForTest]."); // TODO: mimic Contains
        }

        [Test]
        public void DidNotHaveExpectedPropertyValue()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new LambdaRelatedTests.LambdaExceptionForTest(321, "my error message"); }).Throws<LambdaRelatedTests.LambdaExceptionForTest>().WithProperty("ExceptionNumber", 123);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe property [ExceptionNumber] of the checked exception's property does not have the expected value.\nThe checked exception's property:\n\t[321]\nThe given exception's property:\n\t[123]"); // TODO: mimic Contains
        }

        [Test]
        public void CheckReturnValue()
        {
            Check.ThatCode(() => 4).DoesNotThrow().And.WhichResult().IsEqualTo(4);
        }

        [Test]
        public void Should_not_raise_when_expected_DueTo_exception_type_is_part_of_inner_exception()
        {
            Check.ThatCode(() => { throw new ArgumentException("outerException dummy message", new ArgumentOutOfRangeException("kamoulox")); })
                .Throws<ArgumentException>()
                .DueTo<ArgumentOutOfRangeException>();

            Check.ThatCode(() => { throw new ArgumentException("outerException dummy message", new NotFiniteNumberException("whatever mate", new ArgumentOutOfRangeException("kamoulox"))); })
                .Throws<ArgumentException>()
                .DueTo<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Should_raise_when_expected_DueTo_exception_is_not_found_somewhere_within_the_inner_exception()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new ArgumentException("outerException dummy message", new ArgumentOutOfRangeException("kamoulox")); })
                        .Throws<ArgumentException>()
                        .DueTo<NotFiniteNumberException>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked exception did not contain an expected inner exception whereas it must.\nThe inner exception(s):\n\t[\"{ System.ArgumentOutOfRangeException } \"Specified argument was out of the range of valid values.\r\nParameter name: kamoulox\"\"]\nThe expected inner exception:\n\t[System.NotFiniteNumberException]");
        }

        [Test]
        public void Should_raise_with_the_complete_stack_of_InnerExceptions_details_when_expected_DueTo_exception_is_not_found_somewhere_within_the_inner_exception()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new ArgumentException("outerException dummy message", new InvalidCastException("whatever mate", new ArgumentOutOfRangeException("kamoulox"))); })
                        .Throws<ArgumentException>()
                        .DueTo<NotFiniteNumberException>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked exception did not contain an expected inner exception whereas it must.\nThe inner exception(s):\n\t[\"{ System.InvalidCastException } \"whatever mate\"\n--> { System.ArgumentOutOfRangeException } \"Specified argument was out of the range of valid values.\r\nParameter name: kamoulox\"\"]\nThe expected inner exception:\n\t[System.NotFiniteNumberException]");
        }


        #region Lambda related Test Data

        [Serializable]
        private class LambdaExceptionForTest : Exception
        {
            public int ExceptionNumber { get; private set; }

            public LambdaExceptionForTest(int exeptionNumber, string message)
                : base(FormatMessage(exeptionNumber, message))
            {
                this.ExceptionNumber = exeptionNumber;
            }

            #region Serializable stuff

            protected LambdaExceptionForTest(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
                this.ExceptionNumber = info.GetInt32("ExceptionNumber");
            }

            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
                info.AddValue("ExceptionNumber", this.ExceptionNumber);
            }

            #endregion

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
            private readonly int devilMathValue;

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