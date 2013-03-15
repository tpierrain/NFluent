// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IHttpWebResponseFluentAssertion.cs" company="">
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
using System.Net;
namespace NFluent
{
    /// <summary>
    /// Provides assertion methods to be executed on a HttpWebResponse instance.
    /// </summary>
    public interface IHttpWebResponseFluentAssertion : IFluentAssertion, IEqualityFluentAssertion<IHttpWebResponseFluentAssertion>, IInstanceTypeFluentAssertion<IHttpWebResponseFluentAssertion>
    {
        /// <summary>
        /// Checks that the http response status code equals the provided status code.
        /// </summary>
        /// <param name="statusCode">The expected http status code.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The http response code does not equal to the given status code.</exception>
        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> StatusCodeEqualTo(HttpStatusCode statusCode);


        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> ContainsHeader(System.Net.HttpResponseHeader responseHeader);


        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsGZipEncoded();
    }
}