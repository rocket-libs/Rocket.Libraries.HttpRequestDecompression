using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Libraries.HttpRequestDecompression.StreamProviders;

namespace Rocket.Libraries.HttpRequestDecompression.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class RequestDecompressionRegistrar
    {
        
        public static void AddHttpRequestDecompressionSupport (this IServiceCollection services)
        {
            services
                .AddTransient<ICompressionTypeDeterminer, CompressionTypeDeterminer>()
                .AddTransient<IDecompressorProvider, DecompressorProvider>()
                .AddTransient<IGZipStreamProvider,GZipStreamProvider>()
                .AddTransient<IDeflateStreamProvider,DeflateStreamProvider>();
        }
    }
}