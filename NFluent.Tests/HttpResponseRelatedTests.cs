namespace NFluent.Tests
{
    using NUnit.Framework;
    using System.Net;

    [TestFixture]
    public class HttpResponseRelatedTests
    {
        
        [Test]
        public void CanCheckGoogleHttpResponse()
        {
            var request = (HttpWebRequest)HttpWebRequest.Create("http://www.google.ca");
            request.Timeout = 500;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.22 (KHTML, like Gecko) Chrome/25.0.1364.152 Safari/537.22";

            var response = (HttpWebResponse)request.GetResponse();

            Check.That(response)
                .StatusCodeEqualTo(HttpStatusCode.OK).And
                .ContainsHeader(HttpResponseHeader.CacheControl).And
                .IsGZipEncoded();

        }
    }
}