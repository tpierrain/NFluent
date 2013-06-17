namespace NFluent.Tests
{
    using System.Collections;

    using NUnit.Framework;

    [TestFixture]
    public class ToEnumeratedStringTests
    {
        [Test]
        public void ToEnumeratedStringParticularBehaviourWithStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Check.That(guitarHeroes.ToEnumeratedString()).IsEqualTo(@"""Hendrix"", ""Paco de Lucia"", ""Django Reinhardt"", ""Baden Powell""");
        }

        [Test]
        public void ToEnumeratedStringWorksFineWithStrings()
        {
            var departments = new[] { 93, 56, 35, 75 };
            Check.That(departments.ToEnumeratedString()).IsEqualTo("93, 56, 35, 75");
        }

        [Test]
        public void ToEnumeratedStringWorksWithAnotherSeparator()
        {
            var departments = new[] { 93, 56, 35, 75 };

            Check.That(departments.ToEnumeratedString("|")).IsEqualTo("93|56|35|75");
        }

        [Test]
        public void HowToEnumeratedStringHandlesNull()
        {
            var variousStuffs = new ArrayList() { 93, null, "hell yeah!" };

            Check.That(variousStuffs.ToEnumeratedString()).IsEqualTo(@"93, null, ""hell yeah!""");
        }

        [Test]
        public void HowToEnumeratedStringHandlesNullEnumeration()
        {
            Check.That(((IEnumerable)null).ToEnumeratedString()).IsEqualTo("null");
        }
    }
}