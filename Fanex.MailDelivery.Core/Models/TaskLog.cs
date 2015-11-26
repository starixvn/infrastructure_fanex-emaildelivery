using System;

namespace Fanex.MailDelivery.Core.Models
{
    /// <summary>
    /// The Task Log.
    /// </summary>
    public class TaskLog
    {
        public string Message { get; set; }

        public DateTime LogTime { get; set; }

        public string StackTrace { get; set; }
    }
}
