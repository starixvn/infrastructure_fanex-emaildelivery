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
    /// Check Mail Status Job.
    /// </summary>
    public class CheckMailStatusJob : IJob
    {
        public const string CheckAutoMailStatusJobName = "CheckAutoMailStatusJobName";
        public const string CheckAutoMailStatusJobTriggerName = "CheckAutoMailStatusJobTriggerName";

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
                        if (record.SendTime < DateTime.Now)
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
