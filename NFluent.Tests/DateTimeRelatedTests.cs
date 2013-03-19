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
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsBefore(new DateTime(2013, 1, 2));
             Check.That(actual).IsBeforeOrEqualTo(new DateTime(2013, 1, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T00:00:00.0000000] is not before the expected [2012-12-01T00:00:00.0000000]")]
         public void IsBeforeThrowExceptionWhenNotBefore()
         {
             var actual = new DateTime(2013, 1, 1);
             
             Check.That(actual).IsBefore(new DateTime(2012, 12, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T02:00:00.0000000] is not before or equal to the expected [2013-01-01T01:00:00.0000000]")]
         public void IsBeforeOrEqualThrowExceptionWhenNotBeforeOrEqual()
         {
             var actual = new DateTime(2013, 1, 1, 2, 0, 0, 0);

             Check.That(actual).IsBeforeOrEqualTo(new DateTime(2013, 1, 1, 1, 0, 0, 0));
         }

         [Test]
         public void IsAfterWorks()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsAfter(new DateTime(2012, 1, 1));
             Check.That(actual).IsAfterOrEqualTo(new DateTime(2013, 1, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T00:00:00.0000000] is not after the expected [2014-12-01T00:00:00.0000000]")]
         public void IsAfterThrowExceptionWhenNotAfter()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsAfter(new DateTime(2014, 12, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:00:00.0000000] is not after or equal to the expected [2013-01-01T02:00:00.0000000]")]
         public void IsAfterOrEqualThrowExceptionWhenNotAfterOrEqual()
         {
             var actual = new DateTime(2013, 1, 1, 1, 0, 0, 0);

             Check.That(actual).IsAfterOrEqualTo(new DateTime(2013, 1, 1, 2, 0, 0, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[2013-01-01T06:00:00.0000000Z] not equals to the expected [2013-01-01T07:00:00.0000000Z]")]
         public void IsEqualToThrowExceptionWhenNotEqual()
         {
             var actual = new DateTime(2013, 1, 1, 1, 0, 0, 0);

             Check.That(actual).IsEqualTo(new DateTime(2013, 1, 1, 2, 0, 0, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[2013-01-01T06:00:00.0000000Z] not equals to the expected [\"Batman\"]")]
         public void IsEqualToThrowExceptionWhenTypeDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 0, 0, 0);

             Check.That(actual).IsEqualTo("Batman");
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[2013-01-01T05:00:00.0000000Z] equals to the value [2013-01-01T05:00:00.0000000Z] which is not expected.")]
         public void IsNotEqualToThrowExceptionWhenEqual()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsNotEqualTo(new DateTime(2013, 1, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[2013-01-01T01:00:00.0000000] is not an instance of the expected type [System.String] but of [System.DateTime] instead.")]
         public void IsInstanceOfThrowExceptionWhenNotInstanceOf()
         {
             var actual = new DateTime(2013, 1, 1, 1, 0, 0, 0);

             Check.That(actual).IsInstanceOf<string>();
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[2013-01-01T00:00:00.0000000] is an instance of the type [System.DateTime] which is not expected.")]
         public void IsNotInstanceOfThrowExceptionWhenInstanceOf()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsNotInstanceOf<DateTime>();
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T00:00:00.0000000] is not equal to the given date time [2013-01-02T00:00:00.0000000] ignoring hours")]
         public void IsEqualToIgnoringHoursThrowExceptionWhenDayDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsEqualToIgnoringHours(new DateTime(2013, 1, 2));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T00:00:00.0000000] is not equal to the given date time [2013-02-01T00:00:00.0000000] ignoring hours")]
         public void IsEqualToIgnoringHoursThrowExceptionWhenMonthDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsEqualToIgnoringHours(new DateTime(2013, 2, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T00:00:00.0000000] is not equal to the given date time [2014-01-01T00:00:00.0000000] ignoring hours")]
         public void IsEqualToIgnoringHoursThrowExceptionWhenYearDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsEqualToIgnoringHours(new DateTime(2014, 1, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-01-02T01:01:00.0000000] ignoring minutes")]
         public void IsEqualToIgnoringMinutesThrowExceptionWhenDayDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2013, 1, 2, 1, 1, 0, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-02-01T01:01:00.0000000] ignoring minutes")]
         public void IsEqualToIgnoringMinutesThrowExceptionWhenMonthDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2013, 2, 1, 1, 1, 0, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2014-01-01T01:01:00.0000000] ignoring minutes")]
         public void IsEqualToIgnoringMinutesThrowExceptionWhenYearDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2014, 1, 1, 1, 1, 0, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-01-01T02:01:00.0000000] ignoring minutes")]
         public void IsEqualToIgnoringMinutesThrowExceptionWhenHourDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2013, 1, 1, 2, 1, 0, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-01-01T01:02:01.0000000] ignoring seconds")]
         public void IsEqualToIgnoringSecondsThrowExceptionWhenMinutesDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 1, 1, 2, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-01-01T02:01:01.0000000] ignoring seconds")]
         public void IsEqualToIgnoringSecondsThrowExceptionWhenHoursDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 1, 2, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-01-02T01:01:01.0000000] ignoring seconds")]
         public void IsEqualToIgnoringSecondsThrowExceptionWhenDayDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 2, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-02-01T01:01:01.0000000] ignoring seconds")]
         public void IsEqualToIgnoringSecondsThrowExceptionWhenMonthDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 2, 1, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2014-01-01T01:01:01.0000000] ignoring seconds")]
         public void IsEqualToIgnoringSecondsThrowExceptionWhenYearDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2014, 1, 1, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-01-01T01:01:05.0000000] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowExceptionWhenSecondsDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 1, 1, 5, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-01-01T01:05:01.0000000] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowExceptionWhenMinutesDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 1, 5, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-01-01T05:01:01.0000000] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowExceptionWhenHoursDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 5, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-01-05T01:01:01.0000000] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowExceptionWhenDaysDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 5, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2013-02-01T01:01:01.0000000] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowExceptionWhenMonthsDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 2, 1, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual date time [2013-01-01T01:01:00.0000000] is not equal to the given date time [2014-01-01T01:01:01.0000000] ignoring milliseconds")]
         public void IsEqualToIgnoringMillisThrowExceptionWhenYearsDiffer()
         {
             var actual = new DateTime(2013, 1, 1, 1, 1, 0);

             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2014, 1, 1, 1, 1, 1, 0));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual year [2013] is not equal to the given date time year [2014]")]
         public void HaveSameYearThrowExceptionWhenYearDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).HaveSameYear(new DateTime(2014, 1, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual month [1] is not equal to the given date time month [2]")]
         public void HaveSameMonthThrowExceptionWhenMonthDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).HaveSameMonth(new DateTime(2014, 2, 1));
         }

         [Test]
         [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The actual day [1] is not equal to the given date time day [2]")]
         public void HaveSameDayThrowExceptionWhenDayDiffer()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).HaveSameDay(new DateTime(2014, 2, 2));
         }
         [Test]
         public void IsEqualToWorks()
         {
             var actual = new DateTime(2013, 1, 1);

             Check.That(actual).IsEqualTo(new DateTime(2013, 1, 1));
             
             Check.That(actual).IsEqualToIgnoringHours(new DateTime(2013, 1, 1, 1, 1, 1, 1));
             Check.That(actual).IsEqualToIgnoringMinutes(new DateTime(2013, 1, 1, 0, 1, 1, 1));
             Check.That(actual).IsEqualToIgnoringSeconds(new DateTime(2013, 1, 1, 0, 0, 1, 1));
             Check.That(actual).IsEqualToIgnoringMillis(new DateTime(2013, 1, 1, 0, 0, 0, 1));
             
             Check.That(actual).HaveSameYear(new DateTime(2013, 2, 15));
         }

         [Test]
         public void AndOperatorCanChainMultipleAssertionOnDateTime()
         {
             var actual = new DateTime(2013, 1, 1);
             
             Check.That(actual)
                 .HaveSameYear(new DateTime(2013, 2, 1)).And
                 .HaveSameMonth(new DateTime(2014, 1, 1)).And
                 .HaveSameDay(new DateTime(2014, 1, 1)).And
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

             Check.That(now).IsEqualTo(nowUtc);
             Check.That(nowUtc).IsEqualTo(now);
         }
    }
}