using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.MailDelivery.Client.Messages
{
    public class TaskLog
    {
        public string Message { get; set; }

        public DateTime LogTime { get; set; }

        public string StackTrace { get; set; }
    }
}
