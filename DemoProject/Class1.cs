namespace DemoProject
{
    public enum Nationality
    {
        Unknown,
        French,
        Korean,
        Indian
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Nationality Nationality { get; set; }
    }
}
