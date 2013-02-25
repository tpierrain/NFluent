namespace NFluent
{
    using System;

    internal class FluentString : IFluentString
    {
        private string wrappedString;

        public FluentString(string value)
        {
            this.wrappedString = value;
        }

        public void StartsWith(string expectedPrefix)
        {
            if (!this.wrappedString.StartsWith(expectedPrefix))
            {
                throw new FluentAssertionException(String.Format(@"The string [""{0}""] does not start with [""{1}""].", this.wrappedString, expectedPrefix));
            }
        }
    }
}