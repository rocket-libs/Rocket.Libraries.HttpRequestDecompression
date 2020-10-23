using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;

namespace Rocket.Libraries.HttpRequestDecompression.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class RequestDecompressionMiddlewareExtension
    {
        public static IApplicationBuilder UseHttpRequestDecompression(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestDecompressionMiddleware>();
        }

    }
}