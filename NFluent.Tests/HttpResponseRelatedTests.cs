namespace NFluent.Tests
{
    using System.Net;

    using NUnit.Framework;

    [TestFixture]
    public class HttpResponseRelatedTests
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[Trailer] header was not found in the response headers")]
        public void HasHeaderThrowExceptionWhenNotExistsWithEnum()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HasHeader(HttpResponseHeader.Trailer);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The http response content is not encoded using gzip.")]
        public void IsGZipEncodedThrowExceptionWhenNotEncoded()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).IsGZipEncoded();
        }
        
        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The http response content is encoded using gzip.")]
        public void IsNotGZipEncodedThrowExceptionWhenEncoded()
        {
            var request = this.CreateRequest();
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).IsNotGZipEncoded();
        }

        [Test]
        public void IsGZipEncodedWorks()
        {
            var request = this.CreateRequest();
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).IsGZipEncoded();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[OK] not equals to the expected http status code [InternalServerError]")]
        public void StatusCodeEqualToThrowExceptionWhenNotEqual()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).StatusCodeEqualTo(HttpStatusCode.InternalServerError);
        }

        [Test]
        public void StatusCodeEqualToWorks()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).StatusCodeEqualTo(HttpStatusCode.OK);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[\"NFluent\"] header was not found in the response headers")]
        public void HasHeaderThrowExceptionWhenNotExistsWithString()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HasHeader("NFluent");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "HasHeader() must be call before beeing able to look his value")]
        public void HeaderContainsThrowExceptionWhenHasHeaderHasNotBeenCalled()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HeaderContains("gws");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "Response header [Server] is not equal to the expected header value [\"Batman\"]")]
        public void HeaderContainsThrowExceptionWhenHeaderValueIsNotEqual()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HasHeader(HttpResponseHeader.Server).And.HeaderContains("Batman");
            Check.That(response).HasHeader("NFluent").And.HeaderContains("gws");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "Response header [\"Server\"] is not equal to the expected header value [\"Robin\"]")]
        public void HeaderContainsThrowExceptionWhenHeaderValueIsNotEqualWithCustomHeader()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HasHeader("Server").And.HeaderContains("Robin");
        }
        
        [Test]
        public void HeaderContainsWorks()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HasHeader(HttpResponseHeader.Server).And.HeaderContains("gws").And
                                .HasHeader("X-Frame-Options").And.HeaderContains("SAMEORIGIN");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The response content does not contain the expected value(s): [\"Robin\", \"Batman\"].")]
        public void ContainsThrowException()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).Contains("Robin", "Batman");
        }
       
        [Test]
        public void ContainsWorks()
        {
            var request = this.CreateRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).Contains("Google");
        }

        [Test]
        public void WorksWithNonGZipResponseStream()
        {
            var request = this.CreateRequest("http://www.gitbub.com");
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).IsNotGZipEncoded();
        }

        [Test]
        public void CheckThatHttpWebResponseAssertionsWorks()
        {
            var request = this.CreateRequest();
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response)
                .StatusCodeEqualTo(HttpStatusCode.OK).And
                .HasHeader(HttpResponseHeader.CacheControl).And
                .HasHeader("X-Frame-Options").And.HeaderContains("SAMEORIGIN").And
                .HasHeader(HttpResponseHeader.Server).And.HeaderContains("gws").And
                .IsGZipEncoded().And
                .Contains("Google").And
                .IsInstanceOf<HttpWebResponse>().And.IsNotInstanceOf<WebResponse>().And
                .IsEqualTo(response).And.IsNotEqualTo("Batman");

            // X-Frame-Options: SAMEORIGIN
        }

        private HttpWebRequest CreateRequest(string url = "http://www.google.ca")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 5000;
            request.UserAgent = "Mozilla/5.0 compatible";
            
            return request;
        }
    }
}