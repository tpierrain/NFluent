// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IHttpHeaderFluentAssertion.cs" company="">
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
    /// <summary>
    /// Provides assertion methods to be executed on an http header, and allowing them to be
    /// chained with other assertions related to the underlying IHttpWebResponse instance this time.
    /// </summary>
    public interface IHttpHeaderFluentAssertion : IChainableFluentAssertion<IHttpWebResponseFluentAssertion>
    {
        // TODO: make the Contains method accept params of string
        // TODO: make the Contains methode return a IChainableHttpHeaderOrHttpWebResponseFluentAssertion instance
        // TODO: add more methods like StarstWith, EndsWith, etc. May be the proper moment to expose the same methods as the IStringFluentAssert (only the return value will differ) and to leverage on common implementations. 

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
        IChainableFluentAssertion<IHttpWebResponseFluentAssertion> Contains(string expected);
    }
}