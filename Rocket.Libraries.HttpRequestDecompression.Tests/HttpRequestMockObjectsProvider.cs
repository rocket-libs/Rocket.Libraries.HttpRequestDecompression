using Microsoft.AspNetCore.Http;
using Moq;

namespace Rocket.Libraries.HttpRequestDecompression.Tests
{
    public class HttpRequestMockObjectsProvider
    {
        private Mock<HttpRequest> httpRequest;
        public Mock<HttpContext> MockHttpContext
        {
            get
            {
                var httpContext = new Mock<HttpContext> ();
                var httpResponse = new Mock<HttpResponse> ();

                httpContext.Setup (a => a.Request)
                    .Returns (HttpRequest.Object);

                httpContext.Setup (a => a.Response)
                    .Returns (httpResponse.Object);

                return httpContext;
            }
        }

        public Mock<HttpRequest> HttpRequest
        {
            get
            {
                if (httpRequest == null)
                {
                    httpRequest = new Mock<HttpRequest> ();
                }
                return httpRequest;
            }
        }
    }
}