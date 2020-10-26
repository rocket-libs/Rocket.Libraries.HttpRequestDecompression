using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Libraries.HttpRequestDecompression.Logging;
using Rocket.Libraries.HttpRequestDecompression.StreamProviders;

namespace Rocket.Libraries.HttpRequestDecompression.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class RequestDecompressionRegistrar
    {
        
        public static void AddHttpRequestDecompressionSupport (this IServiceCollection services, bool enableLogging = false)
        {
            services
                .AddTransient<ICompressionTypeDeterminer, CompressionTypeDeterminer>()
                .AddTransient<IDecompressorProvider, DecompressorProvider>()
                .AddTransient<IGZipStreamProvider,GZipStreamProvider>()
                .AddTransient<IDeflateStreamProvider,DeflateStreamProvider>();
            
            ConfigureLogging(services, enableLogging);

        }

        private static void ConfigureLogging(IServiceCollection services, bool enableLogging)
        {
            if(enableLogging)
            {
                services.AddTransient<ILogWriter, HttpRequestDecompressionLogger>();
            }
            else
            {
                services.AddTransient<ILogWriter, BlackholeLogger>();
            }
        }
    }
}