using System;
using EasyNetQ;

namespace Fanex.MailDelivery.Core.Logging
{
    /// <summary>
    /// Mail Admin Logger.
    /// </summary>
    public class MailAdminLogger : IEasyNetQLogger
    {
        public void DebugWrite(string format, params object[] args)
        {
            // Do Nothing
        }

        public void ErrorWrite(Exception exception)
        {
            // Do Loggin
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
