using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Rocket.Libraries.HttpRequestDecompression.Logging
{
   

    [ExcludeFromCodeCoverage]
    public class HttpRequestDecompressionLogger : ILogWriter
    {
        private readonly ILogger<HttpRequestDecompressionLogger> logger;

        public HttpRequestDecompressionLogger(ILogger<HttpRequestDecompressionLogger> logger)
        {
            this.logger = logger;
        }

        public void LogInformation(string information)
        {
            logger.LogInformation(information);
        }
    }
}