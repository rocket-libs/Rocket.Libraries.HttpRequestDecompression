using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;
using Rocket.Libraries.HttpRequestDecompression.StreamProviders;

namespace Rocket.Libraries.HttpRequestDecompression
{
    public interface IDecompressorProvider
    {
        Stream GetDecompressor(HttpRequest httpRequest);
    }

    public class DecompressorProvider : IDecompressorProvider
    {
        private readonly ICompressionTypeDeterminer compressionDeterminer;
        private readonly IGZipStreamProvider gZipStreamProvider;
        private readonly IDeflateStreamProvider deflateStreamProvider;

        public DecompressorProvider(
            ICompressionTypeDeterminer compressionDeterminer,
            IGZipStreamProvider gZipStreamProvider,
            IDeflateStreamProvider deflateStreamProvider)
        {
            this.compressionDeterminer = compressionDeterminer;
            this.gZipStreamProvider = gZipStreamProvider;
            this.deflateStreamProvider = deflateStreamProvider;
        }

        public Stream GetDecompressor(HttpRequest httpRequest)
        {
            if (compressionDeterminer.IsCompressedWithEncodingType(httpRequest, CompressionTypeDeterminer.ContentEncodingGzip))
            {
                return gZipStreamProvider.GetStream(httpRequest);
            }
            else if (compressionDeterminer.IsCompressedWithEncodingType(httpRequest, CompressionTypeDeterminer.ContentEncodingDeflate))
            {
                return deflateStreamProvider.GetStream(httpRequest);
            }
            else
            {
                throw new Exception("Unsupported compression method");
            }
        }
    }
}