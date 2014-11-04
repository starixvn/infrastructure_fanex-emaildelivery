namespace Fanex.MailDelivery.Client.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Schedule information.
    /// </summary>
    [Serializable]
    public class Schedule
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public int FunctionId { get; set; }

        public string Name { get; set; }

        public string RepeatExpression { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool Active { get; set; }

        public int RetryIntervalInMinutes { get; set; }

        public int MaxRetryTimes { get; set; }

        public int CurrentRetryTimes { get; set; }

        public Schedule Clone()
        {
            return new Schedule
            {
                Id = Id,
                SiteId = SiteId,
                FunctionId = FunctionId,
                Name = Name,
                RepeatExpression = RepeatExpression,
                StartTime = StartTime,
                EndTime = EndTime,
                Active = Active,
                RetryIntervalInMinutes = RetryIntervalInMinutes,
                MaxRetryTimes = MaxRetryTimes,
                CurrentRetryTimes = CurrentRetryTimes
            };
        }

        public bool IsRunning()
        {
            return CurrentRetryTimes <= MaxRetryTimes;
        }

    }
}
