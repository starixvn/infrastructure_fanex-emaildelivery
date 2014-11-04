namespace Fanex.MailDelivery.Client.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Job Status.
    /// </summary>
    public enum JobStatus : int
    {
        DataIsNoReady = 1,
        Failed = 2,
        Success = 3,
        Running = 4
    }
}
