using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rocket.Libraries.HttpRequestDecompression.Logging;

namespace Rocket.Libraries.HttpRequestDecompression
{
    public class RequestDecompressionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ICompressionTypeDeterminer compressionDeterminer;
        private readonly IDecompressorProvider decompressorProvider;
        private readonly ILogWriter logWriter;

        public RequestDecompressionMiddleware (
            RequestDelegate next,
            ICompressionTypeDeterminer compressionDeterminer,
            IDecompressorProvider decompressorProvider,
            ILogWriter logWriter)
        {
            this.next = next ??
                throw new ArgumentNullException (nameof (next));
            this.compressionDeterminer = compressionDeterminer;
            this.decompressorProvider = decompressorProvider;
            this.logWriter = logWriter;
        }

        public async Task Invoke (HttpContext httpContext)
        {
            if (compressionDeterminer.IsCompressed (httpContext.Request))
            {
                logWriter.LogInformation ($"Compressed {httpContext.Request.Method} request to {httpContext.Request.Path}");
                var decompressor = decompressorProvider.GetDecompressor (httpContext.Request);
                httpContext.Request.Body = decompressor;
            }
            else
            {
                logWriter.LogInformation ($"NOT Compressed {httpContext.Request.Method} request to {httpContext.Request.Path}");
            }
            await next (httpContext);
        }

    }
}