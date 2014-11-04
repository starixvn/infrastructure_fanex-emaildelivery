// -----------------------------------------------------------------------
// <copyright file="AutoInsertScheduleJob.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Quartz;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Models;
    using Fanex.MailDelivery.Enumerations;

    /// <summary>
    /// Insert Auto Mail Record Job.
    /// </summary>
    public class InsertAutoMailRecordJob : IJob
    {
        public const string InsertAutoMailRecordJobName = "InsertAutoMailRecordJobName";
        public const string InsertAutoMailRecordJobTriggerName = "InsertAutoMailRecordJobTriggerName";

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
