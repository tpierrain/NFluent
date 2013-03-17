namespace NFluent.Tests
{
    using System.Net;

    using NUnit.Framework;

    [TestFixture]
    [Ignore("note from Thomas to Marco: these tests are much more integration tests than unit tests. Also, a lot of tests are failing here, due to timeout exception. Would it be feasible to mock every http response creation, or to memoize answers to speed up and make those tests reliable?")]
    public class HttpResponseRelatedTests
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[Trailer] header was not found in the response headers")]
        public void HasHeaderThrowsExceptionWhenNotExistsWithEnum()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HasHeader(HttpResponseHeader.Trailer);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The http response content is not encoded using gzip.")]
        public void IsGZipEncodedThrowsExceptionWhenNotEncoded()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).IsGZipEncoded();
        }
        
        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The http response content is encoded using gzip.")]
        public void IsNotGZipEncodedThrowsExceptionWhenEncoded()
        {
            var request = this.CreateGoogleHttpRequest();
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).IsNotGZipEncoded();
        }

        [Test]
        public void IsGZipEncodedWorks()
        {
            var request = this.CreateGoogleHttpRequest();
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).IsGZipEncoded();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[OK] not equals to the expected http status code [InternalServerError]")]
        public void StatusCodeEqualToThrowsExceptionWhenNotEqual()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).StatusCodeEqualsTo(HttpStatusCode.InternalServerError);
        }

        [Test]
        public void StatusCodeEqualsToWorks()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).StatusCodeEqualsTo(HttpStatusCode.OK);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[\"NFluent\"] header was not found in the response headers")]
        public void HasHeaderThrowsExceptionWhenNotExistsWithString()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HasHeader("NFluent");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "HasHeader() must be called before beeing able to look his value")]
        public void HeaderContainsThrowsExceptionWhenHasHeaderHasNotBeenCalled()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();
            Check.That(response).HeaderContains("gws");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "Response header [Server] is not equal to the expected header value [\"Batman\"]")]
        public void HeaderContainsThrowsExceptionWhenHeaderValueIsNotEqual()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HasHeader(HttpResponseHeader.Server).And.HeaderContains("Batman");
            Check.That(response).HasHeader("NFluent").And.HeaderContains("gws");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "Response header [\"Server\"] is not equal to the expected header value [\"Robin\"]")]
        public void HeaderContainsThrowsExceptionWhenHeaderValueIsNotEqualWithCustomHeader()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HasHeader("Server").And.HeaderContains("Robin");
        }
        
        [Test]
        public void HeaderContainsWorks()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).HasHeader(HttpResponseHeader.Server).And.HeaderContains("gws").And
                                .HasHeader("X-Frame-Options").And.HeaderContains("SAMEORIGIN");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The response content does not contain the expected value(s): [\"Robin\", \"Batman\"].")]
        public void ContainsThrowsException()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).Contains("Robin", "Batman");
        }
       
        [Test]
        public void ContainsWorks()
        {
            var request = this.CreateGoogleHttpRequest();
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).Contains("Google");
        }

        [Test]
        public void WorksWithNonGZipResponseStream()
        {
            var request = this.CreateHttpRequest("http://www.gitbub.com");
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response).IsNotGZipEncoded();
        }

        [Test]
        public void CheckThatHttpWebResponseAssertionsWorks()
        {
            var request = this.CreateGoogleHttpRequest();
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            
            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response)
                .StatusCodeEqualsTo(HttpStatusCode.OK).And
                .HasHeader(HttpResponseHeader.CacheControl).And
                .HasHeader("X-Frame-Options").And.HeaderContains("SAMEORIGIN").And
                .HasHeader(HttpResponseHeader.Server).And.HeaderContains("gws").And
                .IsGZipEncoded().And
                .Contains("Google").And
                .IsInstanceOf<HttpWebResponse>().And.IsNotInstanceOf<WebResponse>().And
                .IsEqualTo(response).And.IsNotEqualTo("Batman");

            // X-Frame-Options: SAMEORIGIN
        }

        private HttpWebRequest CreateGoogleHttpRequest()
        {
            return this.CreateHttpRequest("http://www.google.ca");
        }

        private HttpWebRequest CreateHttpRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 5000;
            request.UserAgent = "Mozilla/5.0 compatible";
            
            return request;
        }
    }
}