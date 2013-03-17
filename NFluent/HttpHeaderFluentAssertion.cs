// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HttpHeaderFluentAssertion.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
namespace NFluent
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides assertion methods to be executed on an http header, and allowing them to be
    /// chained with other assertions related to the underlying IHttpWebResponse instance this time.
    /// </summary>
    internal class HttpHeaderFluentAssertion : IHttpHeaderFluentAssertion
    {
        private readonly string headerName;
        private readonly string headerContent;
        private readonly IHttpWebResponseFluentAssertion webResponseFluentAssertion;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHeaderFluentAssertion" /> class.
        /// </summary>
        /// <param name="headerName">Name of the header.</param>
        /// <param name="headerContent">Content of the header.</param>
        /// <param name="webResponseFluentAssertion">The web response fluent assertion.</param>
        public HttpHeaderFluentAssertion(string headerName, string headerContent, IHttpWebResponseFluentAssertion webResponseFluentAssertion)
        {
            this.headerName = headerName;
            this.headerContent = headerContent;
            this.webResponseFluentAssertion = webResponseFluentAssertion;
        }

        /// <summary>
        /// Chains a new fluent assertion to the current one.
        /// </summary>
        /// <value>
        /// The new fluent assertion instance which has been chained to the previous one.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here.")]
        IHttpWebResponseFluentAssertion IChainableFluentAssertion<IHttpWebResponseFluentAssertion>.And
        {
            get
            {
                return this.webResponseFluentAssertion;
            }
        }

        /// <summary>
        /// Checks that the actual http response header contains the expected given string.
        /// </summary>
        /// <param name="expected">
        /// A string that is expected to be present in the actual response header content.
        /// </param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual http response header does not contain the expected value.
        /// </exception>
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> Contains(string expected)
        {
            if (!this.headerContent.Contains(expected))
            {
                throw new FluentAssertionException(string.Format("Response header with name [{0}] does not contain [{1}]. Header content is [{2}].", this.headerName.ToStringProperlyFormated(), expected.ToStringProperlyFormated(), this.headerContent.ToStringProperlyFormated()));
            }

            return new ChainableFluentAssertion<IHttpWebResponseFluentAssertion>(this.webResponseFluentAssertion);
        }
    }
}