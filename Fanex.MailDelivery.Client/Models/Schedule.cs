namespace Fanex.MailDelivery.Client.Models
{
    using System;

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

        public DateTime? EndTime { get; set; }

        public bool Active { get; set; }

        public int RetryIntervalInSeconds { get; set; }

        public int MaxRetryTimes { get; set; }

        public int CurrentRetryTimes { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>Schedule instance.</returns>
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
                RetryIntervalInSeconds = RetryIntervalInSeconds,
                MaxRetryTimes = MaxRetryTimes,
                CurrentRetryTimes = CurrentRetryTimes
            };
        }

        public bool IsRunning()
        {
            return CurrentRetryTimes <= MaxRetryTimes;
        }

        public bool IsScheduleEnd()
        {
            return EndTime != null && EndTime < DateTime.Now;
        }
    }
}