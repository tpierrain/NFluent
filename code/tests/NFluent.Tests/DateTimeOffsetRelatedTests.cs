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
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]", 
                    "The expected date time:", 
                    "\t[2013-01-01T00:00:00.0000000 +02:00:00]");
            Check.ThatCode(()=>
            Check.That(newYears).IsEqualToIgnoringHours(new DateTimeOffset(new DateTime(2013, 1, 2, 12, 0, 0), new TimeSpan(0, 1, 0, 0)))).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring hours). The dates are different.", 
                    "The checked date time:", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]", 
                    "The expected date time: same day", 
                    "\t[2013-01-02T12:00:00.0000000 +01:00:00]");
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
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
        }

        [Test]
        public void IsSameInstantIgnoringHoursRaisesProperMessage()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
            Check.That(newYears).MatchTheSameUtcInstantIgnoringHours(new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, -1, 0, 0)))).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring hours).", 
                    "The checked date time:", 
                "\t[2013-01-01T00:00:00.0000000 +01:00:00]", 
                "The expected date time: same day", 
                "\t[2013-01-01T00:00:00.0000000 -01:00:00]");
        }

        [Test]
        public void IsSameInstantIgnoringHoursRaisesProperMessageWhenNegated()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
            Check.That(newYears).Not.MatchTheSameUtcInstantIgnoringHours(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one (ignoring hours) whereas it must not.", 
                    "The expected date time: different day", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
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
                    "\t[2013-01-01T02:00:00.0000000 +01:00:00]", 
                    "The expected date time: same hour", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");

            Check.ThatCode(()=>
            Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 0, 0), new TimeSpan(0, 2, 0, 0))).
                IsEqualToIgnoringMinutes(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring hours). The offsets are different.", 
                    "The checked date time:", 
                    "\t[2013-01-01T00:00:00.0000000 +02:00:00]", 
                    "The expected date time: same hour", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");

            Check.ThatCode(()=>
            Check.That(new DateTimeOffset(new DateTime(2013, 1, 2, 0, 0, 0), new TimeSpan(0, 1, 0, 0))).
                IsEqualToIgnoringMinutes(newYears)).
            IsAFailingCheckWithMessage("", 
                "The checked date time is not equal to the given one. The dates are different.", 
                "The checked date time:", 
                "\t[2013-01-02T00:00:00.0000000 +01:00:00]", 
                "The expected date time: same hour", 
                "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
        }

        [Test]
        public void IsSameInstantToIgnoringMinutesRaisesProperMessage()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(() =>
                Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 2, 0, 0), new TimeSpan(0, 1, 0, 0)))
                    .MatchTheSameUtcInstantIgnoringMinutes(newYears)).IsAFailingCheckWithMessage("",
                "The checked date time is not equal to the given one (ignoring minutes).",
                "The checked date time:",
                "\t[2013-01-01T02:00:00.0000000 +01:00:00]",
                "The expected date time: same hour",
                "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
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
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
        }

        [Test]
        public void MatchSameInstantIgnoringMinutesRaisesProperMessageWhenNegated()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(newYears).Not.MatchTheSameUtcInstantIgnoringMinutes(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one (ignoring minutes) whereas it must not.", 
                    "The expected date time: different hour", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
        }

        [Test]
        public void IsSameInstantToIgnoringSecondsRaisesProperMessage()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 1, 0), new TimeSpan(0, 1, 0, 0))).
                        MatchTheSameUtcInstantIgnoringSeconds(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring seconds).", 
                    "The checked date time:", 
                    "\t[2013-01-01T00:01:00.0000000 +01:00:00]", 
                    "The expected date time: same minute", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
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
                    "\t[2013-01-01T00:01:00.0000000 +01:00:00]", 
                    "The expected date time: same time up to the same minute", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 2, 0, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringSeconds(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring seconds). The time of day is different (Hours).", 
                    "The checked date time:", 
                    "\t[2013-01-01T02:00:00.0000000 +01:00:00]", 
                    "The expected date time: same time up to the same minute", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 0, 0), new TimeSpan(0, 2, 0, 0))).
                        IsEqualToIgnoringSeconds(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring seconds). The offsets are different.", 
                    "The checked date time:", 
                    "\t[2013-01-01T00:00:00.0000000 +02:00:00]", 
                    "The expected date time: same time up to the same minute", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 2, 0, 0, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringSeconds(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring seconds). The dates are different.", 
                    "The checked date time:", 
                    "\t[2013-01-02T00:00:00.0000000 +01:00:00]", 
                    "The expected date time: same time up to the same minute", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
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
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
        }
        
        [Test]
        public void IsSameInstantIgnoringSecondsRaisesProperMessageNegated()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(newYears).Not.MatchTheSameUtcInstantIgnoringSeconds(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one (ignoring seconds) whereas it must not.", 
                    "The expected date time: different minute", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
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
                    "\t[2013-01-01T00:00:01.0000000 +01:00:00]", 
                    "The expected date time: same second", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 1, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring milliseconds). The time of day is different (Minutes).", 
                    "The checked date time:", 
                    "\t[2013-01-01T00:01:00.0000000 +01:00:00]", 
                    "The expected date time: same second", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 2, 0, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring milliseconds). The time of day is different (Hours).", 
                    "The checked date time:", 
                    "\t[2013-01-01T02:00:00.0000000 +01:00:00]", 
                    "The expected date time: same second", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 0, 0), new TimeSpan(0, 2, 0, 0))).
                        IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring milliseconds). The offsets are different.", 
                    "The checked date time:", 
                    "\t[2013-01-01T00:00:00.0000000 +02:00:00]", 
                    "The expected date time: same second", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");

            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 2, 0, 0, 0), new TimeSpan(0, 1, 0, 0))).
                        IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring milliseconds). The dates are different.", 
                    "The checked date time:", 
                    "\t[2013-01-02T00:00:00.0000000 +01:00:00]", 
                    "The expected date time: same second", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
        }

        [Test]
        public void MatchInstantIgnoringMillisRaisesProperMessage()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(new DateTimeOffset(new DateTime(2013, 1, 1, 0, 0, 1), new TimeSpan(0, 1, 0, 0))).
                        MatchTheSameUtcInstantIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is not equal to the given one (ignoring seconds).", 
                    "The checked date time:", 
                    "\t[2013-01-01T00:00:01.0000000 +01:00:00]", 
                    "The expected date time: same second", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
        }

        [Test]
        public void IsEqualToIgnoringMillisRaisesProperMessageWhenNegated()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(newYears).Not.IsEqualToIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one (ignoring milliseconds) whereas it must not.", 
                    "The expected date time: different second", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
        }

        [Test]
        public void MatchInstantMillisRaisesProperMessagewhenNegated()
        {
            var newYears = new DateTimeOffset(new DateTime(2013, 1, 1), new TimeSpan(0, 1, 0, 0));
            Check.ThatCode(()=>
                    Check.That(newYears).Not.MatchTheSameUtcInstantIgnoringMillis(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one (ignoring milliseconds) whereas it must not.", 
                    "The expected date time: different second", 
                    "\t[2013-01-01T00:00:00.0000000 +01:00:00]");
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
                    "\t[2013-01-01T01:01:01.0010000 +00:00:00]",
                    "The expected date time: same time", 
                    "\t[2013-01-01T02:01:01.0010000 +00:00:00]");
            Check.ThatCode( ()=>
            Check.That(newYears).Not.
                MatchTheSameUtcInstant(newYears)).
                IsAFailingCheckWithMessage("", 
                    "The checked date time is equal to the given one whereas it must not.",
                    "The expected date time: different time", 
                    "\t[2013-01-01T01:01:01.0010000 +00:00:00]");
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