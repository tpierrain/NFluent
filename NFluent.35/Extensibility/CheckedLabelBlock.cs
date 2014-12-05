namespace NFluent.Extensibility
{
    /// <summary>
    /// The actual label block.
    /// </summary>
    internal class CheckedLabelBlock : GenericLabelBlock
    {
        protected override string Adjective()
        {
            return "checked";
        }
    }
}