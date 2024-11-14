using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;

namespace FlyCronicles.Common.Options
{
    public class ErrorNotifyMessage
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public MessageLevelEnum MessageLevel { get; set; }
    }

    public enum MessageLevelEnum
    {
        Issue = 0,
        Warning = 1,
        Error = 10
    }
        

    public class ErrorNotifyClientIdentity
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class ErrorNotifyClientIdentityResponse
    {
        public string Token { get; set; }
        public string UserName { get; set; }
    }

    public class MessageCreator
    {
        public int Level { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FeedbackContact { get; set; }
    }

    public class ErrorNotifyLogger : ILogger
    {
        private readonly string _name;
        private IErrorNotifyService _errorNotifyService;
        private readonly Func<ErrorNotifyLoggerConfiguration> _getCurrentConfig;

        public ErrorNotifyLogger(string name, IErrorNotifyService errorNotifyService,
            Func<ErrorNotifyLoggerConfiguration> getCurrentConfig)
        {
            _errorNotifyService = errorNotifyService;
            _getCurrentConfig = getCurrentConfig;
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel)
        {
            return _getCurrentConfig().LogLevels.Contains(logLevel);
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            ErrorNotifyLoggerConfiguration config = _getCurrentConfig();
            if (config.EventId == 0 || config.EventId == eventId.Id)
            {
                try
                {
                    _errorNotifyService
                        .Send($"Message: {exception.Message} StackTrace: {exception.StackTrace}")
                        .ContinueWith(s => {
                            if (s.Exception != null)
                            {
                                Console.WriteLine($"ErrorNotify exception: {s.Exception.Message} {s.Exception.StackTrace}");
                            }
                        })
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ErrorNotify exception: {ex.Message} {ex.StackTrace}");
                }
            }
        }
    }

    public class ErrorNotifyLoggerConfiguration
    {
        public int EventId { get; set; }

        public ErrorNotifyOptions Options { get; set; }

        public List<LogLevel> LogLevels { get; set; } = new List<LogLevel>()
        {
            LogLevel.Error,
            LogLevel.Critical
        };
    }

    public sealed class ErrorNotifyLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable _onChangeToken;
        private ErrorNotifyLoggerConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, ErrorNotifyLogger> _loggers = new ConcurrentDictionary<string, ErrorNotifyLogger>();


        public ErrorNotifyLoggerProvider(
            IOptionsMonitor<ErrorNotifyLoggerConfiguration> config)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
        }

        public ILogger CreateLogger(string categoryName)
        {
            var errorNotifyService = new ErrorNotifyService(_currentConfig.Options);
            var logger = _loggers.GetOrAdd(categoryName, name => new ErrorNotifyLogger(name, errorNotifyService, GetCurrentConfig));
            return logger;
        }

        private ErrorNotifyLoggerConfiguration GetCurrentConfig() => _currentConfig;

        public void Dispose()
        {
            _loggers.Clear();
            _onChangeToken.Dispose();
        }
    }
}
