using System;

namespace Fanex.MailDelivery.Core.Messaging.Job
{
    public class ScheduleRequest
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public int TaskId { get; set; }

        public string Name { get; set; }

        public int TriggerType { get; set; }

        public string RepeatExpression { get; set; }

        /// <summary>
        ///     It's not current fire time of system but the time admin want it to be.
        ///     It's mean the task is run for schedule on the pass (Missing jobs).
        /// </summary>
        public DateTime? CurrentFireTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool Active { get; set; }

        public int RetryInterval { get; set; }

        public int MaxRetryTimes { get; set; }

        public int CurrentRetryTimes { get; set; }
    }
}