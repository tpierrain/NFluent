// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DateTimeRelatedTests.cs" company="">
// //   Copyright 2014 Marc-Antoine LATOUR, Thomas PIERRAIN, Cyrille DUPUYDAUBY
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
    using Extensions;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class DateTimeRelatedTests
    {
        [Test]
        public void IsBeforeWorks()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.That(christmas2013).IsBefore(newYearsEve2014);
            Check.That(christmas2013).IsBeforeOrEqualTo(newYearsEve2014);
            Check.That(christmas2013).IsBeforeOrEqualTo(christmas2013);
            Check.That(christmas2013).IsBeforeOrEqualTo(new DateTime(2013, 12, 25));
        }

        [Test]
        public void CanNegateIsBefore()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.That(newYearsEve2014).Not.IsBefore(christmas2013);
            Check.That(newYearsEve2014).Not.IsBeforeOrEqualTo(christmas2013);
        }

        [Test]
        public void NullableDatTimesAreSupported()
        {
            Check.That((DateTime?)null).IsEqualTo(null);
        }

        [Test]
        public void NotIsBeforeMayThrowException()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.ThatCode(() =>
            {
                Check.That(christmas2013).Not.IsBefore(newYearsEve2014);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is before the given one whereas it must not.",
                    "The checked date time:", "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]", 
                    "The expected date time: after or equal", "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void NotIsBeforeOrEqualToMayThrowException()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.ThatCode(() =>
            {
                Check.That(christmas2013).Not.IsBeforeOrEqualTo(newYearsEve2014);
            })
            .IsAFailingCheckWithMessage(
                    "", "The checked date time is before the given one whereas it must not.",
                    "The checked date time:",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: after",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]");
            Check.ThatCode(() =>
            {
                Check.That(christmas2013).Not.IsBeforeOrEqualTo(christmas2013);
            })
            .IsAFailingCheckWithMessage(
                    "", "The checked date time is equal to the given one whereas it must not.",
                    "The checked date time:",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: after",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsBeforeThrowsExceptionWhenNotBefore()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.ThatCode(() =>
            {
                Check.That(newYearsEve2014).IsBefore(christmas2013);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is not before the expected one.",
                    "The checked date time:",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]", 
                    "The expected date time: before",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsBeforeThrowsExceptionWhenSame()
        {
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.ThatCode(() =>
                {
                    Check.That(newYearsEve2014).IsBefore(newYearsEve2014);
                })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not before the expected one.",
                    "The checked date time:",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]", 
                    "The expected date time: before",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsBeforeOrEqualThrowsExceptionWhenNotBeforeOrEqual()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.ThatCode(() =>
            {
                Check.That(newYearsEve2014).IsBeforeOrEqualTo(christmas2013);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is not before or equal to the given one.",
                    "The checked date time:",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: before or equal",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsAfterWorks()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.That(newYearsEve2014).IsAfter(christmas2013);
            Check.That(newYearsEve2014).IsAfterOrEqualTo(newYearsEve2014);
        }

        [Test]
        public void CanNegateIsAfter()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.That(christmas2013).Not.IsAfter(newYearsEve2014);
            Check.That(christmas2013).Not.IsAfterOrEqualTo(newYearsEve2014);
        }

        [Test]
        public void NotIsAfterMayThrowExceptions()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);


            Check.ThatCode(() =>
            {
                Check.That(newYearsEve2014).Not.IsAfter(christmas2013);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is after the given one whereas it must not.",
                    "The checked date time:",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: before or equal",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void NotIsAfterOrEqualToMayThrowExceptions()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);


            Check.ThatCode(() =>
            {
                Check.That(newYearsEve2014).Not.IsAfterOrEqualTo(christmas2013);
            }).
            IsAFailingCheckWithMessage("",
                    "The checked date time is after the given one whereas it must not.",
                    "The checked date time:",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: before",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsAfterThrowsExceptionWhenNotAfter()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.ThatCode(() =>
            {
                Check.That(christmas2013).IsAfter(newYearsEve2014);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is not after the given one.",
                    "The checked date time:",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: after",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsAfterThrowsExceptionWhenSame()
        {
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.ThatCode(() =>
                {
                    Check.That(newYearsEve2014).IsAfter(newYearsEve2014);
                })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not after the given one.",
                    "The checked date time:",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: after",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsAfterOrEqualThrowsExceptionWhenNotAfterOrEqual()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);

            Check.ThatCode(() =>
            {
                Check.That(christmas2013).IsAfterOrEqualTo(newYearsEve2014);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is not after or equal to the given one.",
                    "The checked date time:",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: after or equal",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToWorksWhenMixingDateKind()
        {
            var christmasUtc2013 = new DateTime(2013, 12, 25, 0, 0, 0, DateTimeKind.Utc);
            var christmas2013WithUnspecifiedDateKindExplicit = new DateTime(2013, 12, 25, 0, 0, 0, DateTimeKind.Unspecified);
            var christmas2013WithUnspecifiedDateKindImplicit = new DateTime(2013, 12, 25);
            var christmasInLocalTime2013 = new DateTime(2013, 12, 25, 0, 0, 0, DateTimeKind.Local);

            Check.That(christmasUtc2013).IsEqualTo(christmas2013WithUnspecifiedDateKindExplicit).And
                                        .IsEqualTo(christmas2013WithUnspecifiedDateKindImplicit).And
                                        .IsEqualTo(christmasInLocalTime2013);
        }
         
        [Test]
        public void IsEqualToThrowsExceptionWhenNotEqual()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var newYearsEve2014 = new DateTime(2013, 12, 31);


            Check.ThatCode(() =>
            {
                Check.That(christmas2013).IsEqualTo(newYearsEve2014);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is different from the expected one.",
                    "The checked date time:",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time:",
                    "\t[2013-12-31T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenTypeDiffer()
        {
            var christmas2013 = new DateTime(2013, 12, 25);

            Check.ThatCode(() =>
            {
                Check.That(christmas2013).IsEqualTo("Batman");
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is different from the expected string.",
                    "The checked date time:",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified] of type: [System.DateTime]",
                    "The expected string:",
                    "\t[\"Batman\"] of type: [string]");
        }

        [Test]
        public void NotIsEqualToThrowsExceptionWhenAreEqual()
        {
            var christmas2013 = new DateTime(2013, 12, 25);

            Check.ThatCode(() =>
            {
                Check.That(christmas2013).Not.IsEqualTo(christmas2013);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is equal to the given one whereas it must not.",
                    "The expected date time: different from",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified] of type: [System.DateTime]");
        }

        [Test]
        public void IsNotEqualToThrowsExceptionWhenEqual()
        {
            var christmas2013 = new DateTime(2013, 12, 25);
            var anotherVersionOfChristmas2013 = new DateTime(2013, 12, 25);

            Check.ThatCode(() =>
            {
                Check.That(christmas2013).IsNotEqualTo(anotherVersionOfChristmas2013);
            })
            .IsAFailingCheckWithMessage("",
                     "The checked date time is equal to the expected one whereas it must not.",
                    "The expected date time: different from",
                    "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified] of type: [System.DateTime]");
        }

        [Test]
        public void IsInstanceOfThrowsExceptionWhenNotInstanceOf()
        {
            var christmas2013 = new DateTime(2013, 12, 25);

            Check.ThatCode(() =>
            {
                Check.That(christmas2013).IsInstanceOf<string>();
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is not an instance of [string].",
                    "The checked date time:",
                     "\t[2013-12-25T00:00:00.0000000, Kind = Unspecified] of type: [System.DateTime]",
                    "The expected value:",
                    "\tan instance of [string]");
        }

        [Test]
        public void IsNotInstanceOfThrowsExceptionWhenInstanceOf()
        {
            var actual = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsNotInstanceOf<DateTime>();
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is an instance of [System.DateTime] whereas it must not.",
                    "The checked date time:",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified] of type: [System.DateTime]",
                    "The expected value: different from",
                    "\tan instance of [System.DateTime]");
        }

        [Test]
        public void NotIsNotInstanceOfThrowsExceptionWhenIsNotAnInstanceOf()
        {
            var actual = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(actual).Not.IsNotInstanceOf<string>();
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is not an instance of [string].",
                    "The checked date time:",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified] of type: [System.DateTime]",
                    "The expected value:",
                    "\tan instance of [string]");
        }

        [Test]
        public void IsEqualToIgnoringHoursThrowsExceptionWhenDayDiffer()
        {
            var actual = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringHours(new DateTime(2013, 1, 2));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring hours).",
                    "The checked date time:",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: same day",
                    "\t[2013-01-02T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringHoursThrowsExceptionWhenMonthDiffer()
        {
            var actual = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringHours(new DateTime(2013, 2, 1));
            })
                .IsAFailingCheckWithMessage("",
            "The checked date time is not equal to the given one (ignoring hours).",
            "The checked date time:",
            "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
            "The expected date time: same day",
            "\t[2013-02-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringHoursThrowsExceptionWhenYearDiffer()
        {
            var actual = new DateTime(2013, 1, 1);


            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringHours(new DateTime(2014, 1, 1));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring hours).",
                    "The checked date time:",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: same day",
                    "\t[2014-01-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringMinutesThrowsExceptionWhenDayDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2013, 1, 2, 1, 1, 0, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring minutes).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same hour",
                    "\t[2013-01-02T01:01:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringMinutesThrowsExceptionWhenMonthDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);
            
            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2013, 2, 1, 1, 1, 0, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring minutes).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same hour",
                    "\t[2013-02-01T01:01:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringMinutesThrowsExceptionWhenYearDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2014, 1, 1, 1, 1, 0, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring minutes).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same hour",
                    "\t[2014-01-01T01:01:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringMinutesThrowsExceptionWhenHourDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2013, 1, 1, 2, 1, 0, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring minutes).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same hour",
                    "\t[2013-01-01T02:01:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringSecondsThrowsExceptionWhenMinutesDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 1, 1, 2, 1, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring seconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same minute",
                    "\t[2013-01-01T01:02:01.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringSecondsThrowsExceptionWhenHoursDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 1, 2, 1, 1, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring seconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same minute",
                    "\t[2013-01-01T02:01:01.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringSecondsThrowsExceptionWhenDayDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 2, 1, 1, 1, 0));
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring seconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same minute",
                    "\t[2013-01-02T01:01:01.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringSecondsThrowsExceptionWhenMonthDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 2, 1, 1, 1, 1, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring seconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same minute",
                    "\t[2013-02-01T01:01:01.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringSecondsThrowsExceptionWhenYearDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2014, 1, 1, 1, 1, 1, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring seconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same minute",
                    "\t[2014-01-01T01:01:01.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringMillisThrowsExceptionWhenSecondsDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 1, 1, 5, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring milliseconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same second",
                    "\t[2013-01-01T01:01:05.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringMillisThrowsExceptionWhenMinutesDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 1, 5, 1, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring milliseconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same second",
                    "\t[2013-01-01T01:05:01.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringMillisThrowsExceptionWhenHoursDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 5, 1, 1, 0));
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring milliseconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same second",
                    "\t[2013-01-01T05:01:01.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringMillisThrowsExceptionWhenDaysDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 5, 1, 1, 1, 0));
            })
            .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring milliseconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same second",
                    "\t[2013-01-05T01:01:01.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringMillisThrowsExceptionWhenMonthsDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 2, 1, 1, 1, 1, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring milliseconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same second",
                    "\t[2013-02-01T01:01:01.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToIgnoringMillisThrowsExceptionWhenYearsDiffer()
        {
            var actual = new DateTime(2013, 1, 1, 1, 1, 0);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2014, 1, 1, 1, 1, 1, 0));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is not equal to the given one (ignoring milliseconds).",
                    "The checked date time:",
                    "\t[2013-01-01T01:01:00.0000000, Kind = Unspecified]",
                    "The expected date time: same second",
                    "\t[2014-01-01T01:01:01.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsInSameYearAsThrowsExceptionWhenYearDiffer()
        {
            var actual = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsInSameYearAs(new DateTime(2014, 1, 1));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time has a different year than the given one (actual: 2013, expected: 2014)",
            "The checked date time:",
            "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
            "The expected date time: same year",
                    "\t[2014-01-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void NegatedIsInSameYearAsThrowsException()
        {
            var actual = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(actual).Not.IsInSameYearAs(new DateTime(2013, 1, 1));
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time has the same year as the given one whereas it must not.",
                    "The checked date time:",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: different year",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsInSameMonthAsWorks()
        {
            var actual = new DateTime(1905, 12, 1);
            Check.That(actual).IsInSameMonthAs(new DateTime(2013, 12, 1));
        }

        [Test]
        public void CanNegateIsInSameMonthAs()
        {
            var actual = new DateTime(2013, 12, 1);
            Check.That(actual).Not.IsInSameMonthAs(new DateTime(2013, 1, 1));
        }

        [Test]
        public void IsInSameMonthAsThrowsExceptionWhenMonthDiffer()
        {
            var actual = new DateTime(2013, 12, 1);

            Check.ThatCode(() =>
            {
                Check.That(actual).IsInSameMonthAs(new DateTime(2014, 2, 1));
            }).
                IsAFailingCheckWithMessage("",
                    "The checked date time has a different month than the given one (actual: 12, expected: 2)",
                    "The checked date time:",
                    "\t[2013-12-01T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: same month",
                    "\t[2014-02-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void NotIsInSameMonthAsThrowsExceptionWhenMonthDiffer()
        {
            var actual = new DateTime(2013, 12, 1);

            Check.ThatCode(() =>
            {
                Check.That(actual).Not.IsInSameMonthAs(new DateTime(2014, 12, 1));
            })
           .IsAFailingCheckWithMessage("",
                    "The checked date time has the same month as the given one whereas it must not.",
                    "The checked date time:",
                    "\t[2013-12-01T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: different month",
                    "\t[2014-12-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsInSameDayAsWorks()
        {
            var newYears2013 = new DateTime(2013, 1, 1);
            var newYears2014 = new DateTime(2014, 1, 1);

            Check.That(newYears2013).IsInSameDayAs(newYears2014);
        }

        [Test]
        public void CanNegateIsInSameDayAs()
        {
            var newYears2013 = new DateTime(2013, 1, 1);
            var christmas2013 = new DateTime(2013, 12, 25);

            Check.That(newYears2013).Not.IsInSameDayAs(christmas2013);
        }

        [Test]
        public void IsInSameDayAsThrowsExceptionWhenDayDiffer()
        {
            var newYears = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(newYears).IsInSameDayAs(new DateTime(2014, 2, 2));
            }).
            IsAFailingCheckWithMessage("",
                "The checked date time has a different day than the given one (actual: 1, expected: 2)",
                "The checked date time:",
                "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
                "The expected date time: same day",
                "\t[2014-02-02T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void NotIsInSameDayAsThrowsExceptionWhenDayDiffer()
        {
            var newYears = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(newYears).Not.IsInSameDayAs(new DateTime(1905, 2, 1));
            }).
            IsAFailingCheckWithMessage("",
                    "The checked date time has the same day as the given one whereas it must not.",
                    "The checked date time:",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: different day",
                    "\t[1905-02-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void IsEqualToWorks()
        {
            var newYears = new DateTime(2013, 1, 1);

            Check.That(newYears).IsEqualTo(new DateTime(2013, 1, 1));
             
            Check.That(newYears).IsEqualToIgnoringHours(new DateTime(2013, 1, 1, 1, 1, 1));
            Check.That(newYears).IsEqualToIgnoringMinutes(new DateTime(2013, 1, 1, 0, 1, 1));
            Check.That(newYears).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 1, 0, 0, 1));
            Check.That(newYears).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 0, 0, 0));
             
            Check.That(newYears).IsInSameYearAs(new DateTime(2013, 2, 15));
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            var newYears = new DateTime(2013, 1, 1);
            Check.That(newYears).IsNotEqualTo(new DateTime(1905, 1, 1));
        }

        [Test]
        public void CanNegateIsEqualTo()
        {
            var newYears = new DateTime(2013, 1, 1);

            Check.That(newYears).Not.IsEqualTo(new DateTime(1905, 1, 1));

            Check.That(newYears).Not.IsEqualToIgnoringHours(new DateTime(1905, 1, 1, 1, 1, 1));
            Check.That(newYears).Not.IsEqualToIgnoringMinutes(new DateTime(1905, 1, 1, 0, 1, 1));
            Check.That(newYears).Not.IsEqualToIgnoringSeconds(new DateTime(1905, 1, 1, 0, 0, 1));
            Check.That(newYears).Not.IsEqualToIgnoringMillis(new DateTime(1905, 1, 1, 0, 0, 0));

            Check.That(newYears).Not.IsInSameYearAs(new DateTime(1905, 2, 15));
        }

        [Test]
        public void NotIsEqualToIgnoringMillisMayThrowException()
        {
            var newYears = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(newYears).Not.IsEqualToIgnoringMillis(newYears);
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is equal to the given one (ignoring milliseconds) whereas it must not.",
                    "The checked date time:",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: different second",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void NotIsEqualToIgnoringSecondsMayThrowException()
        {
            var newYears = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(newYears).Not.IsEqualToIgnoringSeconds(newYears);
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is equal to the given one (ignoring seconds) whereas it must not.",
                    "The checked date time:",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: different minute",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void NotIsEqualToIgnoringMinutesMayThrowException()
        {
            var newYears = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(newYears).Not.IsEqualToIgnoringMinutes(newYears);
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is equal to the given one (ignoring minutes) whereas it must not.",
                    "The checked date time:",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: different hour",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void NotIsEqualToIgnoringHoursMayThrowException()
        {
            var newYears = new DateTime(2013, 1, 1);

            Check.ThatCode(() =>
            {
                Check.That(newYears).Not.IsEqualToIgnoringHours(newYears);
            })
                .IsAFailingCheckWithMessage("",
                    "The checked date time is equal to the given one (ignoring hours) whereas it must not.",
                    "The checked date time:",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]",
                    "The expected date time: different day",
                    "\t[2013-01-01T00:00:00.0000000, Kind = Unspecified]");
        }

        [Test]
        public void CanNegateIsNotEqualTo()
        {
            var newYears = new DateTime(2013, 1, 1);
            Check.That(newYears).Not.IsNotEqualTo(newYears);
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnDateTime()
        {
            var newYears = new DateTime(2013, 1, 1);
             
            Check.That(newYears)
                .IsInSameYearAs(new DateTime(2013, 2, 1)).And
                .IsInSameMonthAs(new DateTime(2014, 1, 1)).And
                .IsInSameDayAs(new DateTime(2014, 1, 1)).And
                .IsBefore(new DateTime(2014, 1, 1)).And
                .IsAfter(new DateTime(2012, 1, 1)).And
                .IsEqualToIgnoringHours(new DateTime(2013, 1, 1, 1, 0, 0)).And
                .IsNotInstanceOf<string>().And
                .IsInstanceOf<DateTime>().And
                .IsNotEqualTo("Batman").And.IsNotEqualTo(new DateTime(2014));
        }
        
        [Test]
        public void IsCloseToTimeSpanWorks()
        {
            var reference = DateTime.UtcNow;
            
            Check.That(reference).IsCloseTo(reference.AddMilliseconds(1), TimeSpan.FromMilliseconds(1));
            Check.That(reference).IsCloseTo(reference.AddMilliseconds(-1), TimeSpan.FromMilliseconds(1));
        }
        
        [Test]
        public void IsCloseToDurationWorks()
        {
            var reference = DateTime.UtcNow;
            
            Check.That(reference).IsCloseTo(reference.AddMilliseconds(1), new Duration(1, TimeUnit.Milliseconds));
            Check.That(reference).IsCloseTo(reference.AddMilliseconds(-1), new Duration(1, TimeUnit.Milliseconds));
        }

        [Test]
        public void IsCloseToTimeSpanShouldFailsIfToFar()
        {
            var reference = DateTime.UtcNow;
            var expected = reference.AddMilliseconds(2);
            var within = TimeSpan.FromMilliseconds(1);

            Check.ThatCode(() => Check.That(reference).IsCloseTo(expected, within))
                 .IsAFailingCheckWithMessage("",
                                             "The checked date time is outside the expected value range.",
                                             "The checked date time:",
                                             "\t[" + reference.ToStringProperlyFormatted() + "]",
                                             "The expected value:",
                                             "\t[" + expected + " (+/- 1000 Microseconds)]");
        }
        
        [Test]
        public void IsCloseToDurationShouldFailsIfToFar()
        {
            var reference = DateTime.UtcNow;
            var expected = reference.AddMilliseconds(2);
            var within = new Duration(1, TimeUnit.Milliseconds);

            Check.ThatCode(() => Check.That(reference).IsCloseTo(expected, within))
                 .IsAFailingCheckWithMessage("",
                                             "The checked date time is outside the expected value range.",
                                             "The checked date time:",
                                             "\t[" + reference.ToStringProperlyFormatted() + "]",
                                             "The expected value:",
                                             "\t[" + expected + " (+/- 1 Milliseconds)]");
        }

#if !DOTNET_30 && !DOTNET_20
        [Test]
        public void CanProperlyCompareUtcAndLocalDateTime()
        {
            var now = DateTime.Now;
            DateTime tokyoDateTime = now;
            try
            {
                tokyoDateTime = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"));
            }
            catch
#if !NETCOREAPP1_0 && !NETCOREAPP1_1

            (TimeZoneNotFoundException)
#endif
            {
                // this test works on Windows only
                // we assume we are not on Windows if the timezone is not found
                Assert.Ignore("Test depends on Windows");
            }

            var utcVersionDateTime = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.Utc);

            Check.That(tokyoDateTime).IsNotEqualTo(utcVersionDateTime);
            Check.That(utcVersionDateTime).IsNotEqualTo(tokyoDateTime);
            
            Check.That(DateTime.Today).IsEqualTo(DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc));
        }
#endif
    }
}