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

        private string PrivatePassword
        {
            get
            {
                return "Kamoulox !";
            }
        }

        public string WhisperPassword()
        {
            return this.PrivatePassword;
        }
    }
}
