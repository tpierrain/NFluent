// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HttpWebResponseFluentAssertion.cs" company="">
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
    using System.Linq;
    using System.Net;

    using NFluent.Helpers;

    /// <summary>
    ///     Provides assertion methods to be executed on a HttpWebResponse instance.
    /// </summary>
    public class HttpWebResponseFluentAssertion : IHttpWebResponseFluentAssertion
    {
        #region Fields

        private readonly HttpWebResponse hwrut;

        private HttpResponseHeader? lastCheckedHeader;

        private string lastCheckedCustomHeader;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpWebResponseFluentAssertion"/> class.
        /// </summary>
        /// <param name="value">
        /// The HttpWebResponse Under Test.
        /// </param>
        public HttpWebResponseFluentAssertion(HttpWebResponse value)
        {
            this.hwrut = value;
        }

        #endregion

        /// <summary>
        /// Checks that the http response status code equals the provided status code.
        /// </summary>
        /// <param name="statusCode">The expected http status code.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The http response code does not equal to the given status code.</exception>
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> StatusCodeEqualTo(HttpStatusCode statusCode)
        {
            if (this.hwrut.StatusCode != statusCode)
            {
                throw new FluentAssertionException(string.Format("[{0}] not equals to the expected http status code [{1}]", this.hwrut.StatusCode.ToStringProperlyFormated(), statusCode.ToStringProperlyFormated()));
            }

            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Check whether the specified header is contains within the response headers.
        /// </summary>
        /// <param name="header">The expected response header.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The header was not contains in response headers.</exception>
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> HasHeader(HttpResponseHeader header)
        {
            if (string.IsNullOrEmpty(this.hwrut.Headers[header]))
            {
                throw new FluentAssertionException(string.Format("[{0}] header was not found in the response headers", header.ToStringProperlyFormated()));
            }

            this.lastCheckedHeader = header;
            this.lastCheckedCustomHeader = null;
            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Check whether the specified header is contains within the response headers.
        /// </summary>
        /// <param name="header">The expected response header.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The header was not contains in response headers.</exception>
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> HasHeader(string header)
        {
            if (string.IsNullOrEmpty(this.hwrut.Headers[header]))
            {
                throw new FluentAssertionException(string.Format("[{0}] header was not found in the response headers", header.ToStringProperlyFormated()));
            }

            this.lastCheckedCustomHeader = header;
            this.lastCheckedHeader = null;
            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Checks that the actual response content is "gzip" encoded.
        /// </summary>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual response content is not encoded using gzip.</exception>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsGZipEncoded()
        {
            if (!this.IsGZipEncodedInternal())
            {
                throw new FluentAssertionException("The http response content is not encoded using gzip.");
            }

            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Checks that the actual response content is not encoded using gzip.
        /// </summary>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual response content is encoded using gzip.</exception>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsNotGZipEncoded()
        {
            if (this.IsGZipEncodedInternal())
            {
                throw new FluentAssertionException("The http response content is encoded using gzip.");
            }

            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Checks that the specified header is equal to the provided value.
        /// </summary>
        /// <param name="headerValue">
        /// The expected response header value.
        /// </param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The header value is not equal to the expected value.
        /// The HasHeader method as not being called.
        /// </exception>
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> HeaderContains(string headerValue)
        {
            if (this.lastCheckedHeader.HasValue && this.hwrut.Headers[this.lastCheckedHeader.Value] != headerValue)
            {
                throw new FluentAssertionException(string.Format("Response header [{0}] is not equal to the expected header value [{1}]", this.lastCheckedHeader.Value.ToStringProperlyFormated(), headerValue.ToStringProperlyFormated()));
            }

            if (!string.IsNullOrEmpty(this.lastCheckedCustomHeader) && this.hwrut.Headers[this.lastCheckedCustomHeader] != headerValue)
            {
                throw new FluentAssertionException(string.Format("Response header [{0}] is not equal to the expected header value [{1}]", this.lastCheckedCustomHeader.ToStringProperlyFormated(), headerValue.ToStringProperlyFormated()));
            }

            if (string.IsNullOrEmpty(this.lastCheckedCustomHeader) && !this.lastCheckedHeader.HasValue)
            {
                throw new FluentAssertionException("HasHeader() must be call before beeing able to look his value");
            }

            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Checks that the response content contains the given expected values, in any order.
        /// </summary>
        /// <param name="values">
        /// The expected values to be found.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The response content does not contains all the given strings in any order.
        /// </exception>
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> Contains(params string[] values)
        {
            var responseContent = this.IsGZipEncodedInternal() ? 
                                        HttpHelper.ReadGZipStream(this.hwrut) : 
                                        HttpHelper.ReadResponseStream(this.hwrut);
            var notFound = values.Where(value => !responseContent.Contains(value)).ToList();

            if (notFound.Count > 0)
            {
                throw new FluentAssertionException(string.Format(@"The response content does not contain the expected value(s): [{0}].", notFound.ToEnumeratedString()));
            }

            return this.ChainFluentAssertion();
        }

        #region IEqualityFluentAssertion members
        /// <summary>
        /// Checks that a given instance is considered to be equal to another expected instance. Throws <see cref="FluentAssertionException"/> otherwise.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected instance.</param>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsEqualTo(object expected)
        {
            EqualityHelper.IsEqualTo(this.hwrut, expected);
            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Checks that a given instance is considered to not be equal to another expected instance. Throws <see cref="FluentAssertionException"/> otherwise.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected instance.</param>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsNotEqualTo(object expected)
        {
            EqualityHelper.IsNotEqualTo(this.hwrut, expected);
            return this.ChainFluentAssertion();
        }
        #endregion

        #region IInstanceTypeFluentAssertion members
        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <returns>A chainable fluent assertion.</returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsInstanceOf<T>()
        {
            IsInstanceHelper.IsInstanceOf(this.hwrut, typeof(T));
            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <returns>A chainable fluent assertion. </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsNotInstanceOf<T>()
        {
            IsInstanceHelper.IsNotInstanceOf(this.hwrut, typeof(T));
            return this.ChainFluentAssertion();
        }
        #endregion

        private IChainableFluentAssertion<IHttpWebResponseFluentAssertion> ChainFluentAssertion()
        {
            return new ChainableFluentAssertion<IHttpWebResponseFluentAssertion>(this);
        }

        private bool IsGZipEncodedInternal()
        {
            return this.hwrut.ContentEncoding == "gzip";
        }
    }
}
