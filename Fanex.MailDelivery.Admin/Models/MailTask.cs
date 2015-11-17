using System;

namespace Fanex.MailDelivery.Admin.Models
{
    /// <summary>
    /// Mail Task object.
    /// </summary>
    public class MailTask
    {
        public int TaskId { get; set; }

        public int MailId { get; set; }

        public int SiteId { get; set; }

        public DateTime ScheduleTime { get; set; }

        public DateTime? LastSent { get; set; }

        public int Status { get; set; }

        public int Attempt { get; set; }

        public long MSiteMailId { get; set; }
    }
}
