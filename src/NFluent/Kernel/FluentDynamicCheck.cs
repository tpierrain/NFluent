namespace NFluent
{
    using NFluent.Extensibility;
    using NFluent.Kernel;

#if PORTABLE || NETSTANDARD1_3 || DOTNET_45
    /// <summary>
    /// 
    /// </summary>
    public class FluentDynamicCheck
    {
        private dynamic value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public FluentDynamicCheck(dynamic value)
        {
            this.value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public void IsNotNull()
        {
            if (this.value == null)
            {
                var message = FluentMessage.BuildMessage("The {0} is null whereas it must not").For("dynamic").On(this.value);
                throw new FluentCheckException(message.ToString());
            }
        }
    }
#endif
}