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
    /// TODO: Update summary.
    /// </summary>
    public class CheckAutoMailStatusJob : IJob
    {
        public const string CheckAutoMailStatusJobName = "CheckAutoMailStatusJobName";
        public const string CheckAutoMailStatusJobTriggerName = "CheckAutoMailStatusJobTriggerName";

        public void Execute(IJobExecutionContext context)
        {
            //// Check Auto mail status
            List<AutoMailRecord> autoMailRecords = new List<AutoMailRecord>();

            foreach (var record in autoMailRecords)
            {
                switch (record.MailStatus)
                {
                    case MailStatus.Failed:
                        //// Send mail notify
                        break;
                    case MailStatus.Waiting:
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

            Console.WriteLine("CheckAutoMailStatusJob run");
        }
    }
}
