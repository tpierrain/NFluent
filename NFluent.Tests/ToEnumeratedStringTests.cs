namespace NFluent.Tests
{
    using NUnit.Framework;

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
            var guitarHeroes = new[] { 93, 56, 35, 75 };
            Check.That(guitarHeroes.ToEnumeratedString()).IsEqualTo("93, 56, 35, 75");
        }
    }
}