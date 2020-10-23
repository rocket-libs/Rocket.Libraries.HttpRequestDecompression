using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;

namespace Rocket.Libraries.HttpRequestDecompression.StreamProviders
{
    public interface IGZipStreamProvider
    {
        Stream GetStream(HttpRequest httpRequest);
    }

    [ExcludeFromCodeCoverage]
    public class GZipStreamProvider : IHttpRequestBodyDecompressedStreamProvider, IGZipStreamProvider
    {
        public Stream GetStream(HttpRequest httpRequest)
        {
            return (Stream)new GZipStream(httpRequest.Body, CompressionMode.Decompress, true);
        }
    }
}