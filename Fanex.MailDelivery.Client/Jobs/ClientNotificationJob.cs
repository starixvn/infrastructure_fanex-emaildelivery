// -----------------------------------------------------------------------
// <copyright file="ClientNotificationJob.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Client.Jobs
{
    using Fanex.MailDelivery.Client.Handlers;
    using Quartz;

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
