namespace NFluent.Tests
{
    /// <summary>
    /// Dummy class for unit testing purpose.
    /// </summary>
    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public Nationality Nationality { get; set; }

        // ReSharper disable UnusedMember.Local
        private string PrivateHesitation
        {
            // ReSharper restore UnusedMember.Local
            get
            {
                return "Kamoulox !";
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
