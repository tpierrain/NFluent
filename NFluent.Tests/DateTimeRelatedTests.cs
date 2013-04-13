// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DateTimeRelatedTests.cs" company="">
// //   Copyright 2013 Marc-Antoine LATOUR
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
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-12-31T00:00:00.0000000, Kind = Unspecified] is not before the given one [2013-12-25T00:00:00.0000000, Kind = Unspecified]")]
         public void IsBeforeThrowsExceptionWhenNotBefore()
         {
             var christmas2013 = new DateTime(2013, 12, 25);
             var newYearsEve2014 = new DateTime(2013, 12, 31);
             
             Check.That(newYearsEve2014).IsBefore(christmas2013);
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-12-31T00:00:00.0000000, Kind = Unspecified] is not before or equals to the given one [2013-12-25T00:00:00.0000000, Kind = Unspecified]")]
         public void IsBeforeOrEqualThrowsExceptionWhenNotBeforeOrEqual()
         {
             var christmas2013 = new DateTime(2013, 12, 25);
             var newYearsEve2014 = new DateTime(2013, 12, 31);

             Check.That(newYearsEve2014).IsBeforeOrEqualTo(christmas2013);
         }

         [Test]
         public void IsAfterWorks()
         {
             var christmas2013 = new DateTime(2013, 12, 25);
             var newYearsEve2014 = new DateTime(2013, 12, 31);

             Check.That(newYearsEve2014).IsAfter(christmas2013);
             Check.That(newYearsEve2014).IsAfterOrEqualTo(new DateTime(2013, 12, 31));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-12-25T00:00:00.0000000, Kind = Unspecified] is not after the given one [2013-12-31T00:00:00.0000000, Kind = Unspecified]")]
         public void IsAfterThrowsExceptionWhenNotAfter()
         {
             var christmas2013 = new DateTime(2013, 12, 25);
             var newYearsEve2014 = new DateTime(2013, 12, 31);

             Check.That(christmas2013).IsAfter(newYearsEve2014);
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-12-25T00:00:00.0000000, Kind = Unspecified] is not after or equals to the given one [2013-12-31T00:00:00.0000000, Kind = Unspecified]")]
         public void IsAfterOrEqualThrowsExceptionWhenNotAfterOrEqual()
         {
             var christmas2013 = new DateTime(2013, 12, 25);
             var newYearsEve2014 = new DateTime(2013, 12, 31);

             Check.That(christmas2013).IsAfterOrEqualTo(newYearsEve2014);
         }

         [Test]
         public void IsEqualToWorksWhenMixingDateKind()
         {
             var christmasUtc2013 = new DateTime(2013, 12, 25, 0, 0, 0, DateTimeKind.Utc);
             var christmas2013WithUnspecifiedDateKindExplicit = new DateTime(2013, 12, 25, 0, 0, 0, DateTimeKind.Unspecified);
             var christmas2013WithUnspecifiedDateKindImplicit = new DateTime(2013, 12, 25);
             var christmasInLocalTime2013 = new DateTime(2013, 12, 25, 0, 0, 0, DateTimeKind.Local);

             Check.That(christmasUtc2013)
                 .IsEqualTo(christmas2013WithUnspecifiedDateKindExplicit).And
                 .IsEqualTo(christmas2013WithUnspecifiedDateKindImplicit).And
                 .IsEqualTo(christmasInLocalTime2013);
         }
         
         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "\nWas:\n\t[2013-12-25T00:00:00.0000000, Kind = Unspecified]\n instead of the expected:\n\t[2013-12-31T00:00:00.0000000, Kind = Unspecified].")]
         public void IsEqualToThrowsExceptionWhenNotEqual()
         {
             var christmas2013 = new DateTime(2013, 12, 25);
             var newYearsEve2014 = new DateTime(2013, 12, 31);

             Check.That(christmas2013).IsEqualTo(newYearsEve2014);
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "\nWas:\n\t[2013-12-25T00:00:00.0000000, Kind = Unspecified] of type: [System.DateTime]\n instead of the expected:\n\t[\"Batman\"] of type: [System.String].")]
         public void IsEqualToThrowsExceptionWhenTypeDiffer()
         {
             var christmas2013 = new DateTime(2013, 12, 25);

             Check.That(christmas2013).IsEqualTo("Batman");
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[2013-12-25T00:00:00.0000000, Kind = Unspecified] equals to the value [2013-12-25T00:00:00.0000000, Kind = Unspecified] which is not expected.")]
         public void IsNotEqualToThrowsExceptionWhenEqual()
         {
             var christmas2013 = new DateTime(2013, 12, 25);
             var anotherVersionOfChristmas2013 = new DateTime(2013, 12, 25);

             Check.That(christmas2013).IsNotEqualTo(anotherVersionOfChristmas2013);
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[2013-12-25T00:00:00.0000000, Kind = Unspecified] is not an instance of the expected type [System.String] but of [System.DateTime] instead.")]
         public void IsInstanceOfThrowsExceptionWhenNotInstanceOf()
         {
             var christmas2013 = new DateTime(2013, 12, 25);

             Check.That(christmas2013).IsInstanceOf<string>();
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[2013-01-01T00:00:00.0000000, Kind = Unspecified] is an instance of the type [System.DateTime] which is not expected.")]
         public void IsNotInstanceOfThrowsExceptionWhenInstanceOf()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsNotInstanceOf<DateTime>();
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T00:00:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-01-02T00:00:00.0000000, Kind = Unspecified] ignoring hours")]
         public void IsEqualToIgnoringHoursThrowsExceptionWhenDayDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsEqualToIgnoringHours(new DateTime(2013, 1, 2));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T00:00:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-02-01T00:00:00.0000000, Kind = Unspecified] ignoring hours")]
         public void IsEqualToIgnoringHoursThrowsExceptionWhenMonthDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsEqualToIgnoringHours(new DateTime(2013, 2, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T00:00:00.0000000, Kind = Unspecified] is not equal to the given date time [2014-01-01T00:00:00.0000000, Kind = Unspecified] ignoring hours")]
         public void IsEqualToIgnoringHoursThrowsExceptionWhenYearDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsEqualToIgnoringHours(new DateTime(2014, 1, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-01-02T01:01:00.0000000, Kind = Unspecified] ignoring minutes")]
         public void IsEqualToIgnoringMinutesThrowsExceptionWhenDayDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2013, 1, 2, 1, 1, 0, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-02-01T01:01:00.0000000, Kind = Unspecified] ignoring minutes")]
         public void IsEqualToIgnoringMinutesThrowsExceptionWhenMonthDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2013, 2, 1, 1, 1, 0, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2014-01-01T01:01:00.0000000, Kind = Unspecified] ignoring minutes")]
         public void IsEqualToIgnoringMinutesThrowsExceptionWhenYearDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2014, 1, 1, 1, 1, 0, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-01-01T02:01:00.0000000, Kind = Unspecified] ignoring minutes")]
         public void IsEqualToIgnoringMinutesThrowsExceptionWhenHourDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2013, 1, 1, 2, 1, 0, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-01-01T01:02:01.0000000, Kind = Unspecified] ignoring seconds")]
         public void IsEqualToIgnoringSecondsThrowsExceptionWhenMinutesDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 1, 1, 2, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-01-01T02:01:01.0000000, Kind = Unspecified] ignoring seconds")]
         public void IsEqualToIgnoringSecondsThrowsExceptionWhenHoursDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 1, 2, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-01-02T01:01:01.0000000, Kind = Unspecified] ignoring seconds")]
         public void IsEqualToIgnoringSecondsThrowsExceptionWhenDayDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 2, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-02-01T01:01:01.0000000, Kind = Unspecified] ignoring seconds")]
         public void IsEqualToIgnoringSecondsThrowsExceptionWhenMonthDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 2, 1, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2014-01-01T01:01:01.0000000, Kind = Unspecified] ignoring seconds")]
         public void IsEqualToIgnoringSecondsThrowsExceptionWhenYearDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2014, 1, 1, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-01-01T01:01:05.0000000, Kind = Unspecified] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowsExceptionWhenSecondsDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 1, 1, 5, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-01-01T01:05:01.0000000, Kind = Unspecified] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowsExceptionWhenMinutesDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 1, 5, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-01-01T05:01:01.0000000, Kind = Unspecified] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowsExceptionWhenHoursDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 5, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-01-05T01:01:01.0000000, Kind = Unspecified] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowsExceptionWhenDaysDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 5, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2013-02-01T01:01:01.0000000, Kind = Unspecified] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowsExceptionWhenMonthsDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 2, 1, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000, Kind = Unspecified] is not equal to the given date time [2014-01-01T01:01:01.0000000, Kind = Unspecified] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowsExceptionWhenYearsDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2014, 1, 1, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual year [2013] is not equal to the given date time year [2014]")]
         public void IsInSameYearAsThrowsExceptionWhenYearDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsInSameYearAs(new DateTime(2014, 1, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual month [1] is not equal to the given date time month [2]")]
         public void IsInSameMonthAsThrowsExceptionWhenMonthDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsInSameMonthAs(new DateTime(2014, 2, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual day [1] is not equal to the given date time day [2]")]
         public void IsInSameDayAsThrowsExceptionWhenDayDiffer()
         {
             var newYears = new DateTime(2013, 1, 1);

             Check.That(newYears).IsInSameDayAs(new DateTime(2014, 2, 2));
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
         public void CanProperlyCompareUtcAndLocalDateTime()
         {
             var now = DateTime.Now;
             var nowUtc = now.ToUniversalTime();

             Check.That(now).IsNotEqualTo(nowUtc);
             Check.That(nowUtc).IsNotEqualTo(now);

             Check.That(DateTime.Today).IsEqualTo(DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc));
         }
    }
}