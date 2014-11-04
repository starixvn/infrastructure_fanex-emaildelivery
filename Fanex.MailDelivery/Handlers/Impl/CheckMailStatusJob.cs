namespace Fanex.MailDelivery.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.MailDelivery.Enumerations;
    using Fanex.MailDelivery.Models;
    using Quartz;

    /// <summary>
    /// Check Mail Status Job.
    /// </summary>
    public class CheckMailStatusJob : IJob
    {
        public const string CheckAutoMailStatusJobName = "CheckAutoMailStatusJobName";
        public const string CheckAutoMailStatusJobTriggerName = "CheckAutoMailStatusJobTriggerName";

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
            //// Check mail status
            List<MailRecord> autoMailRecords = new List<MailRecord>();

            foreach (var record in autoMailRecords)
            {
                switch (record.MailStatus)
                {
                    case MailStatus.Failed:
                    case MailStatus.DataIsNotReady:
                        //// Send mail notify
                        break;

                    case MailStatus.Waiting:
                    case MailStatus.Running:
                        if (record.SentTime < DateTime.Now)
                        {
                            //// If exceed wait time config, then send notify email
                        }

                        break;

                    default:
                        //// Log
                        break;
                }
            }

            Console.WriteLine("CheckMailStatusJob run");
        }
    }
}