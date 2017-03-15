using System;

namespace Fanex.MailDelivery.Core.Models
{
    /// <summary>
    /// Schedule information.
    /// </summary>
    [Serializable]
    public class Schedule
    {
        // MailId
        public int Id { get; set; }

        public int SiteId { get; set; }

        public int TaskId { get; set; }

        public string Name { get; set; }

        public int TriggerType { get; set; }

        public string RepeatExpression { get; set; }

        /// <summary>
        /// It's not current fire time of system but the time admin want it to be.
        /// It's mean the task is run for schedule on the pass (Missing jobs).
        /// </summary>
        public DateTime? CurrentFireTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool Active { get; set; }

        public int RetryInterval { get; set; }

        public int MaxRetryTimes { get; set; }

        public int CurrentRetryTimes { get; set; }

        public MailType TriggerTypeEnum
        {
            get 
            {
                return this.TriggerType == 0 ? MailType.Function : MailType.Schedule;
            }

            set 
            {
                this.TriggerType = (int)value;
            }
        }

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
                TaskId = TaskId,
                Name = Name,
                RepeatExpression = RepeatExpression,
                StartTime = StartTime,
                EndTime = EndTime,
                Active = Active,
                RetryInterval = RetryInterval,
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

        /// <summary>
        /// Converts to server time.
        /// </summary>
        public void ConvertToServerTime(bool isLocalEnvironment)
        {
            int hoursDifferent = this.CalculateHoursDifferentBeetweenLocalAndServer(isLocalEnvironment);

            StartTime = StartTime.AddHours(hoursDifferent);
            if (EndTime != null)
            {
                EndTime = EndTime.Value.AddHours(hoursDifferent);
            }

            ConvertCronExpressionToServerTime(hoursDifferent);
        }

        /// <summary>
        /// Converts the CRON expression to server time.
        /// </summary>
        /// <param name="hoursDifferent">The hours different.</param>
        private void ConvertCronExpressionToServerTime(int hoursDifferent)
        {
            try
            {
                string[] cron = this.RepeatExpression.Split(' ');
                int hour = int.Parse(cron[2]);
                hour += hoursDifferent;
                while (hour > 23)
                {
                    hour -= 24;
                }

                while (hour < 0)
                {
                    hour += 24;
                }

                cron[2] = hour.ToString();
                this.RepeatExpression = string.Join(" ", cron);
            }
            catch (Exception)
            {
                // Logger.Log.Error(ex.Message);
            }
        }

        private int CalculateHoursDifferentBeetweenLocalAndServer(bool isLocalEnvironment)
        {
            string timezoneName = isLocalEnvironment ? MailDeliveryConstants.LocalServerTimeZone : MailDeliveryConstants.LiveServerTimeZone;
            DateTime nowDate = DateTime.Now;
            return -(TimeZoneInfo.ConvertTime(nowDate, TimeZoneInfo.FindSystemTimeZoneById(timezoneName)) - nowDate).Hours;
        }
    }
}