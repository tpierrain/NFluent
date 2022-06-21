 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="CheckThatCodeShould.cs" company="">
 //   Copyright 2016 Cyrille DUPUYDAUBY & Thomas PIERRAIN
 //   Licensed under the Apache License, Version 2.0 (the "License");
 //   you may not use this file except in compliance with the License.
 //   You may obtain a copy of the License at
 //       http://www.apache.org/licenses/LICENSE-2.0
 //   Unless required by applicable law or agreed to in writing, software
 //   distributed under the License is distributed on an "AS IS" BASIS,
 //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 //   See the License for the specific language governing permissions and
 //   limitations under the License.
 // </copyright>
 // --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Tests
{
#if !NETCOREAPP1_0 && !NETCOREAPP1_1
    using System.Runtime.Serialization;
#endif
    using System;

    using Helpers;

    using ApiChecks;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class CheckThatCodeShould
    {
        private CultureSession session;

        [OneTimeSetUp]
        public void ForceCulture()
        {
            this.session = new CultureSession("en-US");
        }

        [OneTimeTearDown]
        public void RestoreCulture()
        {
            this.session.Dispose();
        }

        [Test]
        public void NoExceptionRaised()
        {
            Check.ThatCode(() =>
            {
                var unused = new object();
            }).DoesNotThrow();
        }

        [Test]
        public void NotWords()
        {
            Check.ThatCode(() =>
            {
                var unused = new object();
            }).Not.ThrowsAny();
        }

        [Test]
        public void UnexpectedExceptionRaised()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => throw new Exception()).DoesNotThrow();
            })
            .IsAFailingCheckWithMessage("",
                    "The checked code raised an exception, whereas it must not.",
                    "The checked code's raised exception:", 
                    Criteria.FromRegEx(".*"));
        }

        [Test]
        public void DoesNotThrowSucceedWhenExceptionRaisedAndNegated()
        {
            Check.ThatCode(() => throw new Exception()).Not.DoesNotThrow();
        }

        [Test]
        public void DoesNotThrowFailsWhenNoExceptionRaisedAndNegated()
        {
            Check.ThatCode(()=>
            Check.ThatCode(() => 1).Not.DoesNotThrow()).IsAFailingCheckWithMessage("",
                "The checked code did not raise an exception, whereas it must.");
        }

        [Test]
        public void ExpectedExceptionRaised()
        {
            Check.ThatCode(() => throw new InvalidOperationException()).Throws<InvalidOperationException>();
            Check.ThatCode(() => throw new Exception()).ThrowsAny();
        }

        [Test]
        public void DidNotRaiseExpected()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => throw new Exception()).Throws<InvalidOperationException>();
            })
            .IsAFailingCheckWithMessage("",
                    "The checked code's raised exception is of a different type than expected.",
                    "The checked code's raised exception:",
                    Criteria.FromRegEx(".*"),
                    "The expected code's raised exception:", 
                    "\tan instance of [System.InvalidOperationException]");
        }

        [Test]
        public void ThrowsFailsWhenNegatedIfImproperExceptionRaised()
        {
            Check.ThatCode(() =>
                {
                    Check.ThatCode(() => throw new Exception()).Not.Throws<Exception>();
                })
                .IsAFailingCheckWithMessage("", 
                    "The checked code's raised exception raised an exception of the forbidden type.",
                    "The checked code's raised exception:",
                    "\t[{System.Exception}: 'Exception of type 'System.Exception' was thrown.']",
                    "The expected code's raised exception: different from",
                    "\tan instance of [System.Exception]");
        }

        [Test]
        public void DidNotRaiseAny()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() =>
                {
                    var unused = new object();
                }).ThrowsAny();
            })
            .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked code did not raise an exception, whereas it must.");
        }

        [Test]
        public void ThrowsAnySucceedWhenNegatedAndNoException()
        {   
            Check.ThatCode(() =>
            {
                var unused = new object();
            }).Not.ThrowsAny();
        }

        [Test]
        public void ThrowsAnyFailsWhenNegatedAndExceptionRaised()
        {
            Check.ThatCode(() =>
                {
                    Check.ThatCode(() => { throw new Exception(); }).Not.ThrowsAny();
                })
                .IsAFailingCheckWithMessage("",
                    "The checked code raised an exception, whereas it must not.",
                    "The checked code's raised exception:",
                    "\t[{System.Exception}: 'Exception of type 'System.Exception' was thrown.']");
        }
        [Test]
        public void DidNotRaiseAnyTypedCheck()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() =>
                {
                    var unused = new object();
                }).Throws<Exception>();
            })
            .IsAFailingCheckWithMessage("",
                    "The checked code did not raise an exception, whereas it must.",
                    "The expected code's raised exception:",
                    "\tan instance of [System.Exception]");
        }

        [Test]
        public void DidNotRaiseWhenUsedWithValidParameterlessFuncVariable()
        {
            bool Sut() => true;
            Check.ThatCode<bool>(Sut).DoesNotThrow();
        }

        [Test]
        public void CanRaiseWhenUsedWithParameterlessFuncVariable()
        {
#if DOTNET_30 || DOTNET_20
            NFluent.
#endif
            Func<bool> sut = () => throw new Exception();

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
            Check.ThatCode(delegate { var unused = new object(); }).DoesNotThrow();
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
#if DOTNET_30 || DOTNET_20
            NFluent.
#endif
            Func<object> sut = obj.AScalarParameterLessMethodThatShouldNotCrash;
            Check.ThatCode(sut).DoesNotThrow();
        }

        [Test]
        public void CanRaiseWhenUsedWithAnonymousActionExpression()
        {
            Check.ThatCode(() => throw new Exception()).ThrowsAny();
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
        public void
            ThrowsWorkWithAlternateSyntax()
        {
            Check.ThatCode(() => throw new DivideByZeroException()).ThrowsType(typeof(DivideByZeroException));
        }

        [Test]
        public void CanCheckForAMessageOnExceptionRaised()
        {
            Check.ThatCode(() => throw new LambdaExceptionForTest(123, "my error message")).
                Throws<LambdaExceptionForTest>().WithMessage("Err #123 : my error message").And.WithProperty("ExceptionNumber", 123);
        }

        [Test]
        public void DidNotRaiseTheExpectedMessage()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => throw new LambdaExceptionForTest(321, "my error message")).Throws<LambdaExceptionForTest>().WithMessage("a buggy message");
            })
            .IsAFailingCheckWithMessage("",
                    "The checked exception's message is not as expected.",
                    "The checked exception's message:",
                    "\t[\"Err #321 : my error message\"]",
                    "The expected exception's message:",
                    "\t[\"a buggy message\"]");
        }

        [Test]
        public void DidNotHaveExpectedPropertyName()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new LambdaExceptionForTest(321, "my error message"); }).
                    Throws<LambdaExceptionForTest>().
                    WithProperty("inexistingProperty", 123);
            })
            .IsAFailingCheckWithMessage("", 
                    "There is no property [inexistingProperty] on exception type [LambdaExceptionForTest].",
                    "The expected exception's property [inexistingProperty]:", 
                    "\t[123]");

        }

        [Test]
        public void DidNotHaveExpectedPropertyValue()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new LambdaExceptionForTest(321, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithProperty("ExceptionNumber", 123);
            })
            .IsAFailingCheckWithMessage("", 
            "The checked exception's property [ExceptionNumber] does not have the expected value.", 
            "The checked exception's property [ExceptionNumber]:",
            "\t[321]",
            "The expected exception's property [ExceptionNumber]:",
            "\t[123]"); 
        }
