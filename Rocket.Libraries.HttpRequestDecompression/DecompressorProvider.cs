using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Rocket.Libraries.HttpRequestDecompression.Logging;
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
        private readonly ILogWriter logWriter;

        public DecompressorProvider(
            ICompressionTypeDeterminer compressionDeterminer,
            IGZipStreamProvider gZipStreamProvider,
            IDeflateStreamProvider deflateStreamProvider,
            ILogWriter logWriter)
        {
            this.compressionDeterminer = compressionDeterminer;
            this.gZipStreamProvider = gZipStreamProvider;
            this.deflateStreamProvider = deflateStreamProvider;
            this.logWriter = logWriter;
        }

        public Stream GetDecompressor(HttpRequest httpRequest)
        {
            if (compressionDeterminer.IsCompressedWithEncodingType(httpRequest, CompressionTypeDeterminer.ContentEncodingGzip))
            {
                LogCompressionMethod(CompressionTypeDeterminer.ContentEncodingGzip);
                return gZipStreamProvider.GetStream(httpRequest);
            }
            else if (compressionDeterminer.IsCompressedWithEncodingType(httpRequest, CompressionTypeDeterminer.ContentEncodingDeflate))
            {
                LogCompressionMethod(CompressionTypeDeterminer.ContentEncodingDeflate);
                return deflateStreamProvider.GetStream(httpRequest);
            }
            else
            {
                throw new Exception("Unsupported compression method");
            }
        }

        private void LogCompressionMethod(string compressionMethod)
        {
            logWriter.LogInformation($"Request compressed using: {compressionMethod}");
        }
    }
}