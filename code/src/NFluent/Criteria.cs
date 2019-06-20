namespace NFluent
{
#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35
    using System;
#endif
    using System.Text.RegularExpressions;
    using Extensions;

    /// <summary>
    /// Capture a criteria and an evaluation logic
    /// </summary>
    public class Criteria
    {
        private readonly IValueCriteria criteria;

        private Criteria(IValueCriteria criteria)
        {
            this.criteria = criteria;
        }
        
        /// <summary>
        /// Check if the given value answers the appropriate predicated
        /// </summary>
        /// <param name="other">given value to compare</param>
        /// <returns>true if comparison is ok</returns>
        public bool IsEqualTo(string other)
        {
            return this.criteria.Compare(other);
        }
        
        /// <summary>
        /// Build an expected value criteria that will match the same value.
        /// </summary>
        /// <param name="value">Expected value.</param>
        /// <returns>An instance of <see cref="Criteria"/></returns>
        public static implicit operator Criteria(string value)
        {
            return new Criteria(new ValueCriteria(value));
        }

        /// <summary>
        /// Build a regular expression criteria.
        /// </summary>
        /// <param name="expression">regular expression</param>
        /// <returns>An instance of <see cref="Criteria"/></returns>
        public static Criteria FromRegEx(string expression)
        {
            return new Criteria(new RegExCriteria(expression));
        }

        /// <summary>
        /// Split criteria to a multiline criteria.
        /// </summary>
        /// <returns></returns>
        public Criteria[] SplitAsLines()
        {
            if (this.criteria is ValueCriteria line)
            {
                var criterias = line.SplitAsLines();
                var result = new Criteria[criterias.Length];
                for (var i = 0; i < criterias.Length; i++)
                {
                    result[i] = new Criteria(criterias[i]);
                }

                return result;
            }

            var single = new Criteria[1];
            single[0] = this;
            return single;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.criteria.ToString();
        }

        private interface IValueCriteria
        {
            bool Compare(string value);
        }

        private class ValueCriteria : IValueCriteria
        {
            private readonly string reference;

            public ValueCriteria(string reference)
            {
                this.reference = reference;
            }

            public bool Compare(string value)
            {
                return this.reference == value;
            }

            public override string ToString()
            {
                return '"'+this.reference+'"';
            }

            public ValueCriteria[] SplitAsLines()
            {
                var lines = this.reference.SplitAsLines();
                var result = new ValueCriteria[lines.Count];
                for (var i = 0; i < lines.Count; i++)
                {
                    result[i] = new ValueCriteria(lines[i]);
                }

                return result;
            }
        }
        private class RegExCriteria : IValueCriteria
        {
            private readonly string reference;

            public RegExCriteria(string reference)
            {
                this.reference = reference;
            }

            public bool Compare(string value)
            {
                return new Regex(this.reference).IsMatch(value);
            }

            public override string ToString()
            {
                return $"matches: {this.reference}";
            }
        }
    }

}
