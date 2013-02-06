namespace NFluent.Tests
{
    /// <summary>
    /// Dummy class for unit testing purpose.
    /// </summary>
    internal class Person
    {
        public string Name { get; set; }
        
        public int Age { get; set; }

        public Nationality Nationality { get; set; }

// ReSharper disable UnusedMember.Local
        private string PrivatePassword
// ReSharper restore UnusedMember.Local
        {
            get
            {
                return "Kamoulox !";
            }
        }
    }
}
