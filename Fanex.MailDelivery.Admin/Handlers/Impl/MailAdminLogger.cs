namespace Fanex.MailDelivery.Admin.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using EasyNetQ;

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
