using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Client
{
    public static class MailClientHandler
    {
        private static IJobHandler jobHandler;
        private static IMailClientMessageBroker messageBroker;

        public static void Start(IJobHandler jobHandler = null, IMailClientMessageBroker messageBroker = null)
        {
            MailClientHandler.jobHandler = jobHandler ?? new JobHandler();
            MailClientHandler.messageBroker = messageBroker ?? new MailClientMessageBroker();

            MailClientHandler.messageBroker.Start(MailClientHandler.jobHandler);
            MailClientHandler.jobHandler.Start();
        }

        public static async Task Stop()
        {
            messageBroker.Stop();

            await jobHandler.Stop();
        }

        public static async Task SendFunctionMailAsync(Mail mail)
        {
            await messageBroker.PublishMailAsync(mail);
        }

        public static async Task<IList<Schedule>> GetSchedulesAsync()
        {
            return await messageBroker.GetSchedulesAsync();
        }

        public static async Task PublishClientNotifyMessageAsync()
        {
            await messageBroker.PublishClientNotifyMessageAsync();
        }

        public static async Task<JobMessage> RequestAddMailTaskForRunningJobAsync(JobMessage jobResult)
        {
            return await messageBroker.RequestAddMailTaskForRunningJobAsync(jobResult);
        }

        internal static async Task<MailTaskModel> RequestMailTaskByIdAsync(GetMailTaskByIdRequest taskID)
        {
            return await messageBroker.RequestMailTaskByIdAsync(taskID);
        }

        public static async Task ReportJobAsync(JobMessage jobResult)
        {
            await messageBroker.PublishJobMessageAsync(jobResult);
        }

        public static async Task RetryJob(Schedule schedule)
        {
            await jobHandler.RetryJob(schedule);
        }
    }
}