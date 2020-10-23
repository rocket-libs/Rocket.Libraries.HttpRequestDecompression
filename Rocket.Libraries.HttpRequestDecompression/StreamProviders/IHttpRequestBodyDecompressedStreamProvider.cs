using System.IO;
using Microsoft.AspNetCore.Http;

namespace Rocket.Libraries.HttpRequestDecompression.StreamProviders
{
    public interface IHttpRequestBodyDecompressedStreamProvider
    {
        Stream GetStream(HttpRequest httpRequest);
    }
}