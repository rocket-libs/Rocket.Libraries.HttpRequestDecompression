using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;

namespace Rocket.Libraries.HttpRequestDecompression.StreamProviders
{
    public interface IDeflateStreamProvider
    {
        Stream GetStream(HttpRequest httpRequest);
    }

    [ExcludeFromCodeCoverage]
    public class DeflateStreamProvider : IHttpRequestBodyDecompressedStreamProvider, IDeflateStreamProvider
    {
        public Stream GetStream(HttpRequest httpRequest)
        {
            return (Stream)new DeflateStream(httpRequest.Body, CompressionMode.Decompress, true);
        }
    }
}