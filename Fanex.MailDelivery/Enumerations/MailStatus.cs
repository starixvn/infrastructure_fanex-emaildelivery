namespace Fanex.MailDelivery.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Mail Status.
    /// </summary>
    public enum MailStatus : int
    {
        Sent = 1,
        Waiting = 2,
        Failed = 3,
        Running = 4,
        DataIsNotReady = 5
    }
}
