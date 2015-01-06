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
        /// <summary>
        /// The waiting status.
        /// </summary>
        Waiting = 0,

        /// <summary>
        /// The data is not ready status.
        /// </summary>
        DataIsNotReady = 1,

        /// <summary>
        /// The failed status.
        /// </summary>
        Failed = 2,

        /// <summary>
        /// The success status.
        /// </summary>
        Success = 3,

        /// <summary>
        /// The running status.
        /// </summary>
        Running = 4,

        /// <summary>
        /// The retry status.
        /// </summary>
        Retry = 5
    }
}
