// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IHttpWebResponseFluentAssertion.cs" company="">
// //   Copyright 2013 Marc-Antoine LATOUR
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
    using System.Net;

    /// <summary>
    /// Provides assertion methods to be executed on a HttpWebResponse instance.
    /// </summary>
    public interface IHttpWebResponseFluentAssertion : IFluentAssertion, IEqualityFluentAssertionTrait<IHttpWebResponseFluentAssertion>, IInstanceTypeFluentAssertionTrait<IHttpWebResponseFluentAssertion>
    {
        /// <summary>
        /// Checks that the http response status code equals the provided status code.
        /// </summary>
        /// <param name="statusCode">The expected http status code.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The http response code does not equal to the given status code.</exception>
        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> StatusCodeEqualsTo(HttpStatusCode statusCode);

        /// <summary>
        /// Check whether the specified header is contains within the response headers.
        /// </summary>
        /// <param name="header">The expected response header.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The header was not contains in response headers.</exception>
        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> HasHeader(HttpResponseHeader header);

        /// <summary>
        /// Check whether the specified header is contains within the response headers.
        /// </summary>
        /// <param name="header">The expected response header.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The header was not contains in response headers.</exception>
        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> HasHeader(string header);

        /// <summary>
        /// Checks that the actual response content is "gzip" encoded.
        /// </summary>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual response content is not encoded using gzip.</exception>
        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsGZipEncoded();

        /// <summary>
        /// Checks that the actual response content is not encoded using gzip.
        /// </summary>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual response content is encoded using gzip.</exception>
        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsNotGZipEncoded();

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
        /// </exception>
        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> HeaderContains(string headerValue);

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
        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> Contains(params string[] values);
    }
}