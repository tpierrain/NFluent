namespace NFluent.Tests
{
    using System.Net;

    using NUnit.Framework;

    [TestFixture]
    public class HttpResponseRelatedTests
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The http response content is not encoded using gzip.")]
        public void IsGZipEncodedThrowsExceptionWhenNotEncoded()
        {
            var request = this.CreateGoogleHttpRequest();
            
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).IsGZipEncoded();
            }
        }
        
        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "The http response content is encoded using gzip.")]
        public void IsNotGZipEncodedThrowsExceptionWhenEncoded()
        {
            var request = this.CreateGoogleHttpRequest();
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).IsNotGZipEncoded();
            }
        }

        [Test]
        public void IsGZipEncodedWorks()
        {
            var request = this.CreateGoogleHttpRequest();
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).IsGZipEncoded();
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[OK] not equals to the expected http status code [InternalServerError]")]
        public void StatusCodeEqualToThrowsExceptionWhenNotEqual()
        {
            var request = this.CreateGoogleHttpRequest();
            
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).StatusCodeEqualsTo(HttpStatusCode.InternalServerError);
            }
        }

        [Test]
        public void StatusCodeEqualsToWorks()
        {
            var request = this.CreateGoogleHttpRequest();
            
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).StatusCodeEqualsTo(HttpStatusCode.OK);
            }
        }

        [Test]
        public void HasHeaderWhichContainsWorks()
        {
            var request = this.CreateGoogleHttpRequest();

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).HasHeader(HttpResponseHeader.Server).Which.Contains("gws")
                                    .And.HasHeader("X-Frame-Options").Which.Contains("SAMEORIGIN");
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[\"Trailer\"] header was not found in the response headers")]
        public void HasHeaderThrowsExceptionWhenNotExistsWithEnum()
        {
            var request = this.CreateGoogleHttpRequest();

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).HasHeader(HttpResponseHeader.Trailer);
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[\"NFluent\"] header was not found in the response headers")]
        public void HasHeaderThrowsExceptionWhenHeaderIsNotFoundWithString()
        {
            var request = this.CreateGoogleHttpRequest();
            
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).HasHeader("NFluent");
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "[\"ContentMd5\"] header was not found in the response headers")]
        public void HasHeaderThrowsExceptionWhenHeaderIsNotFoundWithEnumeration()
        {
            var request = this.CreateGoogleHttpRequest();

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).HasHeader(HttpResponseHeader.ContentMd5);
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "Response header with name [\"Server\"] does not contain [\"Batman\"]. Header content is [\"gws\"].")]
        public void HasHeaderWhichContainsThrowsExceptionWithProperMessageWithHeaderEnumeration()
        {
            var request = this.CreateGoogleHttpRequest();
            
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).HasHeader(HttpResponseHeader.Server).Which.Contains("Batman");
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "Response header with name [\"Server\"] does not contain [\"Robin\"]. Header content is [\"gws\"].")]
        public void HasHeaderWhichContainsThrowsExceptionWithProperMessageWithCustomHeaderName()
        {
            var request = this.CreateGoogleHttpRequest();
            
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).HasHeader("Server").Which.Contains("Robin");
            }
        }

        [Test]
        public void ContainsWorks()
        {
            var request = this.CreateGoogleHttpRequest();

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).Contains("Google");
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), MatchType = MessageMatch.StartsWith, ExpectedMessage = "The http response content does not contain the expected value(s): [\"Robin\", \"Batman\"].")]
        public void ContainsThrowsException()
        {
            var request = this.CreateGoogleHttpRequest();
            
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).Contains("Robin", "Batman");
            }
        }
       
        [Test]
        public void IsNotGZipEncodedWorks()
        {
            var request = this.CreateHttpRequest("http://www.gitbub.com");

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).IsNotGZipEncoded();
            }
        }

        [Test]
        public void AndOperatorWorksWithVariousAssertionTypes()
        {
            var request = this.CreateGoogleHttpRequest();
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Check.That(response).StatusCodeEqualsTo(HttpStatusCode.OK)
                                    .And.HasHeader(HttpResponseHeader.CacheControl)
                                    .And.HasHeader("X-Frame-Options").Which.Contains("SAMEORIGIN")
                                    .And.HasHeader(HttpResponseHeader.Server).Which.Contains("gws")
                                    .And.IsGZipEncoded().And.Contains("Google")
                                    .And.IsInstanceOf<HttpWebResponse>()
                                    .And.IsNotInstanceOf<WebResponse>()
                                    .And.IsEqualTo(response).And.IsNotEqualTo("Batman");

                // X-Frame-Options: SAMEORIGIN

                // TODO: Allow usage of multiple assertions on a given header such as:  .And.HasHeader("X-Frame-Options").Which.Contains("SAMEORIGIN").And.StartsWith("SAME").And.EndsWith("ORIGIN")
            }
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