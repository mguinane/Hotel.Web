using Hotel.Web.Core.Services.Logging;
using Microsoft.Extensions.Logging;

namespace Hotel.Web.Infrastructure.Services.Logging
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }
    }
}
