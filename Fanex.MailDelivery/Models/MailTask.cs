namespace Fanex.MailDelivery.Models
{
    using System;

    /// <summary>
    /// Mail Task object.
    /// </summary>
    public class MailTask
    {
        public int TaskID { get; set; }

        public int MailID { get; set; }

        public int SiteID { get; set; }

        public DateTime ScheduleTime { get; set; }

        public DateTime? LastSent { get; set; }

        public int Status { get; set; }

        public int Attempt { get; set; }

        public long MSiteMailID { get; set; }
    }
}
