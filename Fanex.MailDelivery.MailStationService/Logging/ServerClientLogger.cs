using System;

namespace Fanex.MailDelivery.MailStationService.Logging
{
    public class ServerClientLogger //: IEasyNetQLogger
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
