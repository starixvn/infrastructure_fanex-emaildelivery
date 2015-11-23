using System;
using EasyNetQ;
using Fanex.Logging;
using Fanex.MailDelivery.Client.Configurations;

namespace Fanex.MailDelivery.Client.Logging
{
    /// <summary>
    /// Mail Client Logger.
    /// </summary>
    public class MailClientLogger : IEasyNetQLogger
    {
        public void DebugWrite(string format, params object[] args)
        {
            // Do Nothing
        }

        public void ErrorWrite(Exception exception)
        {
            Logger.Log.Error(exception.Message, AppSettings.MailCientConfig.MessageBrokerLogCategory, exception);
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
