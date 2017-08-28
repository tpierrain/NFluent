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
        /// Checks if the given dynamic is null.
        /// </summary>
        public void IsNotNull()
        {
            if (this.value != null)
            {
                return;
            }

            var message = FluentMessage.BuildMessage("The {0} is null whereas it must not.").For("dynamic").On(this.value);
            throw new FluentCheckException(message.ToString());
        }

        /// <summary>
        /// Checks if the given dynamic has the expected reference.
        /// </summary>
        /// <param name="expected">Expected reference.</param>
        public void IsSameReferenceAs(dynamic expected)
        {
            if (object.ReferenceEquals(this.value, expected))
            {
                return;
            }

            var message = FluentMessage.BuildMessage("The {0} is not the expected reference.").For("dynamic").Expected(expected).And.On(this.value);
            throw new FluentCheckException(message.ToString());
        }

        /// <summary>
        /// Checks if the given dynamic has the expected value.
        /// </summary>
        /// <param name="expected">The expected value. Comparison is done using <see cref="object.Equals(object, object)"/></param>
        public void IsEqualTo(dynamic expected)
        {
            if (object.Equals(this.value, expected))
            {
                return;
            }
            var message = FluentMessage.BuildMessage("The {0} is not equal to the {1}.").For("dynamic").Expected(expected).And.On(this.value);
            throw new FluentCheckException(message.ToString());
        }
    }
#endif
}