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

        [SetUp]
        public void ForceCulture()
        {
            this.session = new CultureSession("en-US");
        }

        [TearDown]
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
            .ThrowsAny()
            .AndWhichMessage().StartsWith(Environment.NewLine+ "The checked code raised an exception, whereas it must not." + Environment.NewLine + "The raised exception:" + Environment.NewLine + "\t[{System.Exception}:"); // TODO: mimic StartsWith
        }

        [Test]
        public void ExpectedExceptionRaised()
        {
            Check.ThatCode(() => { throw new InvalidOperationException(); }).Throws<InvalidOperationException>();
            Check.ThatCode(() => { throw new Exception(); }).ThrowsAny();
        }

        [Test]
        public void DidNotRaiseExpected()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new Exception(); }).Throws<InvalidOperationException>();
            })
            .ThrowsAny()
            .AndWhichMessage().StartsWith(Environment.NewLine+ "The checked code raised an exception of a different type than expected." + Environment.NewLine + "The raised exception:" + Environment.NewLine + "\t[{System.Exception}:"); // TODO: mimic StartsWith
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
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked code did not raise an exception, whereas it must.");
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
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked code did not raise an exception, whereas it must." + Environment.NewLine + "The expected exception:" + Environment.NewLine + "\tan instance of type: [System.Exception]");
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
            Check.ThatCode(() => throw new LambdaExceptionForTest(123, "my error message")).Throws<LambdaExceptionForTest>().WithMessage("Err #123 : my error message").And.WithProperty("ExceptionNumber", 123);
        }

        [Test]
        public void DidNotRaiseTheExpectedMessage()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => throw new LambdaExceptionForTest(321, "my error message")).Throws<LambdaExceptionForTest>().WithMessage("a buggy message");
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The message of the checked exception is not as expected." + Environment.NewLine + "The checked exception message:" + Environment.NewLine + "\t[\"Err #321 : my error message\"]" + Environment.NewLine + "The expected exception message:" + Environment.NewLine + "\t[\"a buggy message\"]");
        }

        [Test]
        public void DidNotHaveExpectedPropertyName()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => { throw new LambdaExceptionForTest(321, "my error message"); }).Throws<LambdaExceptionForTest>().WithProperty("inexistingProperty", 123);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine + "There is no property [inexistingProperty] on exception type [LambdaExceptionForTest]."); // TODO: mimic Contains
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
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine + 
            "The checked exception's property [ExceptionNumber] does not have the expected value." + Environment.NewLine + 
            "The checked exception's property [ExceptionNumber]:" + Environment.NewLine +
            "\t[321]" + Environment.NewLine +
            "The expected exception's property [ExceptionNumber]:" + Environment.NewLine +
            "\t[123]"); // TODO: mimic Contains
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
                    }).Throws<FluentCheckException>().AndWhichMessage().AsLines().ContainsExactly(
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
        public void Should_not_raise_when_expected_DueTo_exception_type_is_part_of_inner_exception()
        {
            // ReSharper disable once NotResolvedInText
            Check.ThatCode(
                () =>
                    {
                        throw new ArgumentException(
                            "outerException dummy message",
                            new ArgumentOutOfRangeException("kamoulox"));
                    }).Throws<ArgumentException>().DueTo<ArgumentOutOfRangeException>();

            // ReSharper disable once NotResolvedInText
            Check.ThatCode(
                () =>
                    {
                        throw new ArgumentException(
                            "outerException dummy message",
                            new Exception("whatever mate", new ArgumentOutOfRangeException("kamoulox")));
                    }).Throws<ArgumentException>().DueTo<ArgumentOutOfRangeException>().
                    WithMessage("Specified argument was out of the range of valid values." + Environment.NewLine + "Parameter name: kamoulox");
        }

        [Test]
        public void Should_raise_when_expected_DueTo_exception_is_not_found_somewhere_within_the_inner_exception()
        {
            Check.ThatCode(() =>
            {
                // ReSharper disable once NotResolvedInText
                Check.ThatCode(() => { throw new ArgumentException("outerException dummy message", new ArgumentOutOfRangeException("kamoulox")); })
                        .Throws<ArgumentException>()
                        .DueTo<Exception>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked exception did not contain an expected inner exception whereas it must." + Environment.NewLine + "The inner exception(s):" + Environment.NewLine + "\t[\"{ System.ArgumentOutOfRangeException } \"Specified argument was out of the range of valid values." + Environment.NewLine + "Parameter name: kamoulox\"\"]" + Environment.NewLine + "The expected inner exception:" + Environment.NewLine + "\t[System.Exception]");
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
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked exception did not contain an expected inner exception whereas it must." + Environment.NewLine + "The inner exception(s):" + Environment.NewLine + "\t[\"{ System.InvalidCastException } \"whatever mate\"" +Environment.NewLine +"--> { System.ArgumentOutOfRangeException } \"Specified argument was out of the range of valid values." + Environment.NewLine + "Parameter name: kamoulox\"\"]" + Environment.NewLine + "The expected inner exception:" + Environment.NewLine + "\t[System.Exception]");
        }

        [Test]
        // GH 228
        public void
            ThrowExtensionsFailWhenNegated()
        {
            Check.ThatCode(() => 
                Check.ThatCode(() => throw new InvalidOperationException())
                .Not.Throws<ArgumentNullException>().WithMessage("any")
                ).Throws<InvalidOperationException>();

            Check.ThatCode(() => 
                Check.ThatCode(() => throw new InvalidOperationException())
                .Not.Throws<ArgumentNullException>().DueTo<Exception>()
                ).Throws<InvalidOperationException>();            

            Check.ThatCode(() => 
                Check.ThatCode(() => throw new InvalidOperationException())
                .Not.Throws<ArgumentNullException>().WithProperty("any", 12)
                ).Throws<InvalidOperationException>();
            
            Check.ThatCode(() => 
                Check.ThatCode(() => throw new InvalidOperationException())
                .Not.ThrowsType(typeof(ArgumentNullException)).WithMessage("any")
                ).Throws<InvalidOperationException>();
#if !DOTNET_20 && !DOTNET_30
            Check.ThatCode(() => 
                Check.ThatCode(() => throw new InvalidOperationException())
                    .Not.Throws<ArgumentNullException>().WithProperty((x) => x.Message, "")
            ).Throws<InvalidOperationException>();
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

            public LambdaExceptionForTest(int exeptionNumber, string message)
                : base(FormatMessage(exeptionNumber, message))
            {
                this.ExceptionNumber = exeptionNumber;
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