#if !DOTNET_30 && !DOTNET_20
        [Test]
        public void CanCheckForPropertyWithExpression()
        {
            Check.ThatCode(() => { throw new CheckThatCodeShould.LambdaExceptionForTest(321, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithProperty(ex => ex.ExceptionNumber, 321);
        }


        [Test]
        public void CanCheckForPropertyWithComplexExpression()
        {
            Check.ThatCode(() => { throw new CheckThatCodeShould.LambdaExceptionForTest(321, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithProperty(ex => ex.ExceptionNumber + 0, 321);
        }

        [Test]
        public void CanCheckForPropertyWithBasicExpression()
        {
            Check.ThatCode(() => { throw new LambdaExceptionForTest(321, "my error message"); })
                .Throws<LambdaExceptionForTest>()
                .WithProperty(ex => 321, 321);
        }

        [Test]
        public void DidNotHaveExpectedPropertyValueWithExpression()
        {
            Check.ThatCode(
                () =>
                    {
                        Check.ThatCode(
                                () => { throw new LambdaExceptionForTest(321, "my error message"); })
                            .Throws<LambdaExceptionForTest>()
                            .WithProperty(ex => ex.ExceptionNumber, 123);
                    })
                .IsAFailingCheckWithMessage(
                string.Empty,
                "The checked exception's property [ExceptionNumber] does not have the expected value.",
                "The checked exception's property [ExceptionNumber]:",
                "\t[321]",
                "The expected exception's property [ExceptionNumber]:",
                "\t[123]");
        }
#endif

        [Test]
        public void CheckReturnValue()
        {
            Check.ThatCode(() => 4).DoesNotThrow().And.WhichResult().IsEqualTo(4);
        }

        [Test]
        public void AndWorksProperly()
        {
            Check.ThatCode(() => 4).LastsLessThan(100, TimeUnit.Milliseconds).And
                .ConsumesLessThan(100, TimeUnit.Milliseconds);
        }

        [Test]
        public void Should_not_raise_when_expected_DueTo_exception_type_is_part_of_inner_exception()
        {
            Check.ThatCode(
                () => throw new ArgumentException(
                    "outerException dummy message",
                    new ArgumentOutOfRangeException("kamoulox"))).
                Throws<ArgumentException>().
                DueTo<ArgumentOutOfRangeException>();

            Check.ThatCode(
                () => throw new ArgumentException(
                    "outerException dummy message",
                    new Exception("whatever mate", new ArgumentOutOfRangeException("kamoulox")))).
            Throws<ArgumentException>().
            DueTo<ArgumentOutOfRangeException>().
            WithMessage("Specified argument was out of the range of valid values." + Environment.NewLine + "Parameter name: kamoulox");
        }

        [Test]
        public void Should_not_raise_when_expected_DueTo_exception_is_null()
        {
            Check.ThatCode(()=>
            Check.ThatCode(
                () => throw new ArgumentException(
                    "outerException dummy message",
                    (Exception) null)).ThrowsAny().DueTo<Exception>()).IsAFailingCheckWithMessage("",
                "There is no inner exception.", 
                "The expected value's inner exception:", 
                "\tan instance of [System.Exception]");
        }

        [Test]
        public void Should_raise_when_expected_DueTo_exception_is_not_found_somewhere_within_the_inner_exception()
        {
            Check.ThatCode(() =>
            {
                // ReSharper disable once NotResolvedInText
                Check.ThatCode(() => throw new ArgumentException("outerException dummy message", new ArgumentOutOfRangeException("kamoulox")))
                        .Throws<ArgumentException>()
                        .DueTo<Exception>();
            })
            .IsAFailingCheckWithMessage(
                    "",
                    "The checked value's inner exception is not of the expected type.",
                    "The checked value's inner exception:",
                    "\t[{System.ArgumentOutOfRangeException}: 'Specified argument was out of the range of valid values.",
                    "Parameter name: kamoulox'] of type: [System.ArgumentOutOfRangeException]",
                    "The expected value's inner exception:",
                    "\tan instance of [System.Exception]");
        }

        [Test]
        public void Should_raise_when_expected_DueToAny_exception_is_not_found_somewhere_within_the_inner_exception()
        {
            Check.ThatCode(() =>
            {
                // ReSharper disable once NotResolvedInText
                Check.ThatCode(() => throw new ArgumentException("outerException dummy message", new ArgumentOutOfRangeException("kamoulox")))
                        .Throws<ArgumentException>()
                        .DueToAnyFrom(typeof(Exception), typeof(ArgumentException));
            })
            .IsAFailingCheckWithMessage(
                    "",
                    "The checked value's inner exception is not of one of the expected types.",
                    "The checked value's inner exception:",
                    "\t[{System.ArgumentOutOfRangeException}: 'Specified argument was out of the range of valid values.",
                    "Parameter name: kamoulox'] of type: [System.ArgumentOutOfRangeException]",
                    "The expected value's inner exception: an instance of any",
                    "\tan instance of these types {System.Exception, System.ArgumentException}");
            Check.ThatCode(() =>
            {
                // ReSharper disable once NotResolvedInText
                Check.ThatCode(() => throw new ArgumentException("outerException dummy message"))
                        .Throws<ArgumentException>()
                        .DueToAnyFrom(typeof(Exception), typeof(ArgumentException));
            })
            .IsAFailingCheckWithMessage(
                    "",
                    "There is no inner exception.",
                    "The expected value's inner exception: an instance of any",
                    "\tan instance of these types {System.Exception, System.ArgumentException}");
        }

        [Test]
        public void ShouldRaiseWhenUsingDueToOnANegatedCheck()
        {
            Check.ThatCode(() =>
                    Check.ThatCode(() => { }).Not.ThrowsAny().DueToAnyFrom(typeof(Exception)))
                .Throws<InvalidOperationException>().WithMessage("DueToAnyFrom can't be used when negated");
        }

        [Test]
        public void ShouldSucceed_when_expected_DueToAny_exception_is_found_somewhere_within_the_inner_exception()
        {
                // ReSharper disable once NotResolvedInText
                Check.ThatCode(() => throw new ArgumentException("outerException dummy message", new ArgumentOutOfRangeException("kamoulox")))
                        .Throws<ArgumentException>()
                        .DueToAnyFrom(typeof(ArgumentOutOfRangeException), typeof(ArgumentException));
                // duplicate test due to issue with NCover
                Check.ThatCode(() => throw new ArgumentException("outerException dummy message", new ArgumentException("kamoulox")))
                    .Throws<ArgumentException>()
                    .DueToAnyFrom(typeof(ArgumentOutOfRangeException), typeof(ArgumentException));
        }

        [Test]
        public void Should_raise_with_the_complete_stack_of_InnerExceptions_details_when_expected_DueTo_exception_is_not_found_somewhere_within_the_inner_exception()
        {
            Check.ThatCode(() =>
            {
                // ReSharper disable once NotResolvedInText
                Check.ThatCode(() => throw new ArgumentException("outerException dummy message", new InvalidCastException("whatever mate", new ArgumentOutOfRangeException("kamoulox"))))
                        .Throws<ArgumentException>()
                        .DueTo<Exception>();
            })
            .IsAFailingCheckWithMessage("",
                    "The checked value's inner exception is not of the expected type.",
                    "The checked value's inner exception:",
                    "\t[{System.InvalidCastException}: \'whatever mate\'] of type: [System.InvalidCastException]",
                    "The expected value's inner exception:",
                    "\tan instance of [System.Exception]");
        }

        [Test]
        // GH #228
        public void
            ThrowExtensionsFailWhenNegated()
        {
            Check.ThatCode(() => 
                Check.ThatCode(() => throw new InvalidOperationException())
                .Not.Throws<ArgumentNullException>().WithMessage("any")
                ).Throws<InvalidOperationException>().WithMessage("WithMessage can't be used when negated");

            Check.ThatCode(() => 
                Check.ThatCode(() => throw new InvalidOperationException())
                .Not.Throws<ArgumentNullException>().DueTo<Exception>()
                ).Throws<InvalidOperationException>().WithMessage("DueTo can't be used when negated");            

            Check.ThatCode(() => 
                Check.ThatCode(() => throw new InvalidOperationException())
                .Not.Throws<ArgumentNullException>().WithProperty("any", 12)
                ).Throws<InvalidOperationException>().WithMessage("WithProperty can't be used when negated");
            
            Check.ThatCode(() => 
                Check.ThatCode(() => throw new InvalidOperationException())
                .Not.ThrowsType(typeof(ArgumentNullException)).WithMessage("any")
                ).Throws<InvalidOperationException>().WithMessage("WithMessage can't be used when negated");
#if !DOTNET_20 && !DOTNET_30
            Check.ThatCode(() => 
                Check.ThatCode(() => throw new InvalidOperationException())
                    .Not.Throws<ArgumentNullException>().WithProperty((x) => x.Message, "")
            ).Throws<InvalidOperationException>().WithMessage("WithProperty can't be used when negated");
#endif
        }

#region Lambda related Test Data
#if !NETCOREAPP1_0 && !NETCOREAPP1_1
        [Serializable]
#endif
        private class LambdaExceptionForTest : Exception
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int ExceptionNumber { get; private set; }

            public LambdaExceptionForTest(int exceptionNumber, string message)
                : base(FormatMessage(exceptionNumber, message))
            {
                this.ExceptionNumber = exceptionNumber;
            }

#region Serializable stuff
#if !NETCOREAPP1_0 && !NETCOREAPP1_1
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
#endif
#endregion

            private static string FormatMessage(int exceptionNumber, string message)
            {
                return $"Err #{exceptionNumber} : {message}";
            }
        }

        private class AnObjectThatCanCrashOnCtor
        {
            public AnObjectThatCanCrashOnCtor(int i)
            {
                var unused = 1 / i;
            }
        }

        private class AnObjectThatCanCrashWithPropertyGet
        {
            private readonly int devilMathValue;

            public AnObjectThatCanCrashWithPropertyGet(int i)
            {
                this.devilMathValue = i;
            }

            public int BeastBreaker => 666 / this.devilMathValue;
        }

        private class AnObjectWithParameterLessMethodThatCanBeInvokedLikeLambdas
        {
            // ReSharper disable once UnusedMember.Local

            public void AVoidParameterLessMethodThatShouldNotCrash()
            {
                var unused = new object();
            }

            // ReSharper disable once UnusedMember.Local

            public object AScalarParameterLessMethodThatShouldNotCrash()
            {
                return new object();
            }
        }

#endregion
    }
}