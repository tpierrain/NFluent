// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EqualRelatedTests.cs" company="">
//   Copyright 2013 Thomas PIERRAIN, Cyrille DUPUYDAUBY
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
    using System;
    using System.Collections.Generic;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class EqualRelatedTests
    {
        #region IsEqualTo()

        [Test]
        public void LegacyMode()
        {
            var test = new List<string>();
            var test2 = new List<string>();

            Check.That(test).IsEqualTo(test2);

            var currentMode = Check.EqualMode;
            try
            {
                Check.EqualMode = EqualityMode.Equals;
                Check.ThatCode(() => Check.That(test).IsEqualTo(test2)).IsAFailingCheckWithMessage("",
                    "The checked enumerable is different from the expected one.",
                    "The checked enumerable:",
                    "\t{} (0 item)",
                    "The expected enumerable:",
                    "\t{} (0 item)");
            }
            finally
            {
                Check.EqualMode = currentMode;
            }
        }

        [Test]
        public void IsShouldWork()
        {
            Check.That(2).Is(2);
        }

        [Test]
        public void FailOnIllegalValues()
        {
            var test = new List<string>();
            var test2 = new List<string>();

            Check.That(test).IsEqualTo(test2);

            var currentMode = Check.EqualMode;
            try
            {
                Check.EqualMode = (EqualityMode) (-1);
                Check.ThatCode(() => Check.That(test).IsEqualTo(test2)).Throws<NotSupportedException>();
            }
            finally
            {
                Check.EqualMode = currentMode;
            }
        }

        [Test]
        public void WorkForNullValue()
        {
            Check.ThatCode(() =>
                    Check.That((object) null).IsEqualTo(new object())
                )
                .IsAFailingCheckWithMessage("",
                    "The checked object is different from the expected one.",
                    "The checked object:",
                    "\t[null] of type: [object]",
                    "The expected object:",
                    "\t[System.Object] of type: [object]");
        }

        [Test]
        public void IsEqualToWorksWithBooleans()
        {
            const bool tddSucks = false;
            Check.That(tddSucks).IsNotEqualTo(true);
        }

        [Test]
        public void CanNegateIsEqualToWithBooleans()
        {
            const bool tddSucks = false;
            Check.That(tddSucks).Not.IsNotEqualTo(false);
        }

        [Test]
        public void IsEqualToWorksWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo("Son of a test");
        }

        [Test]
        public void IsEqualToWorksWithStringAndObject()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo((object) "Son of a test");
        }

        [Test]
        public void IsEqualToWorksWithArray()
        {
            var array = new[] {45, 43, 54, 666};
            var otherReference = array;

            Check.That(array).IsEqualTo(array);
            Check.That(array).IsEqualTo(otherReference);
        }

        [Test]
        public void IsEqualToWorksWithObject()
        {
            var heroe = new Person {Name = "Gandhi"};
            var otherReference = heroe;

            Check.That(heroe).IsEqualTo(otherReference);
        }

        [Test]
        public void CanNegateIsEqualToWithObject()
        {
            var heroe = new Person {Name = "Gandhi"};

            Check.That(heroe).Not.IsEqualTo(null);
        }

        [Test]
        public void NotIsEqualToWithObjectThrowsExceptionWhenFailing()
        {
            var heroe = new Person {Name = "Gandhi"};
            var otherReference = heroe;

            Check.ThatCode(() => { Check.That(heroe).Not.IsEqualTo(otherReference); })
                .IsAFailingCheckWithMessage("",
                    "The checked value is equal to the given one whereas it must not.",
                    "The expected value: different from",
                    "\t[Gandhi] of type: [NFluent.Tests.Person]");
        }

        [Test]
        public void IsEqualWorksWithIntNumbers()
        {
            const int firstInt = 23;
            const int secondButIdenticalInt = 23;

            Check.That(secondButIdenticalInt).IsEqualTo(firstInt);
        }

        [Test]
        public void IsEqualWorksWithBytes()
        {
            const byte firstInt = 12;
            const int secondButIdenticalInt = 12;

            Check.That((short) 255).Equals((byte)255);
            Check.That(secondButIdenticalInt).IsEqualTo(firstInt);
        }

        [Test]
        public void IsEqualWorksWithDoubleNumbers()
        {
            var firstDouble = 23.7D;
            var secondButIdenticalDouble = 23.7D;

            Check.That(secondButIdenticalDouble).IsEqualTo(firstDouble);
        }

        [Test]
        public void IsEqualWorksWithFloatNumbers()
        {
            var firstFloat = 23.56F;
            var secondButIdenticalFloat = 23.56F;

            Check.That(secondButIdenticalFloat).IsEqualTo(firstFloat);
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenFailingWithIntArray()
        {
            var array = new[] {45, 43, 54, 666, 63};
            var otherSimilarButNotEqualArray = new[] {25, 43, 54, 667, 63};

            Check.ThatCode(() => { Check.That(array).IsEqualTo(otherSimilarButNotEqualArray); })
                .IsAFailingCheckWithMessage("", 
                    "The checked enumerable is different from the expected one. 2 differences found!", 
                    "actual[0] = 45 instead of 25.", 
                    "actual[3] = 666 instead of 667.",
                    "The checked enumerable:", 
                    "\t{*45*,43,54,666,63} (5 items)", 
                    "The expected enumerable:", 
                    "\t{*25*,43,54,667,63} (5 items)");
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenFailingWithIntArrayOfDifferentRank()
        {
            var array = new int[2, 3];
            var otherSimilarButNotEqualArray = new int[6];

            Check.ThatCode(() => { Check.That(array).IsEqualTo(otherSimilarButNotEqualArray); })
                .IsAFailingCheckWithMessage("", 
                    "The checked enumerable is different from the expected one.", 
                    "actual.Rank = 2 instead of 1.", 
                    "The checked enumerable:", 
                    "\t{{0,0,0},{0,0,0}} (6 items) of type: [int[,]]", 
                    "The expected enumerable:", 
                    "\t{0,0,0,0,0,0} (6 items) of type: [int[]]");
        }

        [Test]
        public void IsEqualToThrowsExceptionWithProperMessage()
        {
            var array = new[,,] {{{0, 1}, {2, 3}, {4, 5}}, {{6, 7}, {8, 9}, {10, 11}}};
            var otherSimilarButNotEqualArray = new int[6];

            Check.ThatCode(() => { Check.That(array).IsEqualTo(otherSimilarButNotEqualArray); })
                .IsAFailingCheckWithMessage("", 
                    "The checked enumerable is different from the expected one.", 
                    "actual.Rank = 3 instead of 1.",
                    "The checked enumerable:", 
                    "\t{{{0,1},{2,3},{4,5}},{{6,7},{8,9},{10,11}}} (12 items) of type: [int[,,]]", 
                    "The expected enumerable:", 
                    "\t{0,0,0,0,0,0} (6 items) of type: [int[]]");
        }

        [Test]
        public void
            ShouldWorkWithArbitraryIndexBase()
        {
            var array = Array.CreateInstance(typeof(int), new []{2,2}, new []{-1, -1});
            var otherArray = new int[2, 2];
            var val = 0;
            for (var i = 0; i < 2; i++)
            {
                for(var j = 0; j<2; j++)
                {
                    array.SetValue(val, i-1, j-1);
                    otherArray[i, j] = val;
                    val++;
                }
            }
            Check.That(array).IsEqualTo(otherArray);
            for (var i = 0; i < 2; i++)
            {
                for(var j = 0; j<2; j++)
                {
                    array.SetValue(0, i-1, j-1);
                }
            }

            Check.ThatCode(() =>
                Check.That(array).IsEqualTo(otherArray)).
                IsAFailingCheckWithMessage(	"", 
                    "The checked enumerable is different from the expected one. 3 differences found!", 
                    "actual[0,1] = 0 instead of 1.", 
                    "actual[1,0] = 0 instead of 2.", 
                    "actual[1,1] = 0 instead of 3.",
                    "The checked enumerable:", 
                    "\t{{0,*0*},{0,0}} (4 items)", 
                    "The expected enumerable:", 
                    "\t{{0,*1*},{2,3}} (4 items)");
           
            Check.ThatCode(() =>
                Check.That(otherArray).IsEqualTo(array)).
                IsAFailingCheckWithMessage(	"", 
                    "The checked enumerable is different from the expected one. 3 differences found!", 
                    "actual[0,1] = 1 instead of 0.",
                    "actual[1,0] = 2 instead of 0.", 
                    "actual[1,1] = 3 instead of 0.",
                    "The checked enumerable:", 
                    "\t{{0,*1*},{2,3}} (4 items)",
                    "The expected enumerable:", 
                    "\t{{0,*0*},{0,0}} (4 items)");
        }

        [Test]
        public void
            ShouldProvideTruncationOnLongEnumeration()
        {
            var first = "A sentence can provide a long enumeration.".ToCharArray();
            Check.ThatCode(() => Check.That(first).IsEqualTo("A sentence can provide 1 long enumeration.".ToCharArray()))
                .IsAFailingCheckWithMessage("", 
                    "The checked enumerable is different from the expected one.", 
                    "actual[23] = 'a' instead of '1'.", 
                    "The checked enumerable:", 
                    "\t{...,'n',' ','p','r','o','v','i','d','e',' ',*'a'*,' ','l','o','n','g',' ','e','n','u',...} (42 items)", 
                    "The expected enumerable:", 
                    "\t{...,'n',' ','p','r','o','v','i','d','e',' ',*'1'*,' ','l','o','n','g',' ','e','n','u',...} (42 items)");
        }

        [Test]
        public void
            ShouldProvideOnlyTheFirstErrors()
        {
            var first = "A sentence can provide a long enumeration.".ToCharArray();
            Check.ThatCode(() => Check.That(first).IsEqualTo("A sentence can provide long enumeration.  ".ToCharArray()))
                .IsAFailingCheckWithMessage("", 
                    "The checked enumerable is different from the expected one. 19 differences found!", 
                    "actual[23] value ('a') was found at index 23 instead of 34.", 
                    "actual[24] value (' ') was found at index 24 instead of 27.", 
                    "actual[25] value ('l') was found at index 25 instead of 23.", 
                    "actual[26] value ('o') was found at index 26 instead of 24.", 
                    "actual[27] value ('n') was found at index 27 instead of 25.",                    
                    "... (14 differences omitted)",
                    "The checked enumerable:", 
                    "\t{...,'n',' ','p','r','o','v','i','d','e',' ',*'a'*,' ','l','o','n','g',' ','e','n','u',...} (42 items)", 
                    "The expected enumerable:", 
                    "\t{...,'o','n','g',' ','e','n','u','m','e','r',*'a'*,'t','i','o','n','.',' ',' '} (42 items)");
        }

        [Test]
        public void
            ShouldProvideSmartDiffAnalysis()
        {
            Check.ThatCode(() => Check.That("23".ToCharArray()).IsEqualTo("12".ToCharArray()))
                .IsAFailingCheckWithMessage("",
                    "The checked enumerable is different from the expected one. 2 differences found!",
                    "actual[0] value ('2') was found at index 0 instead of 1.", 
                    "actual[1] = '3' instead of '2'.",
                    "The checked enumerable:", 
                    "\t{*'2'*,'3'} (2 items)",
                    "The expected enumerable:",
	                "\t{'1',*'2'*} (2 items)");
        }

        [Test]
        public void
            ShouldProvideSmartDiffAnalysis2()
        {
            var first = "l.".ToCharArray();
            var other = ". ".ToCharArray();
            Check.ThatCode(() => Check.That(first).IsEqualTo(other))
                .IsAFailingCheckWithMessage("",
                    "The checked enumerable is different from the expected one. 2 differences found!",
                    "actual[0] = 'l' instead of '.'.", 
                    "actual[1] value ('.') was found at index 1 instead of 0.",
                    "The checked enumerable:", 
                    "\t{*'l'*,'.'} (2 items)",
                    "The expected enumerable:",
	                "\t{*'.'*,' '} (2 items)");
        }

        [Test]
        public void 
            IsEqualToThrowsExceptionWhenFailingWithString()
        {
            var first = "Son of a test";

            Check.ThatCode(() => { Check.That(first).IsEqualTo("no way"); })
                .IsAFailingCheckWithMessage(Environment.NewLine +
                                             "The checked string is different from expected one." +
                                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                                             "\t[\"Son of a test\"]" + Environment.NewLine + "The expected string:" +
                                             Environment.NewLine + "\t[\"no way\"]");
        }

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailingWithString()
        {
            var first = "Son of a test";

            Check.ThatCode(() => { Check.That(first).Not.IsEqualTo(first); })
                .IsAFailingCheckWithMessage(Environment.NewLine +
                                             "The checked string is equal to the given one whereas it must not." +
                                             Environment.NewLine + "The expected string: different from" +
                                             Environment.NewLine + "\t[\"Son of a test\"]");
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenFailingWithObject()
        {
            var heroe = new Person {Name = "Gandhi"};
            var bastard = new Person {Name = "PolPot"};

            Check.ThatCode(() => { Check.That(heroe).IsEqualTo(bastard); })
                .IsAFailingCheckWithMessage("",
                    "The checked value is different from the expected one.",
                    "The checked value:",
                    "\t[Gandhi]",
                    "The expected value:",
                    "\t[PolPot]");
        }

        [Test]
        public void IsEqualToThrowsProperExceptionEvenWithNullAsExpected()
        {
            var first = "Son of a test";

            Check.ThatCode(() => { Check.That(first).IsEqualTo(null); })
                .IsAFailingCheckWithMessage("",
                    "The checked string is not null whereas it must.",
                    "The checked string:",
                    "\t[\"Son of a test\"]",
                    "The expected object:",
                    "\t[null]");
        }

        [Test]
        public void IsEqualToThrowsProperExceptionEvenWithNullAsValue()
        {
            Check.ThatCode(() => Check.That((string) null).IsEqualTo("Kamoulox !"))
                .IsAFailingCheckWithMessage(Environment.NewLine + "The checked string is null whereas it must not." +
                                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                                             "\t[null]" + Environment.NewLine + "The expected string:" +
                                             Environment.NewLine + "\t[\"Kamoulox !\"]");
        }

        [Test]
        public void IsEqualToWorksEvenWithNullStrings()
        {
           Check.That((string) null).IsEqualTo((string)null);
        }       

        [Test]
        public void WeCanSeeTheDifferenceBewteenTwoDifferentObjectsThatHaveTheSameToString()
        {
            var dad = new Person {Name = "John"};
            var son = new Child {Name = "John"};

            Check.ThatCode(() => { Check.That(son).IsEqualTo(dad); })
                .IsAFailingCheckWithMessage("",
                    "The checked value is different from the expected one.",
                    "The checked value:",
                    "\t[John] of type: [NFluent.Tests.Child]",
                    "The expected value:",
                    "\t[John] of type: [NFluent.Tests.Person]");
        }

        [Test]
        public void WeCanAlsoSeeTheDifferenceBetweenTwoDifferentInstancesOfTheSameTypeWhithIdenticalToString()
        {
            var dad = new Person {Name = "John", HashCode = 2};
            var uncle = new Person {Name = "John", HashCode = 3};

            Check.ThatCode(() => { Check.That(uncle).IsEqualTo(dad); })
                .IsAFailingCheckWithMessage("",
                    "The checked value is different from the expected one.",
                    "The checked value:",
                    "\t[John] with HashCode: [3]",
                    "The expected value:",
                    "\t[John] with HashCode: [2]");
        }

        [Test]
        public void WeCanAlsoSeeTheDifferenceBetweenTwoDifferentInstancesWithIdenticalToString()
        {
            var dad = new Person {Name = "John", HashCode = 2};

            Check.ThatCode(() => { Check.That("John").IsEqualTo(dad); })
                .IsAFailingCheckWithMessage("",
                    "The checked string is different from the expected value.",
                    "The checked string:",
                    "\t[\"John\"] of type: [string]",
                    "The expected value:",
                    "\t[John] of type: [NFluent.Tests.Person]");
        }

        #endregion

        #region IsNotEqualTo()

        [Test]
        public void IsNotEqualToWorksWithBooleans()
        {
            const bool tddSucks = false;
            Check.That(tddSucks).IsEqualTo(false);
        }

        [Test]
        public void CanNegateIsNotEqualToWithBooleans()
        {
            const bool tddSucks = false;
            Check.That(tddSucks).Not.IsEqualTo(true);
        }

        [Test]
        public void IsNotEqualToWorksWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsNotEqualTo("other text");
        }

        [Test]
        public void IsNotEqualToWorksWithStringAndObject()
        {
            var first = "Son of a test";
            Check.That(first).IsNotEqualTo((object) "other text");
        }

        [Test]
        public void IsNotEqualToWorksWithArray()
        {
            var array = new[] {45, 43, 54, 666};
            var otherArray = new[] {666, 74};

            Check.That(array).IsNotEqualTo(otherArray);
        }

        [Test]
        public void IsEqualReportsDifferentArrayLength()
        {
            var array = new[] {45, 43, 54, 666};
            var otherArray = new[] {666, 74};

            Check.ThatCode(()=>
            Check.That(array).IsEqualTo(otherArray)).IsAFailingCheckWithMessage("", 
                "The checked enumerable is different from the expected one.", 
                "actual.Dimension(0) = 4 instead of 2.", 
                "The checked enumerable:", 
                "\t{45,43,54,666} (4 items)", 
                "The expected enumerable:", 
                "\t{666,74} (2 items)");
        }

        [Test]
        public void IsEqualToWorksWithArrayAndString()
        {
            // this test prevent regression where `string` is processed like an array of `char`.
            var array = new[] {"thumb", "other"};
            var otherArray = new[] {"test", "other"};

            var otherAsCharArray = new []{new []{'t', 'h', 'u', 'm', 'b'}, new []{'o', 't', 'h', 'e', 'r'}};
            Check.ThatCode(() =>
                    Check.That(array).IsEqualTo(otherAsCharArray)).
                IsAFailingCheckWithMessage("", 
                    "The checked enumerable is different from the expected one. 2 differences found!", 
                    "actual[0] = \"thumb\" instead of {'t','h','u','m','b'}.", 
                    "actual[1] = \"other\" instead of {'o','t','h','e','r'}.", 
                    "The checked enumerable:", 
                    "\t{*\"thumb\"*,\"other\"} (2 items) of type: [string[]]", 
                    "The expected enumerable:", 
                    "\t{*{'t','h','u','m','b'}*,{'o','t','h','e','r'}} (2 items) of type: [char[][]]");
            
            Check.ThatCode(() =>
                    Check.That(otherAsCharArray).IsEqualTo(array)).
                IsAFailingCheckWithMessage("", 
                    "The checked enumerable is different from the expected one. 2 differences found!", 
                    "actual[0] = {'t','h','u','m','b'} instead of \"thumb\".", 
                    "actual[1] = {'o','t','h','e','r'} instead of \"other\".", 
                    "The checked enumerable:", 
                    "\t{*{'t','h','u','m','b'}*,{'o','t','h','e','r'}} (2 items) of type: [char[][]]",
                    "The expected enumerable:", 
                    "\t{*\"thumb\"*,\"other\"} (2 items) of type: [string[]]");
            
            Check.ThatCode(() =>
            Check.That(array).IsEqualTo(otherArray)).IsAFailingCheckWithMessage("", 
                "The checked enumerable is different from the expected one.", 
                "actual[0] = \"thumb\" instead of \"test\".", 
                "The checked enumerable:", 
                "\t{*\"thumb\"*,\"other\"} (2 items)", 
                "The expected enumerable:", 
                "\t{*\"test\"*,\"other\"} (2 items)");
        }

        [Test]
        public void IsNotEqualToWorksObject()
        {
            var hero = new Person {Name = "Gandhi"};
            var badGuy = new Person {Name = "Pol Pot"};

            Check.That(hero).IsNotEqualTo(badGuy);
        }

        [Test]
        public void IsNotEqualWorksWithIntNumbers()
        {
            const int firstInt = 23;
            const int secondButIdenticalInt = 7;

            Check.That(secondButIdenticalInt).IsNotEqualTo(firstInt);
        }

        [Test]
        public void IsNotEqualWorksWithDoubleNumbers()
        {
            const double firstDouble = 23.7D;
            const double secondButIdenticalDouble = 23.75D;

            Check.That(secondButIdenticalDouble).IsNotEqualTo(firstDouble);
        }

        [Test]
        public void IsNotEqualWorksWithFloatNumbers()
        {
            const float firstFloat = 23.56F;
            const float secondButIdenticalFloat = 23.99999F;

            Check.That(secondButIdenticalFloat).IsNotEqualTo(firstFloat);
        }

        [Test]
        public void IsNotEqualWorksWithIntNumberAndObject()
        {
            const int firstInt = 23;
            var obj = new object();
            const byte firstByte = 12;

            Check.That(obj).IsNotEqualTo(firstInt);
            Check.That(firstInt).IsNotEqualTo(obj);

            Check.That(obj).IsNotEqualTo(firstByte);
            Check.That(firstByte).IsNotEqualTo(obj);
        }

        [Test]
        public void IsEqualDealWithNumericalTypeConversion()
        {
            Check.ThatCode(()=>
                Check.That((ushort) 65535).IsEqualTo((sbyte)-1)).IsAFailingCheck();
            Check.ThatCode(()=>
                Check.That((sbyte) -1).IsEqualTo((ushort)65535)).IsAFailingCheck();
            Check.That(-1).IsEqualTo((sbyte) -1);
        }

        [Test]
        public void IsNotEqualToThrowsExceptionWithClearStatusWhenFails()
        {
            const string first = "Son of a test";
            const string otherReferenceToSameObject = first;

            Check.ThatCode(() => { Check.That(first).IsNotEqualTo(otherReferenceToSameObject); })
                .IsAFailingCheckWithMessage(Environment.NewLine +
                                             "The checked string is equal to the given one whereas it must not." +
                                             Environment.NewLine + "The expected string: different from" +
                                             Environment.NewLine + "\t[\"Son of a test\"]");
        }

        [Test]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const string first = "Son of a test";

            Check.ThatCode(() => { Check.That(first).Not.IsNotEqualTo("what?"); })
                .IsAFailingCheckWithMessage(Environment.NewLine +
                                             "The checked string is different from expected one." +
                                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                                             "\t[\"Son of a test\"]" + Environment.NewLine + "The expected string:" +
                                             Environment.NewLine + "\t[\"what?\"]");
        }

        #endregion

        #region Equals should always throw

        [Test]
        public void EqualsWorksAsAnAssertion()
        {
            var obj = new object();

            Check.That(obj).Equals(obj);

            Check.That(Check.That(obj).Equals(obj)).IsFalse();
        }

        [Test]
        public void NotEqualsWorksToo()
        {
            var obj = new object();
            var other = new object();

            Check.That(obj).Not.Equals(other);
        }

        [Test]
        public void EqualsThrowsExceptionWhenFailing()
        {
            var question = "What is the question?";
            var magicNumber = 42;

            Check.ThatCode(() => { Check.That(question).Equals(magicNumber); })
                .IsAFailingCheckWithMessage(Environment.NewLine +
                                             "The checked string is different from the expected value." +
                                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                                             "\t[\"What is the question?\"] of type: [string]" + Environment.NewLine +
                                             "The expected value:" + Environment.NewLine + "\t[42] of type: [int]");
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionsForDoubleNumber()
        {
            var doubleNumber = 37.2D;

            Check.That(doubleNumber).IsEqualTo(37.2D).And.IsNotEqualTo(40.0D).And.IsNotZero().And.IsStrictlyPositive();
            Check.That(doubleNumber).IsNotEqualTo(40.0D).And.IsEqualTo(37.2D);
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionsForObject()
        {
            var camus = new Person {Name = "Camus"};
            var sartre = new Person {Name = "Sartre"};

            Check.That(camus).IsNotEqualTo(sartre).And.IsInstanceOf<Person>();
            Check.That(sartre).IsEqualTo(sartre).And.IsInstanceOf<Person>();
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionsForString()
        {
            var camus = "Camus";
            var sartre = "Sartre";

            Check.That(camus).IsNotEqualTo(sartre).And.IsInstanceOf<string>();
            Check.That(sartre).IsEqualTo(sartre).And.IsInstanceOf<string>();
        }
        
        [Test]
        public void HandleDoubleRecursion()
        {
            var a = new List<int> {1, 2};
            var recursive = new List<object> {a};
            recursive.Add(recursive);
            var otherRecursive = new List<object> {a};
            var interim = new List<object> {recursive};
            otherRecursive.Add(interim);
            Check.ThatCode(() => 
                Check.That(recursive).IsEqualTo(otherRecursive)
                ).IsAFailingCheckWithMessage("", 
                "The checked enumerable is different from the expected one.", 
                "actual[1] = {{1,2},{{...}}} instead of {{{1,2},{{...}}}}.",
                "The checked enumerable:", 
                "\t{{1,2},*{{...}}*} (2 items)", 
                "The expected enumerable:", 
                "\t{{1,2},*{{{1,2},{{...}}}}*} (2 items)");
        }

        [Test]
        public void HandleSimpleRecursion()
        {
            var a = new List<int> {1, 2};
            var recursive = new List<object> {a};
            var sut = new List<object> {a, a};
            recursive.Add(recursive);

            Check.ThatCode(() => Check.That(sut).IsEqualTo(recursive)).
                IsAFailingCheckWithMessage("", 
                    "The checked enumerable is different from the expected one. 2 differences found!", 
                    "actual[1][0] = 1 instead of {1,2}.", 
                    "actual[1][1] = 2 instead of {{1,2},{{...}}}.",
                    "The checked enumerable:", 
                    "\t{{1,2},*{1,2}*} (2 items)", 
                    "The expected enumerable:", 
                    "\t{{1,2},*{{...}}*} (2 items)");
        }
    }
}