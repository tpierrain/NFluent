namespace NFluent.Tests
{
    using System;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class DateTimeOffsetRelatedTests
    {
        [Test]
        public void IsEqualToWorks()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));

            Check.That(newYears).IsEqualTo(new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0)));
            Check.That(newYears).IsEqualToIgnoringHours(new DateTimeOffset(new DateTime(2013, 1, 1, 12, 0, 0), new TimeSpan(0,1,0,0)));
            Check.That(newYears).IsEqualToIgnoringMinutes(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 1, 0), new TimeSpan(0,1,0,0)));
            Check.That(newYears).IsEqualToIgnoringSeconds(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 0, 1), new TimeSpan(0,1,0,0)));
            Check.That(newYears).IsEqualToIgnoringMillis(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 0, 0), new TimeSpan(0,1,0,0)));
        }

        [Test]
        public void IsEqualToRaisesProperMessage()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));

            Check.ThatCode(() =>
            Check.That(newYears).IsEqualTo(new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 2, 0, 0)))).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is different from the expected one.", 
                    "The checked date time:", 
                    "\t[01/01/2013 00:00:00 +01:00]", 
                    "The expected date time:", 
                    "\t[01/01/2013 00:00:00 +02:00]");
            Check.ThatCode(()=>
            Check.That(newYears).IsEqualToIgnoringHours(new DateTimeOffset(new DateTime(2013, 1, 2, 12, 0, 0), new TimeSpan(0, 1, 0, 0)))).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring hours). The dates are different.", 
                    "The checked date time:", 
                    "\t[01/01/2013 00:00:00 +01:00]", 
                    "The expected date time: same day", 
                    "\t[02/01/2013 12:00:00 +01:00]");
        }

        [Test]
        public void IsEqualToIgnoringHoursRaisesProperMessageWhenNegated()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
            Check.That(newYears).Not.IsEqualToIgnoringHours(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one (ignoring hours) whereas it must not.", 
                    "The expected date time: different day", 
                    "\t[01/01/2013 00:00:00 +01:00]");
        }

        [Test]
        public void IsEqualToIgnoringMinutesRaisesProperMessage()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
            Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 2, 0, 0), new TimeSpan(0, 1, 0, 0))).
                IsEqualToIgnoringMinutes(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one. The hours are different.", 
                    "The checked date time:", 
                    "\t[01/01/2013 02:00:00 +01:00]", 
                    "The expected date time: same hour", 
                    "\t[01/01/2013 00:00:00 +01:00]");

            Check.ThatCode(()=>
            Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 0, 0), new TimeSpan(0, 2, 0, 0))).
                IsEqualToIgnoringMinutes(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring hours). The offsets are different.", 
                    "The checked date time:", 
                    "\t[01/01/2013 00:00:00 +02:00]", 
                    "The expected date time: same hour", 
                    "\t[01/01/2013 00:00:00 +01:00]");

            Check.ThatCode(()=>
            Check.That(new DateTimeOffset(new DateTime(2013, 1, 2, 0, 0, 0), new TimeSpan(0, 1, 0, 0))).
                IsEqualToIgnoringMinutes(newYears)).
            IsAFailingCheckWithMessage("", 
                "The checked date time is not equal to the given one. The dates are different.", 
                "The checked date time:", 
                "\t[02/01/2013 00:00:00 +01:00]", 
                "The expected date time: same hour", 
                "\t[01/01/2013 00:00:00 +01:00]");
        }

        [Test]
        public void IsEqualToIgnoringMinutesRaisesProperMessageWhenNegated()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(newYears).Not.IsEqualToIgnoringMinutes(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one (ignoring minutes) whereas it must not.", 
                    "The expected date time: different hour", 
                    "\t[01/01/2013 00:00:00 +01:00]");
        }

        [Test]
        public void IsEqualToIgnoringSecondsRaisesProperMessage()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 1, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringSeconds(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring seconds). The time of day is different (Minutes).", 
                    "The checked date time:", 
                    "\t[01/01/2013 00:01:00 +01:00]", 
                    "The expected date time: same time up to the same minute", 
                    "\t[01/01/2013 00:00:00 +01:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 2, 0, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringSeconds(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring seconds). The time of day is different (Hours).", 
                    "The checked date time:", 
                    "\t[01/01/2013 02:00:00 +01:00]", 
                    "The expected date time: same time up to the same minute", 
                    "\t[01/01/2013 00:00:00 +01:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 0, 0), new TimeSpan(0, 2, 0, 0))).
                        IsEqualToIgnoringSeconds(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring seconds). The offsets are different.", 
                    "The checked date time:", 
                    "\t[01/01/2013 00:00:00 +02:00]", 
                    "The expected date time: same time up to the same minute", 
                    "\t[01/01/2013 00:00:00 +01:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 2, 0, 0, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringSeconds(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring seconds). The dates are different.", 
                    "The checked date time:", 
                    "\t[02/01/2013 00:00:00 +01:00]", 
                    "The expected date time: same time up to the same minute", 
                    "\t[01/01/2013 00:00:00 +01:00]");
        }

        [Test]
        public void IsEqualToIgnoringSecondsRaisesProperMessageNegated()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(newYears).Not.IsEqualToIgnoringSeconds(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one (ignoring seconds) whereas it must not.", 
                    "The expected date time: different time up to the same minute", 
                    "\t[01/01/2013 00:00:00 +01:00]");
        }
        
        [Test]
        public void IsEqualToIgnoringMillisRaisesProperMessage()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 0, 1), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring milliseconds). The time of day is different (Seconds).", 
                    "The checked date time:", 
                    "\t[01/01/2013 00:00:01 +01:00]", 
                    "The expected date time: same second", 
                    "\t[01/01/2013 00:00:00 +01:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 1, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring milliseconds). The time of day is different (Minutes).", 
                    "The checked date time:", 
                    "\t[01/01/2013 00:01:00 +01:00]", 
                    "The expected date time: same second", 
                    "\t[01/01/2013 00:00:00 +01:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 2, 0, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring milliseconds). The time of day is different (Hours).", 
                    "The checked date time:", 
                    "\t[01/01/2013 02:00:00 +01:00]", 
                    "The expected date time: same second", 
                    "\t[01/01/2013 00:00:00 +01:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 0, 0), new TimeSpan(0, 2, 0, 0))).
                        IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring milliseconds). The offsets are different.", 
                    "The checked date time:", 
                    "\t[01/01/2013 00:00:00 +02:00]", 
                    "The expected date time: same second", 
                    "\t[01/01/2013 00:00:00 +01:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 2, 0, 0, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring milliseconds). The dates are different.", 
                    "The checked date time:", 
                    "\t[02/01/2013 00:00:00 +01:00]", 
                    "The expected date time: same second", 
                    "\t[01/01/2013 00:00:00 +01:00]");
        }

        [Test]
        public void IsEqualToIgnoringMillisRaisesProperMessageNegated()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(newYears).Not.IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one (ignoring milliseconds) whereas it must not.", 
                    "The expected date time: different second", 
                    "\t[01/01/2013 00:00:00 +01:00]");
        }

        [Test]
        public void IsNotEqualToWorksWhenOffSetIsDifferent()
        {
            var localTime = new DateTime(2013, 1, 1, 10, 2, 0);
            var oneHourOffSet = new DateTimeOffset(localTime, new TimeSpan(7, 0, 0));

            Check.That(oneHourOffSet).IsNotEqualTo(new DateTimeOffset(localTime, new TimeSpan(6, 0, 0)));
            
            Check.That(oneHourOffSet).Not.IsEqualToIgnoringHours(new DateTimeOffset(localTime, new TimeSpan(0,13,0,0)));
            Check.That(oneHourOffSet).Not.IsEqualToIgnoringMinutes(new DateTimeOffset(localTime, new TimeSpan(0,1,0,0)));
            Check.That(oneHourOffSet).Not.IsEqualToIgnoringSeconds(new DateTimeOffset(localTime, new TimeSpan(0,1,0,0)));
            Check.That(oneHourOffSet).Not.IsEqualToIgnoringMillis(new DateTimeOffset(localTime, new TimeSpan(0,1,0,0)));
        }

        [Test]
        public void IsNotEqualToWorksWhenTimeIsDifferent()
        {
            var localTime = new DateTime(2013, 1, 1, 10, 2, 0);
            var oneHourOffSet = new DateTimeOffset(localTime, new TimeSpan(7, 0, 0));

            Check.That(oneHourOffSet).IsNotEqualTo(new DateTimeOffset(localTime, new TimeSpan(6, 0, 0)));
            
            Check.That(oneHourOffSet).Not.IsEqualToIgnoringHours(new DateTimeOffset(new DateTime(2013, 1, 2, 12, 0, 0), new TimeSpan(0,13,0,0)));
            Check.That(oneHourOffSet).Not.IsEqualToIgnoringMinutes(new DateTimeOffset(new DateTime(2013, 1, 1, 1, 1, 0), new TimeSpan(0,13,0,0)));
            Check.That(oneHourOffSet).Not.IsEqualToIgnoringSeconds(new DateTimeOffset(new DateTime(2013, 1, 1, 1, 0, 1), new TimeSpan(0,13,0,0)));
            Check.That(oneHourOffSet).Not.IsEqualToIgnoringMillis(new DateTimeOffset(new DateTime(2013, 1, 1, 1, 0, 0), new TimeSpan(0,13,0,0)));
        }

        [Test]
        public void MatchTheSameUtcInstantWorks()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1, 1, 1, 1, 1), TimeSpan.Zero);

            Check.That(newYears).MatchTheSameUtcInstant(new DateTimeOffset(new DateTime(2013, 1, 1, 1, 1, 1, 1), TimeSpan.Zero));
            Check.That(newYears).MatchTheSameUtcInstantIgnoringHours(new DateTimeOffset(new DateTime(2013, 1, 1, 14, 1, 1, 1), new TimeSpan(0, 13, 0, 0)));
            Check.That(newYears).MatchTheSameUtcInstantIgnoringMinutes(new DateTimeOffset(new DateTime(2013, 1, 1, 2, 5, 1, 1), new TimeSpan(0, 1, 0, 0)));
            Check.That(newYears).MatchTheSameUtcInstantIgnoringSeconds(new DateTimeOffset(new DateTime(2013, 1, 1, 2, 1, 5, 1), new TimeSpan(0, 1, 0, 0)));
            Check.That(newYears).MatchTheSameUtcInstantIgnoringMillis(new DateTimeOffset(new DateTime(2013, 1, 1, 2, 1, 1, 5), new TimeSpan(0, 1, 0, 0)));
        }

        [Test]
        public void MatchTheSameUtcInstantFailsWithProperMessage()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1, 1, 1, 1, 1), TimeSpan.Zero);

            Check.ThatCode( ()=>
            Check.That(newYears).
                MatchTheSameUtcInstant(new DateTimeOffset(new DateTime(2013, 1, 1, 2, 1, 1, 1), TimeSpan.Zero))).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one.", 
                    "The checked date time:", 
                    "\t[01/01/2013 01:01:01 +00:00]",
                    "The expected date time: same time", 
                    "\t[01/01/2013 02:01:01 +00:00]");
            Check.ThatCode( ()=>
            Check.That(newYears).Not.
                MatchTheSameUtcInstant(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one whereas it must not.",
                    "The expected date time: different time", 
                    "\t[01/01/2013 01:01:01 +00:00]");
        }

        [Test]
        public void NotMatchTheSameUtcInstantWorks()
        {
            var localTime = new DateTime(2013, 1, 1, 10, 2, 0);
            var oneHourOffSet = new DateTimeOffset(localTime, new TimeSpan(7, 0, 0));
            Check.That(oneHourOffSet).IsNotEqualTo(new DateTimeOffset(localTime, new TimeSpan(6, 0, 0)));

            Check.That(oneHourOffSet).Not.MatchTheSameUtcInstantIgnoringMillis(new DateTimeOffset(localTime, new TimeSpan(6, 0, 0)));
        }
    }
}