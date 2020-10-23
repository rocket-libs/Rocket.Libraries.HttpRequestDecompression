using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Rocket.Libraries.HttpRequestDecompression
{
    public interface ICompressionTypeDeterminer
    {
        bool IsCompressed (HttpRequest httpRequest);
        bool IsCompressedWithEncodingType (HttpRequest httpRequest, string compressionType);
        bool IsDeflated (HttpRequest httpRequest);
        bool IsGZipped (HttpRequest httpRequest);
    }

    public class CompressionTypeDeterminer : ICompressionTypeDeterminer
    {
        public const string ContentEncodingHeader = "Content-Encoding";
        public const string ContentEncodingGzip = "gzip";
        public const string ContentEncodingDeflate = "deflate";

        public bool IsCompressed (HttpRequest httpRequest) => IsGZipped (httpRequest) || IsDeflated (httpRequest);

        public bool IsGZipped (HttpRequest httpRequest) => IsCompressedWithEncodingType (httpRequest, ContentEncodingGzip);
        public bool IsDeflated (HttpRequest httpRequest) => IsCompressedWithEncodingType (httpRequest, ContentEncodingDeflate);

        public bool IsCompressedWithEncodingType (HttpRequest httpRequest, string compressionType)
        {
            if (httpRequest.Headers.Keys.Contains (ContentEncodingHeader))
            {
                var encodingHeaderValue = httpRequest.Headers[ContentEncodingHeader];
                return encodingHeaderValue.ToString ().Equals (compressionType, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                return false;
            }
        }
    }
}