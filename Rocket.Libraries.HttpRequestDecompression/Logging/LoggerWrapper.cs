using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Rocket.Libraries.HttpRequestDecompression.Logging
{
   

    [ExcludeFromCodeCoverage]
    public class LoggerWrapper : ILogWriter
    {
        private readonly ILogger logger;

        public LoggerWrapper(ILogger logger)
        {
            this.logger = logger;
        }

        public void LogInformation(string information)
        {
            logger.LogInformation(information);
        }
    }
}