namespace NFluent.Tests
{
    /// <summary>
    /// Provides some information about someone's mood. 
    /// </summary>
    public struct Mood
    {
        /// <summary>
        /// Gets or sets a value indicating whether this mood is positive.
        /// </summary>
        /// <value>
        /// <c>true</c> if this mood is positive; otherwise, <c>false</c>.
        /// </value>
        public bool IsPositive { get; set; }

        /// <summary>
        /// Gets or sets the mood description.
        /// </summary>
        /// <value>
        /// The mood description.
        /// </value>
        public string Description { get; set; }
    }
}
