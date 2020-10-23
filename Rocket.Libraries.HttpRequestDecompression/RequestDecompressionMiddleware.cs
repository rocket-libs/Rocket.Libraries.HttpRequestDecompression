using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Rocket.Libraries.HttpRequestDecompression
{
    public class RequestDecompressionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ICompressionTypeDeterminer compressionDeterminer;
        private readonly IDecompressorProvider decompressorProvider;

        public RequestDecompressionMiddleware(
            RequestDelegate next, 
            ICompressionTypeDeterminer compressionDeterminer,
            IDecompressorProvider decompressorProvider)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.compressionDeterminer = compressionDeterminer;
            this.decompressorProvider = decompressorProvider;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (compressionDeterminer.IsCompressed(httpContext.Request))
            {
                var decompressor = decompressorProvider.GetDecompressor(httpContext.Request);
                httpContext.Request.Body = decompressor;
            }
            await next(httpContext);
        }

        
    }
}