namespace NFluent.Tests
{
    using System;
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
        public void NotMatchTheSameUtcInstantWorks()
        {
            var localTime = new DateTime(2013, 1, 1, 10, 2, 0);
            var oneHourOffSet = new DateTimeOffset(localTime, new TimeSpan(7, 0, 0));
            Check.That(oneHourOffSet).IsNotEqualTo(new DateTimeOffset(localTime, new TimeSpan(6, 0, 0)));

            Check.That(oneHourOffSet).Not.MatchTheSameUtcInstantIgnoringMillis(new DateTimeOffset(localTime, new TimeSpan(6, 0, 0)));
        }
    }
}