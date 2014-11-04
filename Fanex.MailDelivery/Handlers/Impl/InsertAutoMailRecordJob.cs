namespace Fanex.MailDelivery.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Enumerations;
    using Fanex.MailDelivery.Models;
    using Quartz;

    /// <summary>
    /// Insert Auto Mail Record Job.
    /// </summary>
    public class InsertAutoMailRecordJob : IJob
    {
        public const string InsertAutoMailRecordJobName = "InsertAutoMailRecordJobName";
        public const string InsertAutoMailRecordJobTriggerName = "InsertAutoMailRecordJobTriggerName";

        /// <summary>
        /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
        /// fires that is associated with the <see cref="T:Quartz.IJob" />.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <remarks>
        /// The implementation may wish to set a  result object on the
        /// JobExecutionContext before this method exits.  The result itself
        /// is meaningless to Quartz, but may be informative to
        /// <see cref="T:Quartz.IJobListener" />s or
        /// <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
        /// execution.
        /// </remarks>
        public void Execute(IJobExecutionContext context)
        {
            List<Schedule> availableSchedules = new List<Schedule>(); //// Get from DB

            foreach (var schedule in availableSchedules)
            {
                CronExpression cron = new CronExpression(schedule.RepeatExpression);
                var nextRun = cron.GetNextValidTimeAfter(DateTimeOffset.Now).Value.LocalDateTime;

                //// Insert to DB new record for schedule with status is waiting
                var autoMailRecord = new AutoMailRecord
                {
                    MailStatus = MailStatus.Waiting,
                    Schedule = schedule,
                    SendTime = nextRun
                };
            }

            Console.WriteLine("InsertAutoMailRecordJob run");
        }
    }
}