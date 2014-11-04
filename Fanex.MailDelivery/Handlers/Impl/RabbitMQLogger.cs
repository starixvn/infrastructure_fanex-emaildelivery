namespace Fanex.MailDelivery.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using EasyNetQ;

    /// <summary>
    /// Rabbit MQ Logger.
    /// </summary>
    public class RabbitMQLogger : IEasyNetQLogger
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
