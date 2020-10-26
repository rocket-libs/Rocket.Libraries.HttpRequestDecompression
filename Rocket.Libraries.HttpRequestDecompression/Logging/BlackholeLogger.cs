using System.Diagnostics.CodeAnalysis;

namespace Rocket.Libraries.HttpRequestDecompression.Logging
{
   [ExcludeFromCodeCoverage]
    public class BlackholeLogger : ILogWriter
    {
        public void LogInformation(string _)
        {
            // Log messages get swallowed into nothingness
            // Nothing to see here. Move along.
        }
    }
}