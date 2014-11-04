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
        /// <summary>
        /// Sent status.
        /// </summary>
        Sent = 1,

        /// <summary>
        /// Waiting status.
        /// </summary>
        Waiting = 2,

        /// <summary>
        /// Failed status.
        /// </summary>
        Failed = 3,

        /// <summary>
        /// Running Status.
        /// </summary>
        Running = 4,

        /// <summary>
        /// Data is not ready status.
        /// </summary>
        DataIsNotReady = 5
    }
}
