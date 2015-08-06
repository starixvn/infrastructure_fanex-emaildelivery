// -----------------------------------------------------------------------
// <copyright file="ClientNotificationJob.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Client.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Quartz;
    using Fanex.MailDelivery.Client.Handlers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ClientNotificationJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            MailClientHandler.PublishClientNotifyMessage();
        }
    }
}
