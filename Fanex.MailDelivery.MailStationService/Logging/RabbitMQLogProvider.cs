using EasyNetQ.Logging;
using Fanex.MailDelivery.Services.Services.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Timers;

namespace Fanex.MailDelivery.MailStationService.Logging
{
    public class RabbitMQLogger
    {
        private const int SendingMaxTime = 10;
        private int sendingMessageCount = SendingMaxTime;
        private readonly Timer timer = new Timer(10 * 60 * 1000);
        private readonly LogService logService;

        public RabbitMQLogger() : this(null)
        {
        }

        public RabbitMQLogger(LogService logService)
        {
            this.logService = logService ?? new LogServiceImpl();
        }

        public void LogError(string message, Exception exception)
        {
            if (sendingMessageCount > 0)
            {
                sendingMessageCount -= 1;

                if (sendingMessageCount == 0)
                {
                    timer.Elapsed += Timer_Elapsed;
                    timer.Start();
                }

                logService.LogError(message, exception);
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            timer.Elapsed -= Timer_Elapsed;

            sendingMessageCount = SendingMaxTime;
        }
    }

    public class RabbitMQLogProvider : EasyNetQ.Logging.ILogProvider
    {
        private readonly RabbitMQLogger logger;

        public RabbitMQLogProvider()
        {
            this.logger = new RabbitMQLogger();
        }

        public EasyNetQ.Logging.Logger GetLogger(string name)
        {
            return (logLevel, messageFunc, exception, formatParameters) =>
            {
                if ((logLevel == LogLevel.Error || logLevel == LogLevel.Fatal) &&
                    messageFunc != null)
                {
                    var message = LogMessageFormatter.FormatStructuredMessage(messageFunc(), formatParameters);

                    if (exception != null)
                    {
                        logger.LogError(message, exception);
                    }
                }

                return true;
            };
        }

        #region NotImplemented
        public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
        {
            throw new NotImplementedException();
        }

        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    internal static class LogMessageFormatter
    {
        private static readonly Regex Pattern = new Regex(@"(?<!{){@?(?<arg>[^ :{}]+)(?<format>:[^}]+)?}",
            RegexOptions.Compiled);

        private static string ReplaceFirst(string text, string search, string replace)
        {
            var pos = text.IndexOf(search, StringComparison.Ordinal);
            if (pos < 0)
            {
                return text;
            }

            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static string FormatStructuredMessage(string targetMessage, object[] formatParameters)
        {
            if (formatParameters == null || formatParameters.Length == 0)
            {
                return targetMessage;
            }

            List<string> processedArguments = null;
            var formattedTargetMessage = targetMessage;

            foreach (Match match in Pattern.Matches(formattedTargetMessage))
            {
                var arg = match.Groups["arg"].Value;
                int outValue;

                if (!int.TryParse(arg, out outValue))
                {
                    processedArguments = processedArguments ?? new List<string>(formatParameters.Length);
                    var argumentIndex = processedArguments.IndexOf(arg);
                    if (argumentIndex == -1)
                    {
                        argumentIndex = processedArguments.Count;
                        processedArguments.Add(arg);
                    }

                    formattedTargetMessage = ReplaceFirst(formattedTargetMessage, match.Value,
                        string.Concat("{", argumentIndex.ToString(), match.Groups["format"].Value, "}"));
                }
            }

            try
            {
                return string.Format(CultureInfo.InvariantCulture, formattedTargetMessage, formatParameters);
            }
            catch (FormatException ex)
            {
                throw new FormatException(
                    "The input string '" + formattedTargetMessage + "' could not be formatted using string.Format", ex);
            }
        }
    }
}