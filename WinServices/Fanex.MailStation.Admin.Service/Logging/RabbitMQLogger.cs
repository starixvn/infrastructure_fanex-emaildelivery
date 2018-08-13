using System;
using EasyNetQ;

namespace Fanex.MailStation.Admin.Service.Logging
{
    /// <summary>
    /// Rabbit MQ Logger.
    /// </summary>
    public class RabbitMqLogger : IEasyNetQLogger
    {
        public void DebugWrite(string format, params object[] args)
        {
            // Do Nothing
        }

        public void ErrorWrite(Exception exception)
        {
            // Do Nothing
        }

        public void ErrorWrite(string format, params object[] args)
        {
            // Do Nothing
        }

        public void InfoWrite(string format, params object[] args)
        {
            // Do Nothing
        }
    }
}
