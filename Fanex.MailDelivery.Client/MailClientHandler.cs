using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using log4net.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Client
{
    public static class MailClientHandler
    {
        private static IJobHandler jobHandler;
        private static IMailClientMessageBroker messageBroker;

        public static async Task StartAsync(IJobHandler jobHandler = null, IMailClientMessageBroker messageBroker = null)
        {
            XmlConfigurator.Configure();

            MailClientHandler.jobHandler = jobHandler ?? new JobHandler();
            MailClientHandler.messageBroker = messageBroker ?? new MailClientMessageBroker();

            await MailClientHandler.messageBroker.StartAsync(MailClientHandler.jobHandler);

            await MailClientHandler.jobHandler.StartAsync();
        }

        public static async Task StopAsync()
        {
            messageBroker.Stop();

            await jobHandler.StopAsync();
        }

        public static async Task SendFunctionMailAsync(Mail mail)
        {
            await messageBroker.SendFunctionMailAsync(mail);
        }

        public static async Task<IList<Schedule>> GetSchedulesAsync()
        {
            return await messageBroker.GetSchedulesAsync();
        }

        internal static async Task PublishClientNotifyMessageAsync()
        {
            await messageBroker.PublishClientNotifyMessageAsync();
        }

        internal static async Task<JobMessage> RequestAddMailTaskForRunningJobAsync(JobMessage jobResult)
        {
            return await messageBroker.RequestAddMailTaskForRunningJobAsync(jobResult);
        }

        internal static async Task<MailTaskModel> RequestMailTaskByIdAsync(GetMailTaskByIdRequest taskID)
        {
            return await messageBroker.RequestMailTaskByIdAsync(taskID);
        }

        internal static async Task ReportJobAsync(JobMessage jobResult)
        {
            await messageBroker.ReportJobAsync(jobResult);
        }

        internal static async Task RetryJobAsync(Schedule schedule)
        {
            await jobHandler.RetryJobAsync(schedule);
        }
    }
}