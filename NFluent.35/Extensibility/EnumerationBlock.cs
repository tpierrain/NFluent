namespace NFluent.Extensibility
{
    using System;
    using System.Collections;
    using System.Text;

    using NFluent.Extensions;

    /// <summary>
    /// Class describing a value block.
    /// </summary>
    internal class EnumerationBlock : IValueDescription
    {
        #region Fields

        /// <summary>
        /// The tested object.
        /// </summary>
        private readonly IEnumerable test;

        /// <summary>
        /// The enumerable count.
        /// </summary>
        private long? enumerableCount;

        /// <summary>
        /// The full type name.
        /// </summary>
        private bool fullTypeName;

        /// <summary>
        /// The include hash.
        /// </summary>
        private bool includeHash;

        /// <summary>
        /// The include type.
        /// </summary>
        private bool includeType;

        /// <summary>
        /// The instance type.
        /// </summary>
        private Type type;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationBlock"/> class.
        /// </summary>
        /// <param name="test">
        /// The tested object.
        /// </param>
        public EnumerationBlock(IEnumerable test)
        {
            this.test = test;
            this.type = test.GetTypeWithoutThrowingException();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the message as a string.
        /// </summary>
        /// <returns>
        /// A string with the properly formatted message.
        /// </returns>
        public string GetMessage()
        {
            var builder = new StringBuilder();
            builder.Append(this.Description());

            if (this.includeType && this.type != null)
            {
                string temp = this.fullTypeName ? this.type.AssemblyQualifiedName : this.type.ToStringProperlyFormated();
                builder.AppendFormat(" of type: [{0}]", temp);
            }

            if (this.includeHash && this.test != null)
            {
                builder.AppendFormat(" with HashCode: [{0}]", this.test.GetHashCode());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Adds a description of the number of items (only relevant if the object is an enumerable).
        /// </summary>
        /// <param name="itemsCount">
        /// The number of items of the enumerable instance.
        /// </param>
        public void WithEnumerableCount(long itemsCount)
        {
            this.enumerableCount = itemsCount;
        }

        /// <summary>
        /// Requests that the Hash value is included in the description block.
        /// </summary>
        /// <param name="active">
        /// True to include the type. This is the default value.
        /// </param>
        public void WithHashCode(bool active = true)
        {
            this.includeHash = active;
        }

        /// <summary>
        /// Requests that the type is included in the description block.
        /// </summary>
        /// <param name="active">
        /// True to include the type. This is the default value.
        /// </param>
        /// <param name="full">
        /// True to display the full type name (with assembly).
        /// </param>
        public void WithType(bool active = true, bool full = false)
        {
            this.fullTypeName = full;
            this.includeType = active;
        }

        /// <summary>
        /// Requests that a specific type is included in the description block.
        /// </summary>
        /// <param name="forcedType">
        /// Type to include in the description.
        /// </param>
        /// <remarks>
        /// Default type is the type of the object instance given in constructor.
        /// </remarks>
        public void WithType(Type forcedType)
        {
            this.type = forcedType;
            this.includeType = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The description.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string Description()
        {
            var description = new StringBuilder("[");
            var iterator = this.test.GetEnumerator();
            for(var i = 0; i < 10; i++)
            {
                if (!iterator.MoveNext())
                {
                    break;
                }
                if (i == 9)
                {
                    description.Append(", ...");
                    break;
                }
                if (i != 0)
                {
                    description.Append(", ");
                }
                description.Append(iterator.Current.ToStringProperlyFormated());
            }
            
            description.AppendFormat("]");

            if (this.enumerableCount.HasValue)
            {
                description.AppendFormat(
                    " ({0} {1})", 
                    this.enumerableCount, 
                    this.enumerableCount <= 1 ? "item" : "items");
            }

            return description.ToString();
        }

        #endregion
    }
}