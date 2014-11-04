// -----------------------------------------------------------------------
// <copyright file="RabbitMQLogger.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Client.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using EasyNetQ;
    using Fanex.Logging;
    using Fanex.MailDelivery.Client.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailClientLogger: IEasyNetQLogger
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
