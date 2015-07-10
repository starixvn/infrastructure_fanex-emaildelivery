namespace Fanex.MailDelivery.Client.Models
{
    using System;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Client.Enumerations;

    /// <summary>
    /// Schedule information.
    /// </summary>
    [Serializable]
    public class Schedule
    {
        public int Id { get; set; }

        public int SiteId { get; set; }

        public int TaskId { get; set; }

        public string Name { get; set; }

        public int TriggerType { get; set; }

        public string RepeatExpression { get; set; }

        public DateTime CurrentFireTime { get; set; }

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
        public void ConvertToServerTime()
        {
            int hoursDifferent = this.CalculateHoursDifferentBeetweenLocalAndServer();
            this.StartTime = this.StartTime.AddHours(hoursDifferent);
            this.CurrentFireTime = this.CurrentFireTime.AddHours(hoursDifferent);
            if (this.EndTime != null)
            {
                this.EndTime = this.EndTime.Value.AddHours(hoursDifferent);
            }

            this.ConvertCronExpressionToServerTime(hoursDifferent);
        }

        /// <summary>
        /// Converts the CRON expression to server time.
        /// </summary>
        /// <param name="hoursDifferent">The hours different.</param>
        private void ConvertCronExpressionToServerTime(int hoursDifferent)
        {
            try
            {
                var cron = this.RepeatExpression.Split(' ');
                var hour = int.Parse(cron[2]);
                hour += hoursDifferent;
                while (hour > 23)
                {
                    hour -= 24;
                }

                cron[2] = hour.ToString();
                this.RepeatExpression = string.Join(" ", cron);
            }
            catch (Exception ex)
            {
                Logging.Logger.Log.Error(ex.Message);
            }
        }

        private int CalculateHoursDifferentBeetweenLocalAndServer()
        {
            string environment = MailClientSection.GetConfig().Environment ?? string.Empty;
            bool isLocal = environment.Length == 0 || environment.Trim().Equals("local") ? true : false;
            string timezoneName = isLocal ? MailDeliveryConstants.LocalServerTimeZone : MailDeliveryConstants.LiveServerTimeZone;
            var nowDate = DateTime.Now;
            return -(TimeZoneInfo.ConvertTime(nowDate, TimeZoneInfo.FindSystemTimeZoneById(timezoneName)) - nowDate).Hours;
        }
    }
}