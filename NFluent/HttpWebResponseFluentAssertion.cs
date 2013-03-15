using NFluent.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NFluent
{
    public class HttpWebResponseFluentAssertion : IHttpWebResponseFluentAssertion
    {

        #region Fields

        private readonly HttpWebResponse hwrut;

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

        private IChainableFluentAssertion<IHttpWebResponseFluentAssertion> Chain()
        {
            return new ChainableFluentAssertion<IHttpWebResponseFluentAssertion>(this);
        }

        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> StatusCodeEqualTo(System.Net.HttpStatusCode statusCode)
        {
            if (this.hwrut.StatusCode != statusCode)
            {
                throw new FluentAssertionException(string.Format("[{0}] not equals to the expected http status code [{1}]", this.hwrut.StatusCode.ToStringProperlyFormated(), statusCode.ToStringProperlyFormated()));
            }

            return Chain();
        }

        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> ContainsHeader(System.Net.HttpResponseHeader responseHeader)
        {

            if (string.IsNullOrEmpty(this.hwrut.Headers[responseHeader]))
            {
                throw new FluentAssertionException(string.Format("[{0}] not include in the received response headers", responseHeader.ToStringProperlyFormated()));
            }

            return Chain();
        }

        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsEqualTo(object expected)
        {
            EqualityHelper.IsEqualTo(this.hwrut, expected);
            return Chain();
        }

        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsNotEqualTo(object expected)
        {
            EqualityHelper.IsNotEqualTo(this.hwrut, expected);
            return Chain();
        }

        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsInstanceOf<T>()
        {
            IsInstanceHelper.IsInstanceOf(this.hwrut, typeof(T));
            return Chain();
        }

        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsNotInstanceOf<T>()
        {
            IsInstanceHelper.IsNotInstanceOf(this.hwrut, typeof(T));
            return Chain();
        }


        public IChainableFluentAssertion<IHttpWebResponseFluentAssertion> IsGZipEncoded()
        {
            if (this.hwrut.ContentEncoding != "gzip")
            {
                throw new FluentAssertionException("http response is not encoded with gzip");
            }

            return Chain();
        }
    }
}